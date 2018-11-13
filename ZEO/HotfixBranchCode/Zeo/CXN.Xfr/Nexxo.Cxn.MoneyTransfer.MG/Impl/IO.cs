using System.Threading;
using MGI.Cxn.MoneyTransfer.MG.AgentConnectService;
using MGI.Cxn.MoneyTransfer.MG.Data;
using System.Collections.Generic;
using System.ServiceModel;
using Spring.Threading;
using MGI.Cxn.MoneyTransfer.MG.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.MoneyTransfer.MG.Impl
{
    public class IO:IIO
    {
		public TLoggerCommon MongoDBLogger { private get; set; }

        #region Private Members

        private const string _serviceURL = "https://extws.moneygram.com/extws/services/AgentConnect1305";

        #endregion

        #region Send Money methods

        public SendValidationResponse SendValidation(SendValidationRequest validationRequest, MGIContext mgiContext)
        {
            try
            {
                AgentConnectClient client = GetAgentClient();

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendValidationRequest>(mgiContext.CustomerSessionId, validationRequest, "SendValidation", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendValidation REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", mgiContext);
				#endregion
				SendValidationResponse validationResponse = client.sendValidation(validationRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendValidationResponse>(mgiContext.CustomerSessionId, validationResponse, "SendValidation", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendValidation RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return validationResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<SendValidationRequest>(validationRequest, "SendValidation", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendValidation - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);				
				var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        public CommitTransactionResponse SendCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext)
        {
            try
            {
                AgentConnectClient client = GetAgentClient();

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<CommitTransactionRequest>(mgiContext.CustomerSessionId, commitRequest, "SendCommit", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendCommit REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", mgiContext);
				#endregion
				CommitTransactionResponse commitResponse = client.commitTransaction(commitRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<CommitTransactionResponse>(mgiContext.CustomerSessionId, commitResponse, "SendCommit", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendCommit RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", mgiContext);
				#endregion
				return commitResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<CommitTransactionRequest>(commitRequest, "SendCommit", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendCommit - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        public DetailLookupResponse DetailLookupRequest(DetailLookupRequest detailLookupRequest)
        {
            AgentConnectClient client = GetAgentClient();
            DetailLookupResponse lookUpResponse = new DetailLookupResponse();

            try
            {

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<DetailLookupRequest>(0, detailLookupRequest, "DetailLookupRequest", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "DetailLookupRequest REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
				lookUpResponse = client.detailLookup(detailLookupRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<DetailLookupResponse>(0, lookUpResponse, "DetailLookupRequest", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "DetailLookupRequest RESPONSE - MGI.Cxn.MoneyTransfer.WU.Impl.IO", new MGIContext());
				#endregion
			}
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<DetailLookupRequest>(detailLookupRequest, "DetailLookupRequest", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in DetailLookupRequest - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
            return lookUpResponse;
        }

        public AmendTransactionResponse AmendTransaction(AmendTransactionRequest amendTransactionRequest)
        {
            AgentConnectClient client = GetAgentClient();
            AmendTransactionResponse amendTransactionResponse = new AmendTransactionResponse();
            try
            {
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<AmendTransactionRequest>(0, amendTransactionRequest, "AmendTransaction", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "AmendTransaction REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
				amendTransactionResponse = client.amendTransaction(amendTransactionRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<AmendTransactionResponse>(0, amendTransactionResponse, "AmendTransaction", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "AmendTransaction RESPONSE - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
			}
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<AmendTransactionRequest>(amendTransactionRequest, "AmendTransaction", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in AmendTransaction - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode,
                                                 errorDetail.detailString == null
                                                     ? errorDetail.errorString
                                                     : errorDetail.errorString + ". " + errorDetail.detailString + ".");
            }
            return amendTransactionResponse;
        }

        public SendReversalResponse SendReversal(SendReversalRequest sendReversalRequest, MGIContext mgiContext)
        {
            try
            {
                AgentConnectClient client = GetAgentClient();

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendReversalRequest>(mgiContext.CustomerSessionId, sendReversalRequest, "SendReversal", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendReversal REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", mgiContext);
				#endregion
				SendReversalResponse sendReversalResponse = client.sendReversal(sendReversalRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<SendReversalResponse>(mgiContext.CustomerSessionId, sendReversalResponse, "SendReversal", AlloyLayerName.CXN,
					ModuleName.MoneyTransfer, "SendReversal RESPONSE - MGI.Cxn.MoneyTransfer.MG.Impl.IO", mgiContext);
				#endregion
				return sendReversalResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<SendReversalRequest>(sendReversalRequest, "SendReversal", AlloyLayerName.CXN, ModuleName.MoneyTransfer,
					"Error in SendReversal - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode + "." + errorDetail.subErrorCode, errorDetail.errorString);
            }
        }

        #endregion

        #region receive money Methods

        public ReferenceNumberResponse RequestReferenceNumber(ReferenceNumberRequest referenceNumberRequest)
        {
            AgentConnectClient client = GetAgentClient();

            try
            {
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<ReferenceNumberRequest>(0, referenceNumberRequest, "RequestReferenceNumber", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "RequestReferenceNumber REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
                var referenceNumberResponse = client.referenceNumber(referenceNumberRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<ReferenceNumberResponse>(0, referenceNumberResponse, "RequestReferenceNumber", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "RequestReferenceNumber RESPONSE - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
                return referenceNumberResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ReferenceNumberRequest>(referenceNumberRequest, "RequestReferenceNumber", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in RequestReferenceNumber - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);

                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }


        public ReceiveValidationResponse ReceiveValidation(ReceiveValidationRequest receiveValidationRequest)
        {
            AgentConnectClient client = GetAgentClient();

            try
            {
				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<ReceiveValidationRequest>(0, receiveValidationRequest, "ReceiveValidation", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "ReceiveValidation REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
                var receiveValidationResponse = client.receiveValidation(receiveValidationRequest);

				#region AL-3370 Transactional Log User Story
				MongoDBLogger.Info<ReceiveValidationResponse>(0, receiveValidationResponse, "ReceiveValidation", AlloyLayerName.CXN,
					ModuleName.ReceiveMoney, "ReceiveValidation RESPONSE - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
				#endregion
                return receiveValidationResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<ReceiveValidationRequest>(receiveValidationRequest, "RequestReferenceNumber", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in RequestReferenceNumber - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);
                
				var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        public CommitTransactionResponse ReceiveCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext)
        {
            try
            {
                AgentConnectClient client = GetAgentClient();
                CommitTransactionResponse commitResponse;

                try
                {
					#region AL-3370 Transactional Log User Story
					MongoDBLogger.Info<CommitTransactionRequest>(0, commitRequest, "ReceiveCommit", AlloyLayerName.CXN,
						ModuleName.ReceiveMoney, "ReceiveCommit REQUEST - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
					#endregion
                    commitResponse = client.commitTransaction(commitRequest);

					#region AL-3370 Transactional Log User Story
					MongoDBLogger.Info<CommitTransactionResponse>(0, commitResponse, "ReceiveCommit", AlloyLayerName.CXN,
						ModuleName.ReceiveMoney, "ReceiveCommit RESPONSE - MGI.Cxn.MoneyTransfer.MG.Impl.IO", new MGIContext());
					#endregion
                }
                catch (System.TimeoutException)
                {
                    //If commit is timed out, then call Commit once more
                    commitResponse = client.commitTransaction(commitRequest);
                }

                return commitResponse;
            }
            catch (FaultException ex)
            {
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<CommitTransactionRequest>(commitRequest, "ReceiveCommit", AlloyLayerName.CXN, ModuleName.ReceiveMoney,
					"Error in ReceiveCommit - MGI.Cxn.MoneyTransfer.WU.Impl.IO", ex.Message, ex.StackTrace);


                var errorDetail = ((FaultException<MGI.Cxn.MoneyTransfer.MG.AgentConnectService.Error>)(ex)).Detail;
                throw new MGramProviderException(errorDetail.errorCode, errorDetail.errorString);
            }
        }

        #endregion


        #region Private Methods

        private AgentConnectClient GetAgentClient()
        {
            AgentConnectClient client = new AgentConnectClient();
            client.Endpoint.Address = new EndpointAddress(_serviceURL);
            return client;
        }

        #endregion
    }
}
