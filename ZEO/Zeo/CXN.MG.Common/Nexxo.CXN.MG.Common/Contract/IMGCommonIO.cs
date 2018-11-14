using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.CXN.MG.Common.Data;
using MGI.CXN.MG.Common.AgentConnectService;
using MGI.Common.Util;

namespace MGI.CXN.MG.Common.Contract
{
	public interface IMGCommonIO
	{
		CommitTransactionResponse CommitTransaction(CommitTransactionRequest commitRequest, MGIContext mgiContext);

		StateRegulatorResponse GetDoddFrankStateRegulatorInfo(StateRegulatorRequest request, MGIContext mgiContext);

		FeeLookupResponse GetFee(FeeLookupRequest feeLookupRequest, MGIContext mgiContext);

		GetFieldsForProductResponse GetFieldsForProduct(GetFieldsForProductRequest request, MGIContext mgiContext);

		CTResponse GetMetaData(Data.BaseRequest request, MGIContext mgiContext);

		PhotoIDType GetPhotoIdType(string idType);

		Data.TranslationsResponse GetTransalations(Data.TranslationsRequest request, MGIContext mgiContext);
	}
}
