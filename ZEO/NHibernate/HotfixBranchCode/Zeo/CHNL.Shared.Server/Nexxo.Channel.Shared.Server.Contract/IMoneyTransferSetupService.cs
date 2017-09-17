using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Contract
{
	public interface IMoneyTransferSetupService
	{
		List<XferMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext);

		List<XferMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext);

		List<XferMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext);

		List<XferMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext);
	}
}
