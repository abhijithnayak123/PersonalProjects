using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using MGI.Cxn.Customer.FIS.Test.FISService;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Cxn.Customer.FIS.Impl; 
using System.Net;
using Spring.Testing.NUnit;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using  MGI.Cxn.Customer.Data;


namespace MGI.Cxn.Customer.FIS.Test
{   
    [TestFixture]
    public class SynchInServiceTest : AbstractTransactionalSpringContextTests

    {
        public FISIOImpl FISIO { private get; set; }



        FISService.wsdlNexxoPolicyClient client = new wsdlNexxoPolicyClient();

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Cxn.Customer.FIS.Test/MGI.Cxn.Customer.FIS.Test/Cxn.Customer.FIS.Test.Spring.xml" }; }
        }
        
        //[SetUp]
        //public void Setup()
        //{
        //    //string endpointUrl = "https://xmlgw-qa.soa.synovus.com:443/wsdlNexxoPolicy";
        //    //string endpointName = "wsdlNexxoPolicy";

        //    //client = new wsdlNexxoPolicyClient(endpointName, endpointUrl);
        //    client.ClientCredentials.UserName.UserName = "nexxouser";
        //    client.ClientCredentials.UserName.Password = "$yn1nex";


        //}
        
        [Test]
        public void ValidateSSN()
        {
            CustTaxNbrSrchReq request = new CustTaxNbrSrchReq();
            request.SearchSSN = "666438018";
            //request.SvcParms.RqstUUID = Guid.NewGuid();

            MsgData _msgdata = new MsgData();
            CICustTaxNbrSrchReqData req = new CICustTaxNbrSrchReqData();

            CICustTaxNbrSrchMtvnSvcReq req1 = new CICustTaxNbrSrchMtvnSvcReq();
            CICustTaxNbrSrchMtvnSvcReqSvc svc = new CICustTaxNbrSrchMtvnSvcReqSvc();
            CICustTaxNbrSrchMtvnSvcReqSvcMsgData msgdata = new CICustTaxNbrSrchMtvnSvcReqSvcMsgData();
            req.E10202 = "666438018";
            msgdata.CICustTaxNbrSrchReqData = req;

            svc.MsgData = msgdata;
            svc.SvcParms = new CICustTaxNbrSrchMtvnSvcReqSvcSvcParms() { ApplID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), RoutingID = "300", SvcID = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch, SvcNme = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcID.CICustTaxNbrSrch.ToString(), SvcVer = CICustTaxNbrSrchMtvnSvcReqSvcSvcParmsSvcVer.Item60 };
            

            CICustTaxNbrSrchMtvnSvcReqSvc[] reqs = new CICustTaxNbrSrchMtvnSvcReqSvc[1];
            reqs[0] = svc;

            req1.Svc = reqs;
            req1.MsgUUID = Guid.NewGuid().ToString();


            requestType apprequest = new requestType();
            apprequest.ApplKy = "A7DA277E-3C52-431F-B53A-D6DB1B4F0681";
            apprequest.ChannelKy = "5";
            apprequest.MetBankNumber = "300";
            apprequest.MsgID = Guid.NewGuid().ToString();


            responseType appresponse = client.synovussoaapplicationkeygetAppKeyxbd(apprequest);

            req1.PrcsParms = new CICustTaxNbrSrchMtvnSvcReqPrcsParms() { SrcID = "Test" };

            CICustTaxNbrSrchMtvnSvcReqSvcSecurity sec = new CICustTaxNbrSrchMtvnSvcReqSvcSecurity() { Item = new CICustTaxNbrSrchMtvnSvcReqSvcSecurityBasicAuth() { UsrID = appresponse.MsgData.MetUsername, Pwd = appresponse.MsgData.MetPassword } };


            req1.Svc[0].Security = sec;

            //req1.Svc = new CICustTaxNbrSrchMtvnSvcReqSvc[]();

