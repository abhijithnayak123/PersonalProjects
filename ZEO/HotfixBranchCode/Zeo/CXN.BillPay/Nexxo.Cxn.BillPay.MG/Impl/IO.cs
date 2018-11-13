using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MGI.CXN.MG.Common.Data;
using MGI.Cxn.BillPay.MG.AgentConnectService;
using MGI.Cxn.BillPay.MG.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.BillPay.MG.Impl
{
    public class IO:IIO
    {
        #region Private Members

        private string _serviceURL = "https://extws.moneygram.com/extws/services/AgentConnect1305";

        #endregion
		public TLoggerCommon MongoDBLogger { private get; set; }

		public BpValidationResponse BpValidation(BpValidationRequest validationRequest, MGIContext mgiContext)
        {
            try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<BpValidationRequest>(mgiContext.CustomerSessionId, validationRequest, "BpValidation", AlloyLayerName.CXN,
					ModuleName.BillPayment, "BpValidation -MGI.Cxn.BillPay.MG.Impl.IO", "REQUEST", mgiContext);
				#endregion
				AgentConnectClient client = GetAgentClient();

                BpValidationResponse validationResponse = client.bpValidation(validationRequest);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<BpValidationResponse>(mgiContext.CustomerSessionId, validationResponse, "BpValidation", AlloyLayerName.CXN,
					ModuleName.BillPayment, "BpValidation -MGI.Cxn.BillPay.MG.Impl.IO", "RESPONSE", mgiContext);
				#endregion
				return validationResponse;
            }
            catch (FaultException ex)
            {
                var errorDetail = ((FaultException<Error>)(ex)).Detail;
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BpValidationRequest>(validationRequest, "BpValidation", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in BpValidation -MGI.Cxn.BillPay.MG.Impl.IO", errorDetail.errorString, ex.StackTrace);			
             
                throw new Data.MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        public BillerSearchResponse BillerSearch(BillerSearchRequest billerSearchRequest)
        {
            try
			{
				#region AL-1014 Transactional Log User Story
				MGIContext mgiContext = new MGIContext();
				MongoDBLogger.Info<BillerSearchRequest>(0, billerSearchRequest, "BillerSearch -REQUEST", AlloyLayerName.CXN,
					ModuleName.BillPayment, "BillerSearch -MGI.Cxn.BillPay.MG.Impl.IO", "REQUEST", mgiContext);
				#endregion

				AgentConnectClient client = GetAgentClient();
                BillerSearchResponse billerSearchResponse = client.billerSearch(billerSearchRequest);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<BillerSearchResponse>(0, billerSearchResponse, "BillerSearch -RESPONSE", AlloyLayerName.CXN,
					ModuleName.BillPayment, "BillerSearch -MGI.Cxn.BillPay.MG.Impl.IO", "RESPONSE", mgiContext);
				#endregion

				return billerSearchResponse;
            }
            catch (FaultException ex)
            {
				var errorDetail = ((FaultException<Error>)(ex)).Detail;
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillerSearchRequest>(billerSearchRequest, "BillerSearch", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in BillerSearch -MGI.Cxn.BillPay.MG.Impl.IO", errorDetail.errorString, ex.StackTrace);			
             
                throw new Data.MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        #region Private Methods

        private AgentConnectClient GetAgentClient()
        {
            var client = new AgentConnectClient();

            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_serviceURL);

            return client;
        }

        #endregion

    }
}
