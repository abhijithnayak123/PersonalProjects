using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Alloy.CXN.Customer.Data;
using MGI.Alloy.CXN.Customer.FIS.Data;
using MGI.Alloy.CXN.Customer.FIS.Impl.FISService;
using MGI.Alloy.Common.Data;
using MGI.Alloy.Common.Util;
using AutoMapper;

using P3Net.Data.Common;
using P3Net.Data;
using P3Net.Data.Sql;

using System.Data;
using System.Text;

namespace MGI.Alloy.CXN.Customer.FIS.Impl
{
	public class FISIOImpl
	{
		wsdlNexxoPolicyClient client = null;

        private const string PrimaryOfficeNumber = "33333";
		private const string SecondaryOfficeNumber = "33333";
		private const string SrcId = "Nexxo";
        
        #region AO
        #region Public Methods

        /// <summary>
        /// FetchAll Customers by CustomerLookupCriteria
        /// </summary>
        /// <param name="context"></param>
        /// <param name="customerLookUpCriteria"></param>
        /// <returns></returns>
        internal List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria searchCriteria, ZeoContext context)
        {
            FISCredential credential = GetFISCredential(searchCriteria.ChannelPartnerId, searchCriteria.AgentBankId);

            CICustTaxNbrSrchMtvnSvcReq request = new CICustTaxNbrSrchMtvnSvcReq();
            request.MsgUUID = Guid.NewGuid().ToString();
            request.PrcsParms = new CICustTaxNbrSrchMtvnSvcReqPrcsParms() { SrcID = SrcId };

            CICustTaxNbrSrchMtvnSvcReqSvc baserequest = new CICustTaxNbrSrchMtvnSvcReqSvc();

            baserequest.Security = new CICustTaxNbrSrchMtvnSvcReqSvcSecurity()
            {
                Item = new CICustTaxNbrSrchMtvnSvcReqSvcSecurityBasicAuth()
                {
                    UsrID = credential.RACFUserId,
                    Pwd = credential.RACFPassword
                }
            };

            baserequest.SvcParms = new CICustTaxNbrSrchMtvnSvcReqSvcSvcParms()
            {
                ApplID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsApplID.CI,
                RqstUUID = Guid.NewGuid().ToString(),
                RoutingID = searchCriteria.AgentBankId,
                SvcID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch,
                SvcNme = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch.ToString(),
                SvcVer = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcVer.Item60
            };

            baserequest.MsgData = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData();
            CICustTaxNbrSrchReqData ciCustTaxNbrSrchReqData = new CICustTaxNbrSrchReqData();

            //Add parameters to the search request
            ciCustTaxNbrSrchReqData.E10202 = searchCriteria.SSN;
            ciCustTaxNbrSrchReqData.E10292 = "I";

            if (!string.IsNullOrWhiteSpace(searchCriteria.Zipcode))
                ciCustTaxNbrSrchReqData.E10091 = searchCriteria.Zipcode;

            if (!string.IsNullOrWhiteSpace(searchCriteria.Phonenumber))
                ciCustTaxNbrSrchReqData.E10201 = searchCriteria.Phonenumber;

            baserequest.MsgData.CICustTaxNbrSrchReqData = ciCustTaxNbrSrchReqData;

            CICustTaxNbrSrchMtvnSvcReqSvc[] requestArray = new CICustTaxNbrSrchMtvnSvcReqSvc[1];
            requestArray[0] = baserequest;

            request.Svc = requestArray;

            CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(request);

            CICustTaxNbrSrchMtvnSvcResSvcMsgData responseMsgData = (CICustTaxNbrSrchMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
            CICustTaxNbrSrchResData responseData = (CICustTaxNbrSrchResData)responseMsgData.Item;

            List<CICustTaxNbrSrchResDataCICustInfo> customerInfoList = new List<CICustTaxNbrSrchResDataCICustInfo>();

            if (responseData != null && responseData.CICustInfoLst != null && responseData.CICustInfoLst.Any())
            {
                if (responseData.ApplMsgLst != null)
                {
                    IEnumerable<Error> errormessage = Mapper.Map<CICustTaxNbrSrchResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
                    RaiseException(errormessage);
                }
                customerInfoList = responseData.CICustInfoLst.ToList();
            }

            if (!string.IsNullOrEmpty(responseData.E10031) && responseData.E10031 == "Y") //Check whether any more records are there to be returned
            {
                FetchRemainingRecords(request, ref customerInfoList, responseData.E10208);
            }

            List<CustomerProfile> customers = CustomerMapper(customerInfoList);
            if (!customers.IsNullorEmpty())
            {
                GetCustomerInqList(credential, customers);
            }
            return customers;
        }
        #endregion
        #region Private Methods
        /// <summary>
        ///  Map the response from Synovus API call to a collection of customer profiles.
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="channelPartnerId"></param>
        /// <returns></returns>
        private List<CustomerProfile> CustomerMapper(List<CICustTaxNbrSrchResDataCICustInfo> customerInfoList)
        {
            List<CustomerProfile> customerprofiles = null;
            if (!customerInfoList.IsNullorEmpty())
            {
                customerprofiles = FISMapper.Map(customerInfoList);
            }
            return customerprofiles;
        }

        /// <summary>
        /// Get customerInformation
        /// </summary>
        /// <param name="credential"></param>
        /// <param name="fisAccounts"></param>
        private void GetCustomerInqList(FISCredential credential, List<CustomerProfile> customers)
        {
            string requestId = Guid.NewGuid().ToString();
            CICustInqMtvnSvcReq request = new CICustInqMtvnSvcReq()
            {
                MsgUUID = requestId,
                MtvnSvcVer = CICustInqMtvnSvcReqMtvnSvcVer.Item10,
                PrcsParms = new CICustInqMtvnSvcReqPrcsParms()
                {
                    SrcID = SrcId
                }
            };

            foreach (var customer in customers)
            {
                CICustInqMtvnSvcReqSvc baseRequest = new CICustInqMtvnSvcReqSvc()
                {
                    Security = new CICustInqMtvnSvcReqSvcSecurity()
                    {
                        Item = new CICustInqMtvnSvcReqSvcSecurityBasicAuth()
                        {
                            UsrID = credential.RACFUserId,
                            Pwd = credential.RACFPassword
                        }
                    },
                    SvcParms = new CICustInqMtvnSvcReqSvcSvcParms()
                    {
                        ApplID = CICustInqMtvnSvcReqSvcSvcParmsApplID.CI,
                        SvcID = CICustInqMtvnSvcReqSvcSvcParmsSvcID.CICustInq,
                        SvcVer = CICustInqMtvnSvcReqSvcSvcParmsSvcVer.Item30,
                        RqstUUID = requestId,
                        RoutingID = credential.BankId
                    },
                    MsgData = new CICustInqMtvnSvcReqSvcMsgData()
                    {
                        CICustInqReqData = new CICustInqReqData()
                        {
                            E10093 = customer.ClientCustomerId
                        }
                    }
                };

                request.Svc = new CICustInqMtvnSvcReqSvc[] { baseRequest };
                CICustInqMtvnSvcRes response = client.CICustInq(request);

                CICustInqMtvnSvcResSvcMsgData responseMsgData = (CICustInqMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
                CICustInqResData responseData = responseMsgData.Item as CICustInqResData;

                if (responseData != null)
                {
                    customer.DateOfBirth = (!string.IsNullOrWhiteSpace(responseData.E10036)) ? Convert.ToDateTime(responseData.E10036) : customer.DateOfBirth;
                    customer.MothersMaidenName = (!string.IsNullOrWhiteSpace(responseData.E10169)) ? responseData.E10169 : customer.MothersMaidenName;
                    customer.EmployerName = (!string.IsNullOrWhiteSpace(responseData.E10159)) ? responseData.E10159 : null;
                    customer.Occupation = (!string.IsNullOrWhiteSpace(responseData.E10171)) ? MapToOccupation(responseData.E10171) : null;
                    customer.IdType = (!string.IsNullOrWhiteSpace(responseData.E10704)) ? MapToNexxoIDType(responseData.E10704) : null;
                    customer.IdNumber = (!string.IsNullOrWhiteSpace(responseData.E10705)) ? responseData.E10705 : null;
                    customer.IdIssuingCountry = GetIssusingCountry(customer.IdType);
                    customer.IdIssuingState = (!string.IsNullOrWhiteSpace(responseData.E10707)) ? MapToState(responseData.E10707, customer.IdType) : null;
                    customer.IdIssueDate = (!string.IsNullOrWhiteSpace(responseData.E10708)) ? Convert.ToDateTime(responseData.E10708) : customer.IdIssueDate;
                    customer.IdExpirationDate = (!string.IsNullOrWhiteSpace(responseData.E10709)) ? Convert.ToDateTime(responseData.E10709) : customer.IdExpirationDate;
                }

            }
        }

        private FISCredential GetFISCredential(long channelPartnerId, string bankId)
        {
            StoredProcedure fiscredentialprocedure = new StoredProcedure("usp_getfiscredential");
            fiscredentialprocedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
            fiscredentialprocedure.WithParameters(InputParameter.Named("BankId").WithValue(bankId));
            IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(fiscredentialprocedure);
            FISCredential credential = null;

            while (datareader.Read())
            {
                credential = new FISCredential();
                credential.Applicationkey = datareader.GetStringOrDefault("ApplicationKey");
                credential.BankId = bankId;
                credential.ChannelKey = datareader.GetStringOrDefault("ChannelKey");
                credential.ChannelPartnerId = channelPartnerId;
                credential.MetBankNumber = datareader.GetStringOrDefault("MetBankNumber");
                credential.User = datareader.GetStringOrDefault("UserName");
                credential.Password = datareader.GetStringOrDefault("Password");
            }

            if (credential == null)
                { throw new Exception(); }

            client = new wsdlNexxoPolicyClient();

            client.ClientCredentials.UserName.UserName = credential.User;
            client.ClientCredentials.UserName.Password = credential.Password;

            responseType svcresponse = null;
            //TODO:: URGH!!! TOTAL HACK !!! FIS TEST GATEWAY IS WIERD, GOES OFFLINE OFTEN, FIRST TRY FAILS, AND SECOND USUALLY SUCCEEDS, LETS SEE HOW THIS WORKS !!!
            int counter = 0;
            while (counter < 3)
            {
                try
                {
                    svcresponse = client.synovussoaapplicationkeygetAppKeyxbd(new requestType() { ApplKy = credential.Applicationkey, ChannelKy = credential.ChannelKey, MetBankNumber = credential.MetBankNumber, MsgID = Guid.NewGuid().ToString() });
                    break;
                }
                catch { }
                counter++;
            }

            if (svcresponse == null || svcresponse.MsgData == null)
            {
                throw new Exception("Service Error in getting Application Info");
            }

            credential.RACFUserId = svcresponse.MsgData.MetUsername;
            credential.RACFPassword = svcresponse.MsgData.MetPassword;

            return credential;
        }
        private class Error
        {
            public string MethodName { get; set; }
            public string ElementId { get; set; }
            public string ErrorNumber { get; set; }
            public string ErrorMessage { get; set; }
        }

        private string MapToOccupation(string id)
        {
            string occupation = string.Empty;

            Dictionary<string, string> occupationMapping = new Dictionary<string, string>()
            {
                {"O", "OTHER"},
                {"101", "MEDICAL-OTHER"},
                {"102", "CLERICAL"},
                {"103", "DATA PROCESSING"},
                {"104", "SELF-EMPLOYED"},
                {"105", "MANUFACTURING"},
                {"106", "CONSTRUCTION"},
                {"107", "GOVERNMENT - STATE"},
                {"108", "UTILITIES"},
                {"109", "BANKING"},
                {"110", "SERVICE INDUSTRY"},
                {"111", "EDUCATION "},
                {"112", "GOVERNMENT - LOCAL"},
                {"113", "MILITARY/CIVIL SER"},
                {"114", "RETIRED"},
                {"115", "UNEMPLOYED"},
                {"116", "MINOR"},
                {"117", "HOMEMAKER"},
                {"118", "MEDICAL-PHYSICIAN"},
                {"119", "MEDICAL-NURSE"},
                {"120", "MEDICAL-HOME HEALTH"},
                {"121", "ATTORNEY"},
                {"122", "ACCOUNTANT"},
                {"123", "REAL ESTATE BROKER"},
                {"124", "PAWN BROKER"},
                {"125", "INSURANCE"},
                {"126", "AUCTION"}
            };

            if (!string.IsNullOrWhiteSpace(id) && occupationMapping.ContainsKey(id))
            {
                occupation = occupationMapping[id];
            }
            return occupation;
        }

        private string MapToNexxoIDType(string id)
        {
            string nexxoId = string.Empty;

            Dictionary<string, string> idMappying = new Dictionary<string, string>()
            {
                {"D", "DRIVER'S LICENSE"},
                {"EAC", "EMPLOYMENT AUTHORIZATION CARD (EAD)"},
                {"M", "MILITARY ID"},
				// Passport is not supported in the first release as we don't know how to map the country
				//{"P", "PASSPORT"},
				{"S", "U.S. STATE IDENTITY CARD"},
                {"MC", "MATRICULA CONSULAR"},
                {"PR", "GREEN CARD / PERMANENT RESIDENT CARD"},
                {"RA", "GREEN CARD / PERMANENT RESIDENT CARD"}
            };

            if (idMappying.ContainsKey(id))
            {
                nexxoId = idMappying[id];
            }
            return nexxoId;
        }

        private string MapToState(string location, string IDType)
        {
            string state = string.Empty;
            if (string.Compare(IDType, "P", true) == 0)
            {
                return string.Empty;
            }

            if (location.Contains(","))
            {
                var locations = location.Split(',');
                if (location.Length > 1)
                {
                    state = locations[1].Trim();
                }
            }
            else
            {
                state = location;
            }

            return state;
        }

        private string GetIssusingCountry(string IDType)
        {
            string country = string.Empty;

            if (string.IsNullOrWhiteSpace(IDType))
            {
                return country;
            }

            switch (IDType)
            {
                case "MC":
                    country = "MEXICO";
                    break;
                case "P":
                    break;
                default:
                    country = "UNITED STATES";
                    break;
            }

            return country;
        }

        private void RaiseException(IEnumerable<Error> errormessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Error occurred processing your request");
            foreach (var error in errormessage)
            {
                sb.AppendLine("Error Number: " + error.ErrorNumber);
                sb.AppendLine("Element Id: " + error.ElementId);
                sb.AppendLine("Error Message: " + error.ErrorMessage);
            }
            if (sb.Length > 0)
            {
                throw new Exception(sb.ToString());
            }
        }

        private void FetchRemainingRecords(CICustTaxNbrSrchMtvnSvcReq request, ref List<CICustTaxNbrSrchResDataCICustInfo> customerInfoList, string custNumberToStartSearch)
        {
            request.Svc[0].MsgData.CICustTaxNbrSrchReqData.E10208 = custNumberToStartSearch;
            CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(request);

            CICustTaxNbrSrchMtvnSvcResSvcMsgData responseMsgData = (CICustTaxNbrSrchMtvnSvcResSvcMsgData)response.Svc[0].MsgData;

            CICustTaxNbrSrchResData responseData = (CICustTaxNbrSrchResData)responseMsgData.Item;
            if (responseData != null && responseData.CICustInfoLst != null && responseData.CICustInfoLst.Any())
            {
                if (responseData.ApplMsgLst != null)
                {
                    IEnumerable<Error> errormessage = Mapper.Map<CICustTaxNbrSrchResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
                    RaiseException(errormessage);
                }

                var result = customerInfoList.Concat(responseData.CICustInfoLst.ToList());
                customerInfoList = result.ToList();

                if (!string.IsNullOrEmpty(responseData.E10031) && responseData.E10031 == "Y")
                {
                    FetchRemainingRecords(request, ref customerInfoList, responseData.E10208);
                }
            }
        }

        #endregion
        #endregion


    }
}
