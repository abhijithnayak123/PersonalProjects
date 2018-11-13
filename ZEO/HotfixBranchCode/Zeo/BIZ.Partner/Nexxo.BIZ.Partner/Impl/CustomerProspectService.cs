using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AutoMapper;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.CXE.Contract;

using ProspectDTO = MGI.Biz.Partner.Data.Prospect;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using PTNRChannelPartner = MGI.Core.Partner.Data.ChannelPartner;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using IPTNRService = MGI.Core.Partner.Contract.IChannelPartnerService;
using ICoreDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using NexxoIdTypeDto = MGI.Core.Partner.Data.NexxoIdType;
using MGI.Common.Util;

using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;

using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Cxn.Common.Processor.Util;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.MoneyTransfer.Contract;

namespace MGI.Biz.Partner.Impl
{
    public class CustomerProspectService : ICustomerProspectService
    {
        private ICoreDataStructureService _ptnrDataStructureService;
        public ICoreDataStructureService PTNRDataStructureService { set { _ptnrDataStructureService = value; } }
        public NLoggerCommon NLogger { get; set; }

        private IPtnrCustomerService _ptnrCustomerService;
        public IPtnrCustomerService PartnerCustomerService { set { _ptnrCustomerService = value; } }

        private IProspectFieldValidator _validator;
        public IProspectFieldValidator ProspectFieldValidator { set { _validator = value; } }

        private ICXECustomerService _cxeCustomerService;
        public ICXECustomerService CXECustomerService { set { _cxeCustomerService = value; } }

        private IPTNRService _ptnrService;
        public IPTNRService ChannelPartnerService { set { _ptnrService = value; } }

        private IChannelPartnerGroupService _ptnrGroupSvc;
        public IChannelPartnerGroupService ChannelPartnerGroupService { set { _ptnrGroupSvc = value; } }

        public MGI.Core.Partner.Contract.IManageLocations LocationService { private get; set; }

        public IProcessorRouter GPRProcessorRouter { private get; set; }
        public IProcessorRouter MoneyTransferProcessorRouter { private get; set; }
        public IProcessorRouter BillPayProcessorRouter { private get; set; }
        public IProcessorRouter CheckProcessorRouter { private get; set; }


        public CustomerProspectService()
        {
            Mapper.CreateMap<PTNRProspect, ProspectDTO>()
                .ForMember(x => x.ID, o => o.Ignore())
                .ForMember(x => x.FName, o => o.MapFrom(s => s.FirstName))
                .ForMember(x => x.LName, o => o.MapFrom(s => s.LastName))
                .ForMember(x => x.LName2, o => o.MapFrom(s => s.LastName2))
                .ForMember(x => x.MName, o => o.MapFrom(s => s.MiddleName))
                .ForMember(x => x.MoMaName, o => o.MapFrom(s => s.MothersMaidenName))
                .ForMember(x => x.PostalCode, o => o.MapFrom(s => s.ZipCode))
                .ForMember(x => x.TextMsgOptIn, o => o.MapFrom(s => s.SMSEnabled))
                .ForMember(x => x.Groups, o => o.MapFrom(s => s.Groups.Select(g => g.ChannelPartnerGroup.Name).ToList()))
                .AfterMap((c, d) =>
                {
                    if (c.GovernmentId != null && c.GovernmentId.IdType != null)
                    {
                        d.ID = new Identification()
                        {
                            Country = (c.GovernmentId.IdType != null && c.GovernmentId.IdType.CountryId != null) ? c.GovernmentId.IdType.CountryId.Name : string.Empty,
                            IssueDate = c.GovernmentId.IssueDate ?? DateTime.MinValue,
                            ExpirationDate = c.GovernmentId.ExpirationDate ?? DateTime.MinValue,
                            IDType = c.GovernmentId.IdType.Name,
                            State = (c.GovernmentId.IdType != null && c.GovernmentId.IdType.StateId != null) ? c.GovernmentId.IdType.StateId.Name : null,
                            GovernmentId = c.GovernmentId.Identification
                        };
                    }
                })

                .AfterMap((c, d) =>
                {
                    if (c.CountryOfBirth != null)
                    {
                        if (d.ID != null)
                            d.ID.CountryOfBirth = c.CountryOfBirth;
                        else
                        {
                            d.ID = new Identification();
                            d.ID.CountryOfBirth = c.CountryOfBirth;
                        }
                    }
                })
                .AfterMap((c, d) =>
                {
                    if (c.EmploymentDetails != null)
                    {
                        d.Employer = c.EmploymentDetails.Employer;
                        d.EmployerPhone = c.EmploymentDetails.EmployerPhone;
                        d.Occupation = c.EmploymentDetails.Occupation;
                        d.OccupationDescription = c.EmploymentDetails.OccupationDescription;
                    }
                });

            NLogger = new NLoggerCommon();
        }

