using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.Customer.FIS.Impl.FISService;
using MGI.Core.Partner.Data;
using AutoMapper;
using System.Diagnostics;
using MGI.TimeStamp;
using MGI.Common.Util;


namespace MGI.Cxn.Customer.FIS.Impl
{
	public class FISIOImpl
	{
		wsdlNexxoPolicyClient client = null;

		public IRepository<FISCredential> FISCredentialRepo { private get; set; }
		public IRepository<FISError> FISErrorRepo { private get; set; }

		private const string PrimaryOfficeNumber = "33333";
		private const string SecondaryOfficeNumber = "33333";
		private const string SrcId = "Nexxo";
		public NLoggerCommon NLogger { get; set; }
		private const string GenericErrorMessage = "Unknown error, report issue to System Administrator";


		public FISIOImpl()
		{
			AutoMapper.Mapper.CreateMap<CICustNameAddrMaintResDataApplMsg, Error>()
				.ForMember(x => x.ElementId, y => y.MapFrom(z => z.ApplMsgFld))
				.ForMember(x => x.ErrorNumber, y => y.MapFrom(z => z.ApplMsgNbr))
				.ForMember(x => x.ErrorMessage, y => y.MapFrom(z => z.ApplMsgTxt));

			AutoMapper.Mapper.CreateMap<CICustTaxNbrSrchResDataApplMsg, Error>()
				.ForMember(x => x.ElementId, y => y.MapFrom(z => z.ApplMsgFld))
				.ForMember(x => x.ErrorNumber, y => y.MapFrom(z => z.ApplMsgNbr))
				.ForMember(x => x.ErrorMessage, y => y.MapFrom(z => z.ApplMsgTxt));

			AutoMapper.Mapper.CreateMap<CIOpenIndvCustResDataApplMsg, Error>()
				.ForMember(x => x.ElementId, y => y.MapFrom(z => z.ApplMsgFld))
				.ForMember(x => x.ErrorNumber, y => y.MapFrom(z => z.ApplMsgNbr))
				.ForMember(x => x.ErrorMessage, y => y.MapFrom(z => z.ApplMsgTxt));

			AutoMapper.Mapper.CreateMap<CIOpenMiscAcctResDataApplMsg, Error>()
				.ForMember(x => x.ElementId, y => y.MapFrom(z => z.ApplMsgFld))
				.ForMember(x => x.ErrorNumber, y => y.MapFrom(z => z.ApplMsgNbr))
				.ForMember(x => x.ErrorMessage, y => y.MapFrom(z => z.ApplMsgTxt));
			Mapper.CreateMap<CICustTaxNbrSrchResDataCICustInfo, FIS.Data.FISAccount>()
				.ForMember(d => d.PartnerAccountNumber, o => o.MapFrom(s => s.E10033))
				.ForMember(d => d.Address1, o => o.MapFrom(s => s.E10042))
				.ForMember(d => d.City, o => o.MapFrom(s => s.E10094))
				.ForMember(d => d.DateOfBirth, o => o.MapFrom(s => Convert.ToDateTime(s.E10036)))
				.ForMember(d => d.FirstName, o => o.MapFrom(s => s.E10102))
				.ForMember(d => d.LastName, o => o.MapFrom(s => s.E10101))
				.ForMember(d => d.MiddleName, o => o.MapFrom(s => s.E10103))
				.ForMember(d => d.Phone1, o => o.MapFrom(s => s.E10109))
				.ForMember(d => d.Phone2, o => o.MapFrom(s => s.E10113))
				.ForMember(d => d.SSN, o => o.MapFrom(s => s.E10132))
				.ForMember(d => d.IDCode, o => o.MapFrom(s => s.E10134 == "T" ? "I" : s.E10134))
				.ForMember(d => d.State, o => o.MapFrom(s => s.E10114))
				.ForMember(d => d.Gender, o => o.MapFrom(s => string.IsNullOrEmpty(s.E10153)
														? s.E10153
														: (s.E10153 == "F" ? "Female" : "Male")))
				.AfterMap((s, d) =>
							  {
								  if (!string.IsNullOrEmpty(s.E10122))
								  {
									  d.ZipCode = s.E10122.Trim();
									  if (d.ZipCode.Length > 5)
									  {
										  d.ZipCode = s.E10122.Trim().Substring(0, 5);
									  }
								  }
								  if (!string.IsNullOrEmpty(s.E10109))
								  {
									  d.Phone1 = s.E10109;
									  d.Phone1Type = "Home";
								  }
								  else if (!string.IsNullOrEmpty(s.E10113))
								  {
									  d.Phone1 = s.E10113;
									  d.Phone1Type = "Work";
								  }
								  else if (!string.IsNullOrEmpty(s.E10097))
								  {
									  d.Phone1 = s.E10097;
									  d.Phone1Type = "Other";
								  }

							  });
		}

		/// <summary>
		/// Search the customer by SSN from Synovus
		/// </summary>
		/// <param name="mgiContext"> The context should have the following:
		/// 1. ChannelPartnerId
		/// 2. BankId
		/// 3. SSN
		/// </param>
		/// <returns></returns>
		public FIS.Data.FISAccount SearchCustomerBySSN(MGIContext mgiContext)
		{
			FISCredential credential = GetFISCredential(mgiContext.ChannelPartnerId, mgiContext.BankId);

			CICustTaxNbrSrchMtvnSvcReq request = new CICustTaxNbrSrchMtvnSvcReq();
			request.MsgUUID = Guid.NewGuid().ToString();
			request.MtvnSvcVer = CICustTaxNbrSrchMtvnSvcReqMtvnSvcVer.Item10;

			request.PrcsParms = new CICustTaxNbrSrchMtvnSvcReqPrcsParms() { SrcID = SrcId };

			CICustTaxNbrSrchMtvnSvcReqSvc baserequest = new CICustTaxNbrSrchMtvnSvcReqSvc();
			baserequest.Security = new CICustTaxNbrSrchMtvnSvcReqSvcSecurity() { Item = new CICustTaxNbrSrchMtvnSvcReqSvcSecurityBasicAuth() { UsrID = credential.RACFUserId, Pwd = credential.RACFPassword } };

			baserequest.SvcParms = new CICustTaxNbrSrchMtvnSvcReqSvcSvcParms() { ApplID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), RoutingID = mgiContext.BankId, SvcID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch, SvcNme = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch.ToString(), SvcVer = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcVer.Item60 };

