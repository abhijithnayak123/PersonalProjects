using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizData = MGI.Biz.Partner.Data;
using BizContract = MGI.Biz.Partner.Contract;
using CoreData = MGI.Core.Partner.Data;
using CoreContract = MGI.Core.Partner.Contract;
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.Partner.Impl
{
	public class LocationCounterIdServiceImpl : ILocationCounterIdService
	{
		public TLoggerCommon MongoDBLogger { private get; set; }
		public CoreContract.ILocationCounterIdService LocationCounterIdService { private get; set; }
		public CoreContract.ICustomerSessionService CustomerSessionService { private get; set; }

		public bool UpdateCounterId(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, string.Empty, "UpdateCounterId", AlloyLayerName.BIZ,
				ModuleName.Customer, "Begin UpdateCounterId - MGI.Biz.Partner.Impl.LocationCounterIdServiceImpl",
				mgiContext);
            #endregion

            CoreData.CustomerSession customerSession = GetCustomerSession(customerSessionId);

			if (customerSession.CustomerSessionCounter != null && !string.IsNullOrEmpty(customerSession.CustomerSessionCounter.CounterId))
			{
				string counterId = customerSession.CustomerSessionCounter.CounterId;

				UpdateLocationCounterIdStatus(customerSession.AgentSession.Terminal.Location, counterId, mgiContext.IsAvailable);
			}

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, string.Empty, "UpdateCounterId", AlloyLayerName.BIZ,
				ModuleName.Customer, "End UpdateCounterId - MGI.Biz.Partner.Impl.LocationCounterIdServiceImpl",
				mgiContext);
            #endregion
            return true;
		}

		private void UpdateLocationCounterIdStatus(CoreData.Location location, string counterId, bool isAvailable)
		{
			CoreData.LocationCounterId locationCounterId = LocationCounterIdService.Get(location.rowguid, counterId);

			if (locationCounterId != null)
			{
				locationCounterId.IsAvailable = isAvailable;

				LocationCounterIdService.Update(locationCounterId);
			}
		}

		private CoreData.CustomerSession GetCustomerSession(long customerSessionId)
		{

			CoreData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			if (session == null)
			{
				throw new BizPartnerException(BizPartnerException.CUSTOMERSESSION_NOT_FOUND);
			}

			return session;

		}
	}
}