        public string SaveProspect(long agentSessionId, SessionContext sessionContext, ProspectDTO prospectDTO, MGIContext mgiContext)
        {
            NLogger.Info("SaveProspect - initial save");

            ValidateProspect(sessionContext.ChannelPartnerId, prospectDTO);
            PTNRProspect newProspect = new PTNRProspect();
            newProspect.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            newProspect.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);

            UpdateProspect(sessionContext, prospectDTO, newProspect, mgiContext);
            return newProspect.AlloyID.ToString();
        }

        public void SaveProspect(long agentSessionId, SessionContext sessionContext, long alloyId, ProspectDTO prospectDTO, MGIContext mgiContext)
        {
            NLogger.Info("SaveProspect - incremental save");

            ValidateProspect(sessionContext.ChannelPartnerId, prospectDTO);
            PTNRProspect existingProspect = _ptnrCustomerService.LookupProspect(alloyId);
            if (prospectDTO == null)
                throw new BizCustomerException(BizCustomerException.LEAD_NOT_FOUND, string.Format("Prospect for AlloyID {0} not found.", alloyId));

            UpdateProspect(sessionContext, prospectDTO, existingProspect, mgiContext);
        }

        public ProspectDTO GetProspect(long agentSessionId, SessionContext sessionContext, long alloyId, MGIContext mgiContext)
        {
            PTNRProspect prospect = _ptnrCustomerService.LookupProspect(alloyId);

            ProspectDTO prospectDto = Mapper.Map<PTNRProspect, ProspectDTO>(prospect);

            return prospectDto;
        }

        public string ConfirmIdentity(long agentSessionId, long customerSessionId, bool status, MGIContext mgiContext)
        {
            return _ptnrCustomerService.ConfirmIdentity(agentSessionId, customerSessionId, status);
        }

        private void UpdateProspect(SessionContext context, ProspectDTO prospectDTO, PTNRProspect prospect, MGIContext mgiContext)
        {
            //2080
            try
            {
                PTNRChannelPartner channelPartner = _ptnrService.ChannelPartnerConfig(context.ChannelPartnerId);

                UpdateProspectProfile(prospectDTO, prospect);
                if (prospectDTO.ID != null && prospectDTO.CustomerScreen == CustomerScreen.Identification)
                {
                    NexxoIdTypeDto idType = GetIdType(channelPartner.Id, prospectDTO.ID);
                    UpdateIdentification(prospectDTO.ID, idType, prospect);
                }
                UpdateEmploymentDetails(prospectDTO, prospect);
                UpdateProspectGroups(channelPartner, prospectDTO, prospect);



                prospect.EmploymentDetails.DTTerminalCreate = prospect.DTTerminalCreate;
                prospect.EmploymentDetails.DTTerminalLastModified = prospect.DTTerminalLastModified;

                _ptnrCustomerService.SaveProspect(prospect);

                //Update all Provider Accounts
                UpdateProviderAccounts(context, prospect, mgiContext);
            }
            catch (PartnerCustomerException ex)
            {
                throw new PartnerCustomerException(PartnerCustomerException.PROSPECT_SAVE_FAILED, ex.Message);
            }
        }