			baserequest.MsgData = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData() { CICustTaxNbrSrchReqData = new CICustTaxNbrSrchReqData() { E10202 = mgiContext.Context["SSN"].ToString(), E10292 = "I" } };

			CICustTaxNbrSrchMtvnSvcReqSvc[] requestArray = new CICustTaxNbrSrchMtvnSvcReqSvc[1];
			requestArray[0] = baserequest;

			request.Svc = requestArray;

			NLogger.Info(string.Format("Connecting to financial connect service - CICustTaxNbrSrch - SearchCustomerBySSN - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));
			CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(request);
			NLogger.Info(string.Format("Retrieved the customer details - CICustTaxNbrSrch - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

			CICustTaxNbrSrchMtvnSvcResSvcMsgData responseMsgData = (CICustTaxNbrSrchMtvnSvcResSvcMsgData)response.Svc[0].MsgData;

			CICustTaxNbrSrchResData responseData = (CICustTaxNbrSrchResData)responseMsgData.Item;

			FIS.Data.FISAccount fisAccount = SearchCustomerBySSNResponseMapper(responseData, mgiContext.ChannelPartnerId);

			if (fisAccount != null)
			{
				GetCustomerInq(credential, fisAccount);
			}

			return fisAccount;
		}

		/// <summary>
		/// FetchAll Customers by CustomerLookupCriteria
		/// </summary>
		/// <param name="context"></param>
		/// <param name="customerLookUpCriteria"></param>
		/// <returns></returns>
		public List<FIS.Data.FISAccount> FetchAll(System.Collections.Generic.Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
		{
			//long channelPartnerId = cxnContext.ChannelPartnerId;
			//string bankId = cxnContext.BankId;
			FISCredential credential = GetFISCredential(cxnContext.ChannelPartnerId, cxnContext.BankId);

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
				RoutingID = cxnContext.BankId,
				SvcID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch,
				SvcNme = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch.ToString(),
				SvcVer = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcVer.Item60
			};

			baserequest.MsgData = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData();
			CICustTaxNbrSrchReqData ciCustTaxNbrSrchReqData = new CICustTaxNbrSrchReqData();

			//Add parameters to the search request
			ciCustTaxNbrSrchReqData.E10202 = customerLookUpCriteria["SSN"].ToString();
			ciCustTaxNbrSrchReqData.E10292 = "I";

			if (customerLookUpCriteria.ContainsKey("ZipCode"))
				ciCustTaxNbrSrchReqData.E10091 = customerLookUpCriteria["ZipCode"].ToString();

			if (customerLookUpCriteria.ContainsKey("PhoneNumber"))
				ciCustTaxNbrSrchReqData.E10201 = customerLookUpCriteria["PhoneNumber"].ToString();

			baserequest.MsgData.CICustTaxNbrSrchReqData = ciCustTaxNbrSrchReqData;

			CICustTaxNbrSrchMtvnSvcReqSvc[] requestArray = new CICustTaxNbrSrchMtvnSvcReqSvc[1];
			requestArray[0] = baserequest;

			request.Svc = requestArray;

			NLogger.Info(string.Format("Connecting to financial connect service - CICustTaxNbrSrch - SearchCustomerBySSN,PhoneNumber,Zipcode - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));
			CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(request);
			NLogger.Info(string.Format("Retrieved the customer details - CICustTaxNbrSrch - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

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

			List<FIS.Data.FISAccount> fisAccounts = FetchAllResponseMapper(customerInfoList, cxnContext.ChannelPartnerId);
			if (fisAccounts != null && fisAccounts.Any())
			{
				GetCustomerInqList(credential, fisAccounts);
			}

			return fisAccounts;
		}

		private void FetchRemainingRecords(CICustTaxNbrSrchMtvnSvcReq request, ref  List<CICustTaxNbrSrchResDataCICustInfo> customerInfoList, string custNumberToStartSearch)
		{
			NLogger.Info(string.Format("Connecting to financial connect service - CICustTaxNbrSrch - SearchCustomerBySSN,PhoneNumber,Zipcode - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));
			request.Svc[0].MsgData.CICustTaxNbrSrchReqData.E10208 = custNumberToStartSearch;
			CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(request);
			NLogger.Info(string.Format("Retrieved the customer details - CICustTaxNbrSrch - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

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

		/// <summary>
		///  Map the response from Synovus API call to a collection of customer profiles.
		/// </summary>
		/// <param name="responseData"></param>
		/// <param name="channelPartnerId"></param>
		/// <returns></returns>
		private List<FISAccount> FetchAllResponseMapper(List<CICustTaxNbrSrchResDataCICustInfo> customerInfoList, long channelPartnerId)
		{
			List<FIS.Data.FISAccount> customerprofiles = null;
			if (customerInfoList.Any())
			{
				customerprofiles = Mapper.Map<List<CICustTaxNbrSrchResDataCICustInfo>, List<FIS.Data.FISAccount>>(customerInfoList);
			}
			return customerprofiles;
		}

		public string CreateFISCustomer(FIS.Data.FISAccount customer, MGIContext context)
		{
			FISCredential credential = GetFISCredential(context.ChannelPartnerId, context.BankId);

			//Creating the request object
			CIOpenIndvCustMtvnSvcReq request = new FISService.CIOpenIndvCustMtvnSvcReq();

			//Creating the respons object
			FISService.CIOpenIndvCustMtvnSvcRes reseponse = new FISService.CIOpenIndvCustMtvnSvcRes();


			request.MsgUUID = Guid.NewGuid().ToString();
			request.MtvnSvcVer = CIOpenIndvCustMtvnSvcReqMtvnSvcVer.Item10;
			request.PrcsParms = new CIOpenIndvCustMtvnSvcReqPrcsParms() { SrcID = SrcId }; //ToDo:

			CIOpenIndvCustMtvnSvcReqSvc baserequest = new CIOpenIndvCustMtvnSvcReqSvc();
			baserequest.Security = new CIOpenIndvCustMtvnSvcReqSvcSecurity() { Item = new CIOpenIndvCustMtvnSvcReqSvcSecurityBasicAuth() { UsrID = credential.RACFUserId, Pwd = credential.RACFPassword } };
			baserequest.SvcParms = new CIOpenIndvCustMtvnSvcReqSvcSvcParms() { ApplID = (CIOpenIndvCustMtvnSvcReqSvcSvcParmsApplID)MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ReqSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), RoutingID = context.BankId, SvcID = CIOpenIndvCustMtvnSvcReqSvcSvcParmsSvcID.CIOpenIndvCust, SvcNme = CxnFISEnum.ReqSvcParmsSvcID.CIOpenIndvCust.ToString(), SvcVer = CIOpenIndvCustMtvnSvcReqSvcSvcParmsSvcVer.Item40 };


			FISService.CIOpenIndvCustReqData reqData = OpenIndividualAccountRequestMapper(customer, context);

			CIOpenIndvCustMtvnSvcReqSvcMsgData msgdata = new CIOpenIndvCustMtvnSvcReqSvcMsgData();
			msgdata.CIOpenIndvCustReqData = reqData;
			baserequest.MsgData = msgdata;

			CIOpenIndvCustMtvnSvcReqSvc[] svcrequestArray = new CIOpenIndvCustMtvnSvcReqSvc[1];
			svcrequestArray[0] = baserequest;
			request.Svc = svcrequestArray;

			NLogger.Info(string.Format("Start - Connecting to financial connect service - CIOpenIndvCust - CreateFISCustomer - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));
			//Calling the Service method 
			reseponse = client.CIOpenIndvCust(request);
			NLogger.Info(string.Format("End - CIOpenIndvCust - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));

			reseponse.ErrMsg = (reseponse != null && string.IsNullOrWhiteSpace(reseponse.ErrMsg)) ? GenericErrorMessage : reseponse.ErrMsg;
			CustOpenIndvCustRes indvRes = new CustOpenIndvCustRes();

			CIOpenIndvCustMtvnSvcResSvcMsgData resmsgdata = new CIOpenIndvCustMtvnSvcResSvcMsgData();
			resmsgdata = reseponse.Svc[0].MsgData;
			CIOpenIndvCustResData indvCustResData = new CIOpenIndvCustResData();
			indvCustResData = (CIOpenIndvCustResData)resmsgdata.Item;

			return OpenIndividualAccountResponseMapper(indvCustResData, request, context.BankId, context);
		}

		//Used to update Name, phone, address on FIS customer record
		public void UpdateCustomerProfile(FIS.Data.FISAccount customerprofile, MGIContext context)
		{
			//long channelPartnerId = Convert.ToInt64(NexxoUtil.GetDictionaryValue(context, "ChannelPartnerId"));

			//string bankId = Convert.ToString(NexxoUtil.GetDictionaryValue(context, "BankID"));

			FISCredential credential = GetFISCredential(context.ChannelPartnerId, context.BankId);

			//Creating the request object
			CICustNameAddrMaintMtvnSvcReq request = new CICustNameAddrMaintMtvnSvcReq();

			//Creating the response object 
			CustNameAddrMaintRes custUpdateResponse = new CustNameAddrMaintRes();

			request.MsgUUID = Guid.NewGuid().ToString();
			request.MtvnSvcVer = CICustNameAddrMaintMtvnSvcReqMtvnSvcVer.Item10;
			request.PrcsParms = new CICustNameAddrMaintMtvnSvcReqPrcsParms() { SrcID = SrcId };

			CICustNameAddrMaintMtvnSvcReqSvc baserequest = new CICustNameAddrMaintMtvnSvcReqSvc();

			baserequest.Security = new CICustNameAddrMaintMtvnSvcReqSvcSecurity() { Item = new CICustNameAddrMaintMtvnSvcReqSvcSecurityBasicAuth() { UsrID = credential.RACFUserId, Pwd = credential.RACFPassword } };
			baserequest.SvcParms = new CICustNameAddrMaintMtvnSvcReqSvcSvcParms() { ApplID = (CICustNameAddrMaintMtvnSvcReqSvcSvcParmsApplID)MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ReqSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), SvcID = CICustNameAddrMaintMtvnSvcReqSvcSvcParmsSvcID.CICustNameAddrMaint, RoutingID = context.BankId, SvcNme = CxnFISEnum.ReqSvcParmsSvcID.CICustNameAddrMaint.ToString(), SvcVer = (CICustNameAddrMaintMtvnSvcReqSvcSvcParmsSvcVer)MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ReqSvcParmsSvcVer.Item30 };

			CICustNameAddrMaintReqData CustNameAddrReqData = UpdateCustomerRequestMapper(customerprofile);


			CICustNameAddrMaintMtvnSvcReqSvcMsgData msgdata = new CICustNameAddrMaintMtvnSvcReqSvcMsgData();
			msgdata.CICustNameAddrMaintReqData = CustNameAddrReqData;
			baserequest.MsgData = msgdata;

			CICustNameAddrMaintMtvnSvcReqSvc[] requestArray = new CICustNameAddrMaintMtvnSvcReqSvc[1];
			requestArray[0] = baserequest;
			request.Svc = requestArray;

			NLogger.Info(string.Format("Start - Connecting to financial connect service - CICustNameAddrMaint - UpdateCustomerProfile - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));
			//Creating the response object
			CICustNameAddrMaintMtvnSvcRes response = client.CICustNameAddrMaint(request);
			NLogger.Info(string.Format("End - CICustNameAddrMaint - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));

			CICustNameAddrMaintMtvnSvcResSvcMsgData responseMsgData = (CICustNameAddrMaintMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
			CICustNameAddrMaintResData responseData = (CICustNameAddrMaintResData)responseMsgData.Item;

			UpdateCustomerResponseMapper(responseData);
		}

		//Used to associate  account relationship in FIS for Connections and for Connects customers with their FIS customer ID        
		public bool CreateMiscAccount(FIS.Data.FISAccount customerprofile, MGIContext mgiContext)
		{
			NLogger.Info("Inside CreateMiscAccount");

			//DE1971 - Fix , change bankId from location.BankID to FISAccount.BankId
			//if (context.ContainsKey("AccountNumber") && context["AccountNumber"] != null)
			//    { bankId = customerprofile.BankId.ToString(); }

			//Modified the above to handle it in different way.

			NLogger.Info("Before getting fis credential");
			FISCredential credential = GetFISCredential(mgiContext.ChannelPartnerId, mgiContext.BankId);
			//Creating the request object
			CIOpenMiscAcctMtvnSvcReq request = new CIOpenMiscAcctMtvnSvcReq();
			//Creating the respons object
			CIOpenMiscAcctMtvnSvcRes response = new CIOpenMiscAcctMtvnSvcRes();

			request.MsgUUID = Guid.NewGuid().ToString();
			request.MtvnSvcVer = CIOpenMiscAcctMtvnSvcReqMtvnSvcVer.Item10;
			request.PrcsParms = new CIOpenMiscAcctMtvnSvcReqPrcsParms() { SrcID = SrcId };

			CIOpenMiscAcctMtvnSvcReqSvc baserequest = new CIOpenMiscAcctMtvnSvcReqSvc();
			baserequest.Security = new CIOpenMiscAcctMtvnSvcReqSvcSecurity() { Item = new CIOpenMiscAcctMtvnSvcReqSvcSecurityBasicAuth() { UsrID = credential.RACFUserId, Pwd = credential.RACFPassword } };
			baserequest.SvcParms = new CIOpenMiscAcctMtvnSvcReqSvcSvcParms() { ApplID = (CIOpenMiscAcctMtvnSvcReqSvcSvcParmsApplID)MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ReqSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), SvcID = CIOpenMiscAcctMtvnSvcReqSvcSvcParmsSvcID.CIOpenMiscAcct, RoutingID = customerprofile.BankId, SvcNme = CxnFISEnum.ReqSvcParmsSvcID.CIOpenMiscAcct.ToString(), SvcVer = (CIOpenMiscAcctMtvnSvcReqSvcSvcParmsSvcVer)MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ReqSvcParmsSvcVer.Item30 };

			NLogger.Info("Before getting request object from mapper");
			CIOpenMiscAcctReqData reqData = OpenMiscAccountRequestMapper(customerprofile, mgiContext);
			NLogger.Info("After getting request object from mapper");
			CIOpenMiscAcctMtvnSvcReqSvcMsgData msgdata = new CIOpenMiscAcctMtvnSvcReqSvcMsgData();
			msgdata.CIOpenMiscAcctReqData = reqData;
			baserequest.MsgData = msgdata;

			CIOpenMiscAcctMtvnSvcReqSvc[] requestArray = new CIOpenMiscAcctMtvnSvcReqSvc[1];
			requestArray[0] = baserequest;
			request.Svc = requestArray;

			NLogger.Info(string.Format("Start - Connecting to financial connect service - CIOpenMiscAcct - CreateMiscAccount - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));
			//Calling the Service method
			response = client.CIOpenMiscAcct(request);
			NLogger.Info(string.Format("End - CIOpenMiscAcct - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));

			CIOpenMiscAcctMtvnSvcResSvcMsgData responseMsgData = (CIOpenMiscAcctMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
			CIOpenMiscAcctResData responseData = (CIOpenMiscAcctResData)responseMsgData.Item;

			return OpenMiscAccountResponseMapper(responseData, request, mgiContext.BankId, mgiContext);
		}

		/// <summary>
		/// Map the response from Synovus API call to a collection of customer profile.
		/// </summary>
		/// <param name="responseData"></param>
		/// <param name="channelPartnerId"></param>
		/// <returns></returns>
		private FISAccount SearchCustomerBySSNResponseMapper(CICustTaxNbrSrchResData responseData, long channelPartnerId)
		{
			FIS.Data.FISAccount customerprofile = null;

			if (responseData.CICustInfoLst != null)
			{
				// for all practical reasons there should be only 1 customer record for a given ssn for a given branch.

				if (responseData.CICustInfoLst.Count() > 1)
					throw new ClientCustomerException(ClientCustomerException.MULTIPLE_ACCOUNT_FOUND, "Found more than one customer with same ssn");

				if (responseData.ApplMsgLst != null)
				{
					IEnumerable<Error> errormessage = Mapper.Map<CICustTaxNbrSrchResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
					RaiseException(errormessage);
				}

				CICustTaxNbrSrchResDataCICustInfo customerinfo = responseData.CICustInfoLst.FirstOrDefault();
				if (customerinfo != null)
				{
					customerprofile = new FIS.Data.FISAccount();
					customerprofile.PartnerAccountNumber = customerinfo.E10033;
					// DE2363 Fix Start
					//customerprofile.Address1 = customerinfo.E10115;
					customerprofile.Address1 = customerinfo.E10042;
					//customerprofile.Address2 = customerinfo.E10043;
					// DE2363 Fix End
					customerprofile.City = customerinfo.E10094;
					customerprofile.DateOfBirth = Convert.ToDateTime(customerinfo.E10036);
					customerprofile.FirstName = customerinfo.E10102;
					customerprofile.LastName = customerinfo.E10101;
					customerprofile.MiddleName = customerinfo.E10103;
					customerprofile.Phone1 = customerinfo.E10109;
					customerprofile.SSN = customerinfo.E10132;
					customerprofile.IDCode = customerinfo.E10134 == "T" ? "I" : customerinfo.E10134;//Mapping I instead of T as, as per FIS T - ITIN
					customerprofile.State = customerinfo.E10114;
					customerprofile.ZipCode = customerinfo.E10122;
					customerprofile.Gender = customerinfo.E10153;

					if (!string.IsNullOrEmpty(customerinfo.E10122))
					{
						customerprofile.ZipCode = customerinfo.E10122.Trim();
						if (customerprofile.ZipCode.Length > 5)
						{
							customerprofile.ZipCode = customerinfo.E10122.Trim().Substring(0, 5);
						}
					}

					if (!string.IsNullOrEmpty(customerinfo.E10109))
					{
						customerprofile.Phone1 = customerinfo.E10109;
						customerprofile.Phone1Type = "Home";
					}
					else if (!string.IsNullOrEmpty(customerinfo.E10113))
					{
						customerprofile.Phone1 = customerinfo.E10113;
						customerprofile.Phone1Type = "Work";
					}
					else if (!string.IsNullOrEmpty(customerinfo.E10097))
					{
						customerprofile.Phone1 = customerinfo.E10097;
						customerprofile.Phone1Type = "Other";
					}

					#region No Correspoding Elements For Mapping
					//customerprofile.Address2 // No corresponding elemement.
					//customerprofile.CardNumber // No corresponding element.
					//customerprofile.DoNotCall  // No corresponding element.
					//customerprofile.Email // No corresponding element.
					//customerprofile.Gender  // No corresponding element.
					//customerprofile.IsPartnerAccountHolder // No corresponding element
					//customerprofile.LastName2 // No corresponding element
					//customerprofile.MailingAddress1 = customerinfo.E10115; // Is this same as Address ?
					//customerprofile.MailingAddress2 // No corresponding element
					//customerprofile.MailingAddressDifferent // No corresponding element
					//customerprofile.MailingCity // No corresponding element
					//customerprofile.MailingState // No corresponding element
					//customerprofile.MailingZipCode // No corresponding element
					//customerprofile.MarketingSMSEnabled //No corresponding element
					//customerprofile.MothersMaidenName // No corresponding element
					//customerprofile.Phone1Provider // No corresponding element
					//customerprofile.Phone1Type // No corresponding element
					//customerprofile.Phone2 // No corresponding element
					//customerprofile.Phone2Provider // No corresponding element
					//customerprofile.Phone2Type // No corresponding element
					//customerprofile.PIN // No corresponding element
					//customerprofile.ProfileStatus // No corresponding element
					//customerprofile.ReceiptLanguage // No corresponding element
					//customerprofile.ReferralCode // No corresponding element
					//customerprofile.SMSEnabled // No corresponding element
					//customerprofile.TaxpayerId // No corresponding element
					#endregion
				}
			}

			// if we have come this far, then there is atleast 1 customer profile, so we return the hashset with customer profile.
			return customerprofile;
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
				throw new ClientCustomerException(ClientCustomerException.PROVIDER_ERROR, sb.ToString());
			}
		}

		// Map the FISAccount object with the request tags.
		private CIOpenIndvCustReqData OpenIndividualAccountRequestMapper(FIS.Data.FISAccount customer, MGIContext context)
		{
			CIOpenIndvCustReqData requestdata = new CIOpenIndvCustReqData();

			//string BranchId = context.ContainsKey("BranchId") ? context["BranchId"].ToString() : string.Empty;
			//string BankId = context.ContainsKey("BankId") ? context["BankId"].ToString() : string.Empty;

			requestdata.E10036 = Convert.ToDateTime(customer.DateOfBirth).ToString("yyyy-MM-dd");
			requestdata.E10070 = customer.FirstName + " " + customer.MiddleName + " " + customer.LastName;
			requestdata.E10071 = customer.Address1;
			requestdata.E10072 = customer.City + " " + customer.State + " " + customer.ZipCode;
			requestdata.E10076 = CxnFISEnum.CISAddressLineCode.N.ToString();
			requestdata.E10077 = CxnFISEnum.CISAddressLineCode.S.ToString();
			requestdata.E10078 = CxnFISEnum.CISAddressLineCode.C.ToString();
			requestdata.E10094 = customer.City;
			requestdata.E10098 = customer.FirstName + " " + customer.MiddleName;
			requestdata.E10109 = customer.Phone1.Replace("-", "").Replace("(", "").Replace(")", "");
			requestdata.E10113 = string.IsNullOrEmpty(customer.Phone2) ? null : customer.Phone2.Replace("-", "").Replace("(", "").Replace(")", "");
			requestdata.E10114 = customer.State;
			requestdata.E10121 = CxnFISEnum.CISCustomerTypeIndicator.I.ToString();
			requestdata.E10122 = customer.ZipCode;
			requestdata.E10132 = customer.SSN;
			requestdata.E10134 = customer.IDCode == "I" ? "T" : customer.IDCode; //Passing T instead of I as, as per FIS T - ITIN
			requestdata.E10153 = string.IsNullOrEmpty(customer.Gender) ? "" : (customer.Gender.ToLower() == "male" ? "M" : "F");
			requestdata.E10169 = customer.MothersMaidenName;
			requestdata.E10051 = customer.BranchId;
			requestdata.E10123 = PrimaryOfficeNumber; //"010";// Primary Officer #
			requestdata.E10128 = SecondaryOfficeNumber;  //"012";// Secondary Officer #
			requestdata.E10053 = customer.BankId + customer.BranchId;   //"001"; // Cost Center #
			requestdata.E10118 = customer.LastName + " " + customer.LastName2;
			//requestdata.E10159  // customer profile has no occupation information
			requestdata.E10704 = !string.IsNullOrEmpty(customer.GovernmentIDType) ? MapToFISIDType(customer.GovernmentIDType) : string.Empty;
			requestdata.E10705 = customer.GovernmentId;
			requestdata.E10706 = customer.IDIssuingCountry;
			requestdata.E10708 = customer.IDIssueDate != null ? Convert.ToDateTime(customer.IDIssueDate).ToString("yyyy-MM-dd") : null;
			requestdata.E10709 = customer.IDExpirationDate != DateTime.MinValue ? Convert.ToDateTime(customer.IDExpirationDate).ToString("yyyy-MM-dd") : null;
			requestdata.E10707 = customer.IDIssuingState;
			requestdata.E10113 = customer.Phone2;

			return requestdata;
		}

		// Process the response and return FIS Account number, else raise exception.
		private string OpenIndividualAccountResponseMapper(CIOpenIndvCustResData responseData, CIOpenIndvCustMtvnSvcReq request, string bankId, MGIContext mgiContext)
		{
			if (responseData.ApplMsgLst != null)
			{
				IEnumerable<Error> errormessage = Mapper.Map<CIOpenIndvCustResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
				LogFISError(request, bankId, errormessage, mgiContext);
				RaiseException(errormessage);
				return string.Empty;
			}
			else
				return responseData.E10033;
		}

		// Map the FISAccount object with the request tags.
		private CIOpenMiscAcctReqData OpenMiscAccountRequestMapper(FIS.Data.FISAccount customerprofile, MGIContext mgiContext)
		{
			string accounttype = mgiContext.Context.ContainsKey("AccountType") && mgiContext.Context["AccountType"] != null ? mgiContext.Context["AccountType"].ToString() : null;
			CIOpenMiscAcctReqData requestdata = new CIOpenMiscAcctReqData();

			requestdata.E10176 = customerprofile.PartnerAccountNumber;
			requestdata.E10183 = CxnFISEnum.CISRelationshipIndicator.P.ToString();
			requestdata.E16000 = CxnFISEnum.CISAccountLineCode.N.ToString();
			requestdata.E16001 = CxnFISEnum.CISAccountLineCode.S.ToString();
			requestdata.E16002 = CxnFISEnum.CISAccountLineCode.C.ToString();
			requestdata.E16008 = customerprofile.FirstName + " " + customerprofile.MiddleName + " " + customerprofile.LastName;
			requestdata.E16009 = customerprofile.Address1;
			requestdata.E16010 = customerprofile.City + " " + customerprofile.State + " " + customerprofile.ZipCode;
			requestdata.E16055 = customerprofile.RelationshipAccountNumber;

			if (!string.IsNullOrEmpty(accounttype))
			{
				if (accounttype.ToLower() == CxnFISEnum.ConnectionsType.PREPD.ToString().ToLower())
				{
					requestdata.E16054 = CxnFISEnum.ConnectionsType.PREPD.ToString();
					requestdata.E16055 = customerprofile.RelationshipAccountNumber;
				}

				if (accounttype.ToLower() == CxnFISEnum.ConnectionsType.CNECT.ToString().ToLower())
				{ requestdata.E16054 = CxnFISEnum.ConnectionsType.CNECT.ToString(); }
			}
			return requestdata;
		}

		private bool OpenMiscAccountResponseMapper(CIOpenMiscAcctResData responseData, CIOpenMiscAcctMtvnSvcReq request, string bankId, MGIContext context)
		{
			if (responseData.ApplMsgLst != null)
			{
				IEnumerable<Error> errorMessages = Mapper.Map<CIOpenMiscAcctResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
				//RaiseException(errormessage);
				LogFISError(request, bankId, errorMessages, context);

				return false;
			}
			else
				return true;
		}

		private void LogFISError(CIOpenMiscAcctMtvnSvcReq request, string bankId, IEnumerable<Error> errorMessages, MGIContext mgiContext)
		{
			if (errorMessages.Any())
			{
				string timeZone = string.Empty;
				string branchId = null;
				if (!string.IsNullOrEmpty(mgiContext.TimeZone))
				{
					timeZone = mgiContext.TimeZone;
				}
				if (mgiContext.BranchId != 0)
				{
					branchId = Convert.ToString(mgiContext.BranchId);
				}

				Error error = errorMessages.FirstOrDefault();
				CIOpenMiscAcctReqData requestData = GetRequestData(request);
				FISError fisError = null;

				if (error != null)
				{
					fisError = new FISError()
					{
						AppID = error.ElementId,
						ErrorNumber = error.ErrorNumber,
						ErrorMessage = error.ErrorMessage,
						BankID = bankId,
						NexxoCustomerId = requestData != null ? requestData.E16055 : string.Empty,
						FISRelationshipIndicator = requestData != null ? requestData.E10183 : string.Empty,
						FISAddressLineCode1 = requestData != null ? requestData.E16000 : string.Empty,
						FISAddressLineCode2 = requestData != null ? requestData.E16001 : string.Empty,
						FISAddressLineCode3 = requestData != null ? requestData.E16002 : string.Empty,
						FISCurrentNameAddressLine1 = requestData != null ? requestData.E16008 : string.Empty,
						FISCurrentNameAddressLine2 = requestData != null ? requestData.E16008 : string.Empty,
						FISCurrentNameAddressLine3 = requestData != null ? requestData.E16010 : string.Empty,
						FISAccountNumber = requestData != null ? requestData.E10176 : string.Empty,
						FISAcountType = requestData != null ? requestData.E16054 : string.Empty,
						BranchId = branchId,
						NxoEvent = GetNexxoEvent(requestData.E16054),
						DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone),
						DTServerCreate = DateTime.Now,
						DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone),
						DTServerLastModified = DateTime.Now
					};
				}

				try
				{
					FISErrorRepo.Add(fisError);
				}
				catch { }
			}
		}

		private void LogFISError(CIOpenIndvCustMtvnSvcReq request, string bankId, IEnumerable<Error> errorMessages, MGIContext mgiContext)
		{
			if (errorMessages.Any())
			{
				string timeZone = string.Empty;
				string branchId = null;
				if (!string.IsNullOrEmpty(mgiContext.TimeZone))
				{
					timeZone = mgiContext.TimeZone;
				}
				if (mgiContext.BranchId != 0)
				{
					branchId = Convert.ToString(mgiContext.BranchId);
				}

				Error error = errorMessages.FirstOrDefault();
				CIOpenIndvCustReqData requestData = GetRequestData(request);
				FISError fisError = null;

				if (error != null)
				{
					fisError = new FISError()
					{
						AppID = error.ElementId,
						ErrorNumber = error.ErrorNumber,
						ErrorMessage = error.ErrorMessage,
						BankID = bankId,
						NexxoCustomerId = string.Empty,
						FISRelationshipIndicator = string.Empty,
						FISAddressLineCode1 = requestData != null ? requestData.E10076 : string.Empty,
						FISAddressLineCode2 = requestData != null ? requestData.E10077 : string.Empty,
						FISAddressLineCode3 = requestData != null ? requestData.E10078 : string.Empty,
						FISCurrentNameAddressLine1 = requestData != null ? requestData.E10070 : string.Empty,
						FISCurrentNameAddressLine2 = requestData != null ? requestData.E10071 : string.Empty,
						FISCurrentNameAddressLine3 = requestData != null ? requestData.E10072 : string.Empty,
						FISAccountNumber = string.Empty,
						FISAcountType = "CORE",
						BranchId = branchId,
						NxoEvent = "Customer Registration",
						DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone),
						DTServerCreate = DateTime.Now
					};
				}

				try
				{
					FISErrorRepo.Add(fisError);
				}
				catch { }
			}
		}

		private string GetNexxoEvent(string accountType)
		{
			string nexxoEvent = "UnKnown";
			if (string.Compare(accountType, "CNECT", true) == 0)
			{
				nexxoEvent = "Customer Registration";
			}
			else if (string.Compare(accountType, "PREPD", true) == 0)
			{
				nexxoEvent = "GPR Registration";
			}
			return nexxoEvent;
		}

		private CIOpenMiscAcctReqData GetRequestData(CIOpenMiscAcctMtvnSvcReq request)
		{
			CIOpenMiscAcctReqData requestData = null;
			CIOpenMiscAcctMtvnSvcReqSvc[] svc = request.Svc;
			if (svc.Length > 0)
			{
				CIOpenMiscAcctMtvnSvcReqSvc service = svc.FirstOrDefault();
				if (service != null)
				{
					requestData = ((CIOpenMiscAcctMtvnSvcReqSvcMsgData)service.MsgData).CIOpenMiscAcctReqData;
				}
			}
			return requestData;
		}

		private CIOpenIndvCustReqData GetRequestData(CIOpenIndvCustMtvnSvcReq request)
		{
			CIOpenIndvCustReqData requestData = null;
			CIOpenIndvCustMtvnSvcReqSvc[] svc = request.Svc;
			if (svc.Length > 0)
			{
				CIOpenIndvCustMtvnSvcReqSvc service = svc.FirstOrDefault();
				if (service != null)
				{
					requestData = ((CIOpenIndvCustMtvnSvcReqSvcMsgData)service.MsgData).CIOpenIndvCustReqData;
				}
			}
			return requestData;
		}

		// Map the FISAccount object with the request tags.
		private CICustNameAddrMaintReqData UpdateCustomerRequestMapper(FIS.Data.FISAccount customerprofile)
		{
			CICustNameAddrMaintReqData requestdata = new CICustNameAddrMaintReqData();

			requestdata.E10033 = customerprofile.PartnerAccountNumber; // where do we get this from?
			requestdata.E10070 = customerprofile.FirstName + " " + customerprofile.MiddleName + " " + customerprofile.LastName;
			requestdata.E10071 = customerprofile.Address1;
			requestdata.E10072 = customerprofile.City + " " + customerprofile.State + " " + customerprofile.ZipCode;
			requestdata.E10076 = CxnFISEnum.CISAccountLineCode.N.ToString();
			requestdata.E10077 = CxnFISEnum.CISAccountLineCode.S.ToString();
			requestdata.E10078 = CxnFISEnum.CISAccountLineCode.C.ToString();
			requestdata.E10109 = customerprofile.Phone1;
			requestdata.E10113 = customerprofile.Phone2;

			return requestdata;
		}

		// Map the response tags to response object
		private void UpdateCustomerResponseMapper(CICustNameAddrMaintResData responseData)
		{
			if (responseData.ApplMsgLst != null)
			{
				IEnumerable<Error> errormessage = Mapper.Map<CICustNameAddrMaintResDataApplMsg[], IEnumerable<Error>>(responseData.ApplMsgLst);
				RaiseException(errormessage);
			}
		}

		private FISCredential GetFISCredential(long channelPartnerId, string bankId)
		{
			FISCredential credential = FISCredentialRepo.FindBy(x => x.BankId == bankId && x.ChannelPartnerId == channelPartnerId);

			if (credential == null)
			{ throw new ClientCustomerException(ClientCustomerException.FIS_CREDENTIALS_NOT_FOUND, "Unable to get credentials for FIS Service from database"); }

			client = null;
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
					NLogger.Info(string.Format("Start - Connecting to financial connect service - synovussoaapplicationkeygetAppKeyxbd - GetFISCredential - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));
					svcresponse = client.synovussoaapplicationkeygetAppKeyxbd(new requestType() { ApplKy = credential.Applicationkey, ChannelKy = credential.ChannelKey, MetBankNumber = credential.MetBankNumber, MsgID = Guid.NewGuid().ToString() });
					NLogger.Info(string.Format("End - synovussoaapplicationkeygetAppKeyxbd - {0} : {1}", this.GetType().Name, DateTime.Now.ToString()));
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

		#region CICustInq related methods
		/// <summary>
		/// Get customerInformation
		/// </summary>
		/// <param name="credential"></param>
		/// <param name="fisAccounts"></param>
		private void GetCustomerInqList(FISCredential credential, List<FIS.Data.FISAccount> fisAccounts)
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

			#region BaseRequest

			foreach (var fisAccount in fisAccounts)
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
							E10093 = fisAccount.PartnerAccountNumber
						}
					}
				};

				request.Svc = new CICustInqMtvnSvcReqSvc[] { baseRequest };

				NLogger.Info(string.Format("Connecting to financial connect service - CICustInq - CICustInq - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));
				CICustInqMtvnSvcRes response = client.CICustInq(request);
				NLogger.Info(string.Format("Retrieved the customer details - CICustInq - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

				CICustInqMtvnSvcResSvcMsgData responseMsgData = (CICustInqMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
				CICustInqResData responseData = responseMsgData.Item as CICustInqResData;

				if (responseData != null)
				{
					fisAccount.DateOfBirth = (!string.IsNullOrWhiteSpace(responseData.E10036)) ? Convert.ToDateTime(responseData.E10036) : fisAccount.DateOfBirth;
					fisAccount.MothersMaidenName = (!string.IsNullOrWhiteSpace(responseData.E10169)) ? responseData.E10169 : fisAccount.MothersMaidenName;
					fisAccount.EmployerName = (!string.IsNullOrWhiteSpace(responseData.E10159)) ? responseData.E10159 : null;
					fisAccount.Occupation = (!string.IsNullOrWhiteSpace(responseData.E10171)) ? MapToOccupation(responseData.E10171) : null;
					fisAccount.GovernmentIDType = (!string.IsNullOrWhiteSpace(responseData.E10704)) ? MapToNexxoIDType(responseData.E10704) : null;
					fisAccount.GovernmentId = (!string.IsNullOrWhiteSpace(responseData.E10705)) ? responseData.E10705 : null;
					fisAccount.IDIssuingCountry = GetIssusingCountry(fisAccount.GovernmentIDType);
					fisAccount.IDIssuingState = (!string.IsNullOrWhiteSpace(responseData.E10707)) ? MapToState(responseData.E10707, fisAccount.GovernmentIDType) : null;
					fisAccount.IDIssueDate = (!string.IsNullOrWhiteSpace(responseData.E10708)) ? Convert.ToDateTime(responseData.E10708) : fisAccount.IDIssueDate;
					fisAccount.IDExpirationDate = (!string.IsNullOrWhiteSpace(responseData.E10709)) ? Convert.ToDateTime(responseData.E10709) : fisAccount.IDIssueDate;
				}

			}

			#endregion

		}

		private void GetCustomerInq(FISCredential credential, FIS.Data.FISAccount fisAccount)
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

			#region  BaseRequest

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
						E10093 = fisAccount.PartnerAccountNumber
					}
				}
			};
			#endregion

			request.Svc = new CICustInqMtvnSvcReqSvc[] { baseRequest };

			NLogger.Info(string.Format("Connecting to financial connect service - CICustInq - CICustInq - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));
			CICustInqMtvnSvcRes response = client.CICustInq(request);
			NLogger.Info(string.Format("Retrieved the customer details - CICustInq - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

			CICustInqMtvnSvcResSvcMsgData responseMsgData = (CICustInqMtvnSvcResSvcMsgData)response.Svc[0].MsgData;
			CICustInqResData responseData = responseMsgData.Item as CICustInqResData;

			CustomerInqMapper(responseData, ref fisAccount);
		}

		private void CustomerInqMapper(CICustInqResData responseData, ref FIS.Data.FISAccount fisAccount)
		{
			if (responseData != null)
			{
				fisAccount.DateOfBirth = (!string.IsNullOrWhiteSpace(responseData.E10036)) ? Convert.ToDateTime(responseData.E10036) : fisAccount.DateOfBirth;
				fisAccount.MothersMaidenName = (!string.IsNullOrWhiteSpace(responseData.E10169)) ? responseData.E10169 : fisAccount.MothersMaidenName;
				fisAccount.EmployerName = (!string.IsNullOrWhiteSpace(responseData.E10159)) ? responseData.E10159 : null;
				fisAccount.Occupation = (!string.IsNullOrWhiteSpace(responseData.E10171)) ? MapToOccupation(responseData.E10171) : null;
				fisAccount.GovernmentIDType = (!string.IsNullOrWhiteSpace(responseData.E10704)) ? MapToNexxoIDType(responseData.E10704) : null;
				fisAccount.GovernmentId = (!string.IsNullOrWhiteSpace(responseData.E10705)) ? responseData.E10705 : null;
				fisAccount.IDIssuingCountry = GetIssusingCountry(fisAccount.GovernmentIDType);
				fisAccount.IDIssuingState = (!string.IsNullOrWhiteSpace(responseData.E10707)) ? MapToState(responseData.E10707, fisAccount.GovernmentIDType) : null;
				fisAccount.IDIssueDate = (!string.IsNullOrWhiteSpace(responseData.E10708)) ? Convert.ToDateTime(responseData.E10708) : fisAccount.IDIssueDate;
				fisAccount.IDExpirationDate = (!string.IsNullOrWhiteSpace(responseData.E10709)) ? Convert.ToDateTime(responseData.E10709) : fisAccount.IDIssueDate;
			}
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

		private string MapToFISIDType(string nexxoIdType)
		{
			string fisId = string.Empty;

			Dictionary<string, string> idMappying = new Dictionary<string, string>()
			{
				{"DRIVER'S LICENSE"						,"D"		},
				{"EMPLOYMENT AUTHORIZATION CARD (EAD)"	,"EAC"		},
				{"GREEN CARD / PERMANENT RESIDENT CARD"	,"PR"		},
//				{"GREEN CARD / PERMANENT RESIDENT CARD" ,"RA"		},
				{"MILITARY ID"							,"M"		},
				{"PASSPORT"								,"P"		},
				{"U.S. STATE IDENTITY CARD"				,"S"		}, 			
				{"MATRICULA CONSULAR"					,"MC"		}
			};

			if (idMappying.ContainsKey(nexxoIdType))
			{
				fisId = idMappying[nexxoIdType];
			}
			return fisId;
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

		#endregion

	}
}
