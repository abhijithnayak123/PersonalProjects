using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Common.DataAccess.Contract;
using MGI.TimeStamp;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.Partner.Impl
{
	public class LocationCounterIdServiceImpl : ILocationCounterIdService
	{
		public IRepository<LocationCounterId> LocationCounterIdRepo { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }
		public string Get(Guid locationID, int providerId)
		{
			string counterId = string.Empty;

			try
			{
				var locationCounterId = LocationCounterIdRepo.FilterBy(x => x.LocationId == locationID && providerId == x.ProviderId)
										.Where(x => x.IsAvailable == true)
										.OrderBy(x => x.DTServerLastModified)
										.FirstOrDefault();

				if (locationCounterId != null)
					counterId = locationCounterId.CounterId;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.LOCATION_COUNTERID_NOT_FOUND, ex);
			}

			return counterId;
		}

		public LocationCounterId Get(Guid locationID, string counterId, int providerId)
		{
			try
			{
				return LocationCounterIdRepo.FilterBy(x => x.LocationId == locationID && counterId == x.CounterId && providerId == x.ProviderId).FirstOrDefault();			
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Location Id:" + Convert.ToString(locationID));
				details.Add("Counter Id:" + counterId);
				details.Add("Provider Id:" + Convert.ToString(providerId));

				MongoDBLogger.ListError<string>(details, "Get", AlloyLayerName.CORE, ModuleName.Customer,
					"Error in Get -MGI.Core.Partner.Impl.LocationCounterIdServiceImpl", ex.Message, ex.StackTrace);

				throw new ChannelPartnerException(ChannelPartnerException.LOCATION_COUNTERID_NOT_FOUND, ex);
			}
		}


		public LocationCounterId Get(Guid locationID, string counterId)
		{
			try
			{
				return LocationCounterIdRepo.FilterBy(x => x.LocationId == locationID && counterId == x.CounterId && x.IsAvailable == false).FirstOrDefault();
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Location Id:" + Convert.ToString(locationID));
				details.Add("Counter Id:" + counterId);

				MongoDBLogger.ListError<string>(details, "Get", AlloyLayerName.CORE, ModuleName.Customer,
					"Error in Get -MGI.Core.Partner.Impl.LocationCounterIdServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.LOCATION_COUNTERID_NOT_FOUND, ex);
			}
		}

		public bool Update(LocationCounterId locationCounterId)
		{
			try
			{
				locationCounterId.DTServerLastModified = DateTime.Now;
				LocationCounterIdRepo.Update(locationCounterId);
				return true;
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<LocationCounterId>(locationCounterId, "Update", AlloyLayerName.CORE, ModuleName.Customer,
					"Error in Update -MGI.Core.Partner.Impl.LocationCounterIdServiceImpl", ex.Message, ex.StackTrace);

				throw new ChannelPartnerException(ChannelPartnerException.LOCATION_COUNTERID_STATUS_UPDATE_FAILED, ex);
			}
		}
	}
}