        private void UpdateProviderAccounts(SessionContext context, PTNRProspect prospect, MGIContext mgiContext)
        {
            MGI.Core.Partner.Data.Customer customer = _ptnrCustomerService.LookupByCxeId(prospect.AlloyID);

            if (customer != null)
            {

                MGI.Core.Partner.Data.Location location = LocationService.Lookup(context.LocationId);

                //Check Account Update
                //Get Provider for Check
                string chkProvider = _GetCheckProvider(mgiContext.ChannelPartnerName);
                int checkProvider = chkProvider == null ? 0 : ((int)Enum.Parse(typeof(ProviderIds), chkProvider));
                MGI.Core.Partner.Data.Account ptnrAcct = customer.GetAccount(checkProvider);
                if (ptnrAcct != null)
                {
                    ICheckProcessor checkProcessor = _GetCheckProcessor(mgiContext.ChannelPartnerName);

                    MGI.Cxn.Check.Data.CheckAccount checkAccount = checkProcessor.GetAccount(ptnrAcct.CXNId);

                    if (checkAccount != null)
                    {
                        checkAccount.FirstName = prospect.FirstName;
                        checkAccount.LastName = prospect.LastName;
                        checkAccount.SecondLastName = prospect.LastName2;
                        checkAccount.Address1 = prospect.Address1;
                        checkAccount.City = prospect.City;
                        checkAccount.State = prospect.State;
                        checkAccount.Zip = prospect.ZipCode;


                        checkAccount.DateOfBirth = (prospect.DateOfBirth == null || prospect.DateOfBirth == DateTime.MinValue) ? null : prospect.DateOfBirth;


                        checkAccount.Phone = prospect.Phone1;
                        checkAccount.SSN = prospect.SSN;
                        checkAccount.IDCode = prospect.IDCode;
                        checkAccount.Occupation = prospect.EmploymentDetails == null ? string.Empty : prospect.EmploymentDetails.Occupation;
                        checkAccount.Employer = prospect.EmploymentDetails == null ? string.Empty : prospect.EmploymentDetails.Employer;
                        checkAccount.EmployerPhone = prospect.EmploymentDetails == null ? string.Empty : prospect.EmploymentDetails.EmployerPhone;
                        checkAccount.GovernmentId = prospect.GovernmentId == null ? string.Empty : prospect.GovernmentId.Identification;
                        checkAccount.IDCountry = (prospect.GovernmentId != null && prospect.GovernmentId.IdType != null && prospect.GovernmentId.IdType.CountryId != null) ? prospect.GovernmentId.IdType.CountryId.Name : string.Empty;

                        if (prospect.GovernmentId != null && prospect.GovernmentId.IdType != null && prospect.GovernmentId.IdType.StateId != null)
                        {
                            checkAccount.IDState = prospect.GovernmentId.IdType.StateId.Abbr;
                        }
                        else
                        {
                            checkAccount.IDState = string.Empty;
                        }

                        checkAccount.IDExpireDate = prospect.GovernmentId == null ? null : prospect.GovernmentId.ExpirationDate == DateTime.MinValue ? null : prospect.GovernmentId.ExpirationDate;
                        checkAccount.IDType = prospect.GovernmentId != null && prospect.GovernmentId.IdType != null ? prospect.GovernmentId.IdType.Name : string.Empty;
                        checkAccount.IDIssueDate = prospect.GovernmentId == null ? null : prospect.GovernmentId.IssueDate == DateTime.MinValue ? null : prospect.GovernmentId.IssueDate;

                        checkProcessor.Update(checkAccount, mgiContext);
                    }
                }

                //Fund Account Update
                //Get Provider for Fund
                string fundProviderName = _GetFundProvider(mgiContext.ChannelPartnerName);
                if (!string.IsNullOrEmpty(fundProviderName))
                {
                    int fundProvider = (int)Enum.Parse(typeof(ProviderIds), fundProviderName);
                    ptnrAcct = customer.GetAccount(fundProvider);
                    if (ptnrAcct != null)
                    {
                        IFundProcessor fundProcessor = _GetFundProcessor(mgiContext.ChannelPartnerName);

                        MGI.Cxn.Fund.Data.CardAccount cardAccount = fundProcessor.LookupCardAccount(ptnrAcct.CXNId, true);

                        if (cardAccount != null)
                        {
                            cardAccount.FirstName = prospect.FirstName;
                            cardAccount.MiddleName = prospect.MiddleName;
                            cardAccount.LastName = prospect.LastName;
                            cardAccount.Address1 = prospect.MailingAddress1;
                            cardAccount.Address2 = prospect.MailingAddress2;
                            cardAccount.City = prospect.MailingCity;
                            cardAccount.State = prospect.MailingState;
                            cardAccount.ZipCode = prospect.MailingZipCode;
                            cardAccount.CountryCode = prospect.CountryOfBirth;
                            cardAccount.DateOfBirth = prospect.DateOfBirth ?? DateTime.MinValue;
                            cardAccount.SSN = prospect.SSN;
                            cardAccount.Phone = prospect.Phone1;
                            cardAccount.PhoneType = prospect.Phone1Type;
                            cardAccount.IDCode = prospect.IDCode;

                            fundProcessor.UpdateAccount(cardAccount, mgiContext);
                        }
                    }
                }
                //BillPay Account Update
                //Get Provider for Billpay
                int billPayProvider = (int)Enum.Parse(typeof(ProviderIds), _GetBillPayProvider(mgiContext.ChannelPartnerName));
                ptnrAcct = customer.GetAccount(billPayProvider);
                if (ptnrAcct != null)
                {
                    IBillPayProcessor billpayProcessor = _GetBillPayProcessor(mgiContext.ChannelPartnerName, billPayProvider);
                    MGI.Cxn.BillPay.Data.BillPayAccount billPayAccount = billpayProcessor.GetBillPayAccount(ptnrAcct.CXNId);

                    if (billPayAccount != null)
                    {
                        billPayAccount.Id = ptnrAcct.CXNId;
                        billPayAccount.FirstName = prospect.FirstName;
                        billPayAccount.LastName = prospect.LastName;
                        billPayAccount.Address1 = prospect.Address1;
                        billPayAccount.Address2 = prospect.Address2;
                        billPayAccount.City = prospect.City;
                        billPayAccount.State = prospect.State;
                        billPayAccount.PostalCode = prospect.ZipCode;
                        billPayAccount.Street = prospect.Address2;
                        //2080
                        billPayAccount.DateOfBirth = (prospect.DateOfBirth == null || prospect.DateOfBirth == DateTime.MinValue) ? null : prospect.DateOfBirth;
                        billPayAccount.Email = prospect.Email;
                        billPayAccount.ContactPhone = prospect.Phone1;
                        billPayAccount.MobilePhone = GetCustomerMobileNumber(prospect);

                        billpayProcessor.UpdateAccount(billPayAccount, mgiContext);
                    }
                }

                //MoneyTransfer Account Update
                //Get Provider for MoneyTransfer
                int moneyTransferProvider = (int)Enum.Parse(typeof(ProviderIds), _GetMoneyTransferProvider(mgiContext.ChannelPartnerName));
                ptnrAcct = customer.GetAccount(moneyTransferProvider);
                if (ptnrAcct != null)
                {
                    IMoneyTransfer moneyTransfer = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);

                    MGI.Cxn.MoneyTransfer.Data.Account moneyTransferAccount = moneyTransfer.GetAccount(ptnrAcct.CXNId, mgiContext);

                    if (moneyTransferAccount != null)
                    {
                        moneyTransferAccount.Address = prospect.Address1;
                        moneyTransferAccount.City = prospect.City;
                        moneyTransferAccount.ContactPhone = prospect.Phone1;
                        moneyTransferAccount.Email = prospect.Email;
                        moneyTransferAccount.FirstName = prospect.FirstName;
                        moneyTransferAccount.LastName = prospect.LastName;
                        moneyTransferAccount.MiddleName = prospect.MiddleName;
                        moneyTransferAccount.SecondLastName = prospect.LastName2;
                        moneyTransferAccount.MobilePhone = GetCustomerMobileNumber(prospect);
                        moneyTransferAccount.PostalCode = prospect.ZipCode;
                        moneyTransferAccount.State = prospect.State;
                        moneyTransferAccount.SmsNotificationFlag = prospect.SMSEnabled ? "Y" : "N";

                        moneyTransfer.UpdateAccount(moneyTransferAccount, mgiContext);
                    }
                }

                //MO  Account Update
            }
        }

        private string GetCustomerMobileNumber(PTNRProspect prospect)
        {
            string mobileNumber = string.Empty;
            if (!string.IsNullOrEmpty(prospect.Phone1) && prospect.Phone1Type == "Cell")
            {
                mobileNumber = prospect.Phone1;
            }
            else if (!string.IsNullOrEmpty(prospect.Phone2) && prospect.Phone2Type == "Cell")
            {
                mobileNumber = prospect.Phone2;
            }
            return mobileNumber;
        }

        //A Method to Get Check Provider based on ChannelPartner
        private string _GetCheckProvider(string channelPartner)
        {
            // get the check provider for the channel partner.
            return CheckProcessorRouter.GetProvider(channelPartner);
        }

        private ICheckProcessor _GetCheckProcessor(string channelPartner)
        {
            // get the check processor for the channel partner.
            return (ICheckProcessor)CheckProcessorRouter.GetProcessor(channelPartner);
        }

        //A Method to Get Fund Provider based on ChannelPartner
        private string _GetFundProvider(string channelPartner)
        {
            // get the fund provider for the channel partner.
            return GPRProcessorRouter.GetProvider(channelPartner);
        }

        private IFundProcessor _GetFundProcessor(string channelPartner)
        {
            // get the check processor for the channel partner.
            return (IFundProcessor)GPRProcessorRouter.GetProcessor(channelPartner);
        }

        //A Method to Get Moneytransfer Provider based on ChannelPartner
        private string _GetMoneyTransferProvider(string channelPartner)
        {
            // get the Moneytransfer provider for the channel partner.
            return MoneyTransferProcessorRouter.GetProvider(channelPartner);
        }

        private IMoneyTransfer _GetMoneyTransferProcessor(string channelPartner)
        {
            // get the BillPay Processor for the channel partner.
            return (IMoneyTransfer)MoneyTransferProcessorRouter.GetProcessor(channelPartner);
        }

        //A Method to Get BillPay Provider based on ChannelPartner
        private string _GetBillPayProvider(string channelPartner)
        {
            // get the BillPay provider for the channel partner.
            return BillPayProcessorRouter.GetProvider(channelPartner);
        }

        private IBillPayProcessor _GetBillPayProcessor(string channelPartner, int providerId)
        {
            MGI.Core.Partner.Contract.ProviderIds enumProviderId = (MGI.Core.Partner.Contract.ProviderIds)providerId;
            // get the BillPay Processor for the channel partner.
            return (IBillPayProcessor)BillPayProcessorRouter.GetProcessor(channelPartner, enumProviderId.ToString());
        }

        private void UpdateProspectProfile(ProspectDTO prospectDTO, PTNRProspect prospect)
        {
            prospect.FirstName = (prospectDTO.FName ?? "").ToUpper();
            prospect.MiddleName = (prospectDTO.MName ?? "").ToUpper();
            prospect.LastName = (prospectDTO.LName ?? "").ToUpper();
            prospect.LastName2 = (prospectDTO.LName2 ?? "").ToUpper();
            prospect.MothersMaidenName = (prospectDTO.MoMaName ?? "").ToUpper();
            prospect.Address1 = (prospectDTO.Address1 ?? "").ToUpper();
            prospect.Address2 = (prospectDTO.Address2 ?? "").ToUpper();
            prospect.City = (prospectDTO.City ?? "").ToUpper();
            prospect.State = (prospectDTO.State ?? "").ToUpper();
            prospect.ZipCode = prospectDTO.PostalCode;
            prospect.CountryOfBirth = prospectDTO.ID == null || (prospectDTO.ID != null && string.IsNullOrEmpty(prospectDTO.ID.CountryOfBirth)) ? "" : prospectDTO.ID.CountryOfBirth;

            prospect.ClientID = string.IsNullOrEmpty(prospectDTO.ClientID) ? "" : prospectDTO.ClientID;
            prospect.LegalCode = string.IsNullOrEmpty(prospectDTO.LegalCode) ? "" : prospectDTO.LegalCode;
            prospect.PrimaryCountryCitizenShip = string.IsNullOrEmpty(prospectDTO.PrimaryCountryCitizenShip) ? "" : prospectDTO.PrimaryCountryCitizenShip;
            prospect.SecondaryCountryCitizenShip = string.IsNullOrEmpty(prospectDTO.SecondaryCountryCitizenShip) ? "" : prospectDTO.SecondaryCountryCitizenShip;

            if (prospectDTO.DateOfBirth == DateTime.MinValue)
                prospect.DateOfBirth = null;
            else
                prospect.DateOfBirth = prospectDTO.DateOfBirth;

            prospect.Email = (prospectDTO.Email ?? "").ToUpper();
            prospect.Gender = (prospectDTO.Gender ?? "").ToUpper();
            prospect.Phone1 = prospectDTO.Phone1;
            prospect.Phone1Provider = prospectDTO.Phone1Provider;
            prospect.Phone1Type = prospectDTO.Phone1Type;
            prospect.Phone2 = prospectDTO.Phone2;
            prospect.Phone2Provider = prospectDTO.Phone2Provider;
            prospect.Phone2Type = prospectDTO.Phone2Type;

            prospect.DoNotCall = prospectDTO.DoNotCall;

            prospect.MailingAddressDifferent = prospectDTO.MailingAddressDifferent;
            prospect.MailingAddress1 = (prospectDTO.MailingAddress1 ?? "").ToUpper();
            prospect.MailingAddress2 = (prospectDTO.MailingAddress2 ?? "").ToUpper();
            prospect.MailingCity = (prospectDTO.MailingCity ?? "").ToUpper();
            prospect.MailingState = (prospectDTO.MailingState ?? "").ToUpper();
            prospect.MailingZipCode = prospectDTO.MailingZipCode;

            prospect.IsAccountHolder = prospectDTO.IsAccountHolder;
            prospect.SSN = prospectDTO.SSN;
            prospect.PIN = prospectDTO.PIN;
            prospect.ReferralCode = prospectDTO.ReferralCode;
            prospect.ChannelPartnerId = prospectDTO.ChannelPartnerId;
            prospect.ReceiptLanguage = prospectDTO.ReceiptLanguage;
            prospect.ProfileStatus = prospectDTO.ProfileStatus;
            prospect.SMSEnabled = prospectDTO.TextMsgOptIn;
            prospect.Notes = prospectDTO.Notes;
            prospect.IDCode = prospectDTO.SSN == null ? string.Empty : NexxoUtil.GetIDCode(prospect.SSN);
        }

        private void UpdateIdentification(Identification prospectID, NexxoIdTypeDto idType, PTNRProspect prospect)
        {

            DateTime? expirationDate;
            if (prospectID.ExpirationDate == DateTime.MinValue)
                expirationDate = null;
            else
                expirationDate = prospectID.ExpirationDate;

            DateTime? issueDate;
            if (prospectID.IssueDate == DateTime.MinValue)
                issueDate = null;
            else
                issueDate = prospectID.IssueDate;

            if (prospect.GovernmentId == null)
            {
                prospect.AddGovernmentId(idType, prospectID.GovernmentId, issueDate, expirationDate);
                prospect.GovernmentId.DTTerminalCreate = prospect.DTTerminalCreate;
            }
            else
            {
                prospect.UpdateGovernmentId(idType, prospectID.GovernmentId, issueDate, expirationDate);
                prospect.GovernmentId.DTTerminalLastModified = prospect.DTTerminalLastModified;
            }

        }

        private void UpdateEmploymentDetails(ProspectDTO prospectDTO, PTNRProspect prospect)
        {
            string occupation = prospectDTO.Occupation == null ? string.Empty : prospectDTO.Occupation.ToUpper();
            string description = prospectDTO.OccupationDescription == null ? string.Empty : prospectDTO.OccupationDescription.ToUpper();
            string employer = prospectDTO.Employer == null ? string.Empty : prospectDTO.Employer.ToUpper();
            if (prospect.EmploymentDetails == null)
                prospect.AddEmploymentDetails(occupation, description, employer, prospectDTO.EmployerPhone);
            else
                prospect.UpdateEmploymentDetails(occupation, description, employer, prospectDTO.EmployerPhone);
        }

        private void UpdateProspectGroups(PTNRChannelPartner channelPartner, ProspectDTO prospectDTO, PTNRProspect prospect)
        {
            if (prospectDTO.Groups.Count > 0)
            {
                List<ChannelPartnerGroup> cpGroups = _ptnrGroupSvc.GetAll(channelPartner.rowguid);
                List<string> ptnrCustomerGroups = prospect.Groups.Select(g => g.ChannelPartnerGroup.Name).ToList();

                // get list of groups that need to be added and removed
                var toAdd = prospectDTO.Groups.Except(ptnrCustomerGroups);
                var toRemove = ptnrCustomerGroups.Except(prospectDTO.Groups);

                // remove any group settings for groups in "toRemove"
                List<ProspectGroupSetting> groupSettingsList = prospect.Groups.ToList();

                foreach (ProspectGroupSetting g in groupSettingsList)
                {
                    if (toRemove.Contains(g.ChannelPartnerGroup.Name))
                        prospect.Groups.Remove(g);
                }
                // add group settings for groups in "toAdd"
                toAdd.ToList().ForEach(m => prospect.AddtoGroup(cpGroups.Find(g => g.Name == m)));
                prospect.Groups.ToList().ForEach(x => x.DTTerminalCreate = prospect.DTTerminalCreate);
                prospect.Groups.ToList().ForEach(x => x.DTTerminalLastModified = prospect.DTTerminalLastModified);
            }
            else if (prospect.Groups.Count > 0)
            {
                // if there are existing group settings that need to be removed
                prospect.Groups.Clear();
            }
        }

        private NexxoIdTypeDto GetIdType(long channelPartnerId, Identification prospectID)
        {
            NexxoIdTypeDto idType = null;
            if (prospectID != null && !string.IsNullOrEmpty(prospectID.Country) && !string.IsNullOrEmpty(prospectID.IDType))
            {
                idType = _ptnrDataStructureService.Find(channelPartnerId, prospectID.IDType, prospectID.Country, prospectID.State);
                if (idType == null)
                    throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND, string.Format("Could not find Identification Type {0} {1}", prospectID.Country, prospectID.IDType));
            }
            return idType;
        }

        private void ValidateProspect(long channelPartnerId, ProspectDTO prospect)
        {
            if (prospect != null)
            {
                _validator.ValidateNames(prospect);
                _validator.ValidateAddress(prospect);

                _validator.ValidatePhone(prospect);
                _validator.ValidateEmail(prospect);

                if (prospect.ID != null && prospect.CustomerScreen == CustomerScreen.Identification)
                {
                    NexxoIdTypeDto idType = null;
                    if (!string.IsNullOrEmpty(prospect.ID.Country) && !string.IsNullOrEmpty(prospect.ID.IDType))
                    {
                        idType = _ptnrDataStructureService.Find(channelPartnerId, prospect.ID.IDType, prospect.ID.Country, prospect.ID.State);
                        if (idType == null)
                            throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_ID_TYPE_NOT_FOUND, string.Format("Could not find Identification Type {0} {1}", prospect.ID.Country, prospect.ID.IDType));
                    }
                    //Author : Abhijith
                    //User Story: AL-1626
                    //Starts Here
                    var channelPartner = _ptnrService.ChannelPartnerConfig(channelPartnerId);
                    if (channelPartner != null && channelPartner.ChannelPartnerConfig != null)
                        _validator.ValidateDOB(prospect, channelPartner.ChannelPartnerConfig.CustomerMinimumAge);
                    //Ends Here

                    _validator.ValidateID(prospect.ID, idType);
                }

                if (prospect.CustomerScreen == CustomerScreen.Employment)
                {
                    _validator.ValidateEmployment(prospect);
                }
            }
        }
    }
}