            CICustTaxNbrSrchMtvnSvcRes response = client.CICustTaxNbrSrch(req1);

            
        }

        [Test]
        public void GetAppInfo()
        {
            requestType request = new requestType();
            request.ApplKy = "A7DA277E-3C52-431F-B53A-D6DB1B4F0681";
            request.ChannelKy = "5";
            request.MetBankNumber = "300";
            request.MsgID = Guid.NewGuid().ToString();
            
            responseType response = client.synovussoaapplicationkeygetAppKeyxbd(request);
           
            Assert.IsNotNullOrEmpty(response.MsgData.MetPassword);
            Assert.IsNotNullOrEmpty(response.MsgData.MetUsername);
            Assert.IsNotNullOrEmpty(response.MsgData.MetVendorID);
        }

        [Test]
        public void CreateFISCustomer()
        {
          //  FISIOImpl fisIo = new FISIOImpl();
            string customerId;
            Cxn.Customer.FIS.Data.FISAccount custProfile = new Cxn.Customer.FIS.Data.FISAccount();
            custProfile.FirstName = "FISNexxoTest"; 
            custProfile.MiddleName = "MiddleName"; 
            //need to know what to be mapped for other fields. 
            //custProfile.

            System.Collections.Generic.Dictionary<string, object> context = new System.Collections.Generic.Dictionary<string, object>(); 
            context.Add("ChannelParnerId",33); 
            context.Add("BankId",300); 

            customerId = FISIO.CreateFISCustomer(custProfile,context);  

            /* will need to clean up this */

            requestType apprequest = new requestType();
            apprequest.ApplKy = "A7DA277E-3C52-431F-B53A-D6DB1B4F0681";
            apprequest.ChannelKy = "5";
            apprequest.MetBankNumber = "300";
            apprequest.MsgID = Guid.NewGuid().ToString();

            responseType appresponse = client.synovussoaapplicationkeygetAppKeyxbd(apprequest);

            //Creating the request object
            CIOpenIndvCustMtvnSvcReq request = new FISService.CIOpenIndvCustMtvnSvcReq();

            //Creating the respons object
            FISService.CIOpenIndvCustMtvnSvcRes reseponse = new FISService.CIOpenIndvCustMtvnSvcRes();


            request.MsgUUID = Guid.NewGuid().ToString();
            request.MtvnSvcVer = CIOpenIndvCustMtvnSvcReqMtvnSvcVer.Item10;
            request.PrcsParms = new CIOpenIndvCustMtvnSvcReqPrcsParms() { SrcID = "Test" };

            CIOpenIndvCustMtvnSvcReqSvc baserequest = new CIOpenIndvCustMtvnSvcReqSvc();
            baserequest.Security = new CIOpenIndvCustMtvnSvcReqSvcSecurity() { Item = new CIOpenIndvCustMtvnSvcReqSvcSecurityBasicAuth() { UsrID = appresponse.MsgData.MetUsername, Pwd = appresponse.MsgData.MetPassword } };
            baserequest.SvcParms = new CIOpenIndvCustMtvnSvcReqSvcSvcParms() { ApplID = CIOpenIndvCustMtvnSvcReqSvcSvcParmsApplID.CI, RqstUUID = Guid.NewGuid().ToString(), SvcID = CIOpenIndvCustMtvnSvcReqSvcSvcParmsSvcID.CIOpenIndvCust, RoutingID = "300", SvcNme = CIOpenIndvCustMtvnSvcReqSvcSvcParmsSvcID.CIOpenIndvCust.ToString(), SvcVer = CIOpenIndvCustMtvnSvcReqSvcSvcParmsSvcVer.Item40 };


            FISService.CIOpenIndvCustReqData reqData = new FISService.CIOpenIndvCustReqData();
            reqData.E10070 = "Test";
            reqData.E10071 = "Test";
            //reqData.E10072 = 
            //reqData.E10121 =
            //reqData.E10076 = 
            //reqData.E10077 = 
            //reqData.E10078 = 
            //reqData.E10121 =
            CIOpenIndvCustMtvnSvcReqSvcMsgData msgdata = new CIOpenIndvCustMtvnSvcReqSvcMsgData();
            msgdata.CIOpenIndvCustReqData = reqData;
            baserequest.MsgData = msgdata;

            CIOpenIndvCustMtvnSvcReqSvc[] svcrequestArray = new CIOpenIndvCustMtvnSvcReqSvc[1];
            svcrequestArray[0] = baserequest;
            request.Svc = svcrequestArray;

            //Calling the Service method 
            reseponse = client.CIOpenIndvCust(request);

            CustOpenIndvCustRes indvRes = new CustOpenIndvCustRes();

            CIOpenIndvCustMtvnSvcResSvcMsgData resmsgdata = new CIOpenIndvCustMtvnSvcResSvcMsgData();
            resmsgdata = reseponse.Svc[0].MsgData;
            CIOpenIndvCustResData indvCustResData = new CIOpenIndvCustResData();
            indvCustResData = (CIOpenIndvCustResData)resmsgdata.Item;

            indvRes.CustCustomerNumber = indvCustResData.E10033.ToString();

        }
   }
}
