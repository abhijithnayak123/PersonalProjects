using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
//using Moq;
//using MGI.Core.Partner.Test.FISService;
using System.Net; 

using Spring.Testing.NUnit;
using System.IO;
using System.Xml; 

namespace MGI.Core.Partner.Test
{   
    [TestFixture]
    public class SynchInServiceTest
    {
		//FISService.wsdlNexxoPolicyClient client = null;
		//[TestFixtureSetUp]
		//public void Setup()
		//{
		//	string endpointUrl = "https://xmlgw-qa.soa.synovus.com:443/wsdlNexxoPolicy";
		//	string endpointName = "wsdlNexxoPolicy";

		//	client = new wsdlNexxoPolicyClient(endpointName, endpointUrl);
		//	client.ClientCredentials.UserName.UserName = "nexxouser";
		//	client.ClientCredentials.UserName.Password = "$yn1nex";
		//}
        
        [Test]
        public void ValidateSSN()
        {
            // request object 
            //CICustTaxNbrSrchMtvnSvcReq request = new CICustTaxNbrSrchMtvnSvcReq();
            //request.MsgUUID = 
            //request.MtvnSvcVer = CICustTaxNbrSrchMtvnSvcReqMtvnSvcVer.Item10;
            //request.PrcsParms = new CICustTaxNbrSrchMtvnSvcReqPrcsParms();
            //CICustTaxNbrSrchMtvnSvcReqPrcsParms prcsparams = new  CICustTaxNbrSrchMtvnSvcReqPrcsParms();
            //prcsparams.SrcID

			// here starting code 
			#region
			//CICustTaxNbrSrchMtvnSvcReq request = new CICustTaxNbrSrchMtvnSvcReq();
			//request.MtvnSvcVer = CICustTaxNbrSrchMtvnSvcReqMtvnSvcVer.Item10;
           
			//CICustTaxNbrSrchMtvnSvcReqPrcsParms reqparams = new CICustTaxNbrSrchMtvnSvcReqPrcsParms();
			//reqparams.SrcID = "";
			//reqparams.TestInd = "";
			//request.PrcsParms = reqparams;
            
			//CICustTaxNbrSrchMtvnSvcReq req = new CICustTaxNbrSrchMtvnSvcReq();
			//CICustTaxNbrSrchReqData cICustTaxNbrSrchReqDataField = new CICustTaxNbrSrchReqData();
			//cICustTaxNbrSrchReqDataField.E10202 = "666014830";

			//CICustTaxNbrSrchMtvnSvcReqSvcMsgData msgdata = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData();
			//msgdata.CICustTaxNbrSrchReqData = cICustTaxNbrSrchReqDataField;
            
			//CICustTaxNbrSrchMtvnSvcReqSvc[] svc = new CICustTaxNbrSrchMtvnSvcReqSvc[1];
			//CICustTaxNbrSrchMtvnSvcReqSvc svc1 = new CICustTaxNbrSrchMtvnSvcReqSvc();
			//svc1.MsgData = msgdata;
			//svc[0] = svc1;
			//request.Svc = svc;

			//MGI.Core.Partner.Test.FISService.CICustTaxNbrSrchMtvnSvcRes res = new CICustTaxNbrSrchMtvnSvcRes();
			//res = client.CICustTaxNbrSrch(request);

			#endregion

			//Ending of here

            //CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSrc src = new CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSrc(); 
            //src.
            //req.Svc[0].SvcParms = 
            //request.Svc = req; 
            //CICustTaxNbrSrchMtvnSvcReqPrcsParms prms = new CICustTaxNbrSrchMtvnSvcReqPrcsParms(); 
            //prms.SrcID = ""; 
            //prms.TestInd ="";
            //req.Svc[0].SvcParms = prms; 
            //MGI.Core.Partner.Test.FISService.CICustTaxNbrSrchResponse1 CICustTaxNbrSrch = new MGI.Core.Partner.Test.FISService.CICustTaxNbrSrchResponse1();
            //CICustTaxNbrSrchReqData redata = new CICustTaxNbrSrchReqData();
            //redata.E10202 = "987001306"; 
            //CICustTaxNbrSrchMtvnSvcReqSvcMsgData msgdata = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData();
            //msgdata.CICustTaxNbrSrchReqData = redata;

            //CICustTaxNbrSrchMtvnSvcReqSvc reqsvc = new CICustTaxNbrSrchMtvnSvcReqSvc();
            //reqsvc.MsgData = msgdata;

            //CICustTaxNbrSrchMtvnSvcReq req = new CICustTaxNbrSrchMtvnSvcReq();
            //req.Svc[0] = reqsvc;
            
        }

        [Test]
        public void GetAppInfo()
        {
			//requestType request = new requestType();
			//request.ApplKy = "A7DA277E-3C52-431F-B53A-D6DB1B4F0681";
			//request.ChannelKy = "5";
			//request.MetBankNumber = "300";
			//request.MsgID = Guid.NewGuid().ToString();
            
			//responseType response = client.synovussoaapplicationkeygetAppKeyxbd(request);
           
			//Assert.IsNotNullOrEmpty(response.MsgData.MetPassword);
			//Assert.IsNotNullOrEmpty(response.MsgData.MetUsername);
			//Assert.IsNotNullOrEmpty(response.MsgData.MetVendorID);
        }


        [Test] //Not Used
        public void GetAppInfoFromURL()
        {
            //string _url = "https://xmlgw-qa.soa.synovus.com:443/wsdlNexxoPolicy";
            //string _userName = "nexxouser";
            //string _password = "$yn1nex";
            //var request = WebRequest.Create(_url);
            //request.PreAuthenticate = true;
            //request.Credentials = new NetworkCredential(_userName, _password);
            

            //requestType request1 = new requestType();
            //request1.ApplKy = "A7DA277E-3C52-431F-B53A-D6DB1B4F0681";
            //request1.ChannelKy = "5";
            //request1.MetBankNumber = "300";
            //request1.MsgID = Guid.NewGuid().ToString();

            ////WebClient webClient = new WebClient();
            ////webClient.Credentials = new NetworkCredential(_userName, _password);
            ////Stream stream = webClient.OpenRead(_url);

            //System.ServiceModel.Channels.Binding httpBinding = CreateBasicHttpBinding();
            //EndpointAddress address = CreateEndPoint();
            //var serviceClient = new MyServiceClient(httpBinding, address);


        }




    }
}
