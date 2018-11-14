using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using AutoMapper;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class ManageLocationsImpl : MGI.Biz.Partner.Contract.IManageLocations
	{
		private MGI.Core.Partner.Contract.IManageLocations _locationService;
		public MGI.Core.Partner.Contract.IManageLocations LocationService
		{
			get { return _locationService; }
			set { _locationService = value; }
		}

		public ManageLocationsImpl()
		{
			Mapper.CreateMap<Biz.Partner.Data.Location, MGI.Core.Partner.Data.Location>()
				.ForMember(x => x.LocationProcessorCredentials, o => o.MapFrom(s => s.ProcessorCredentials));
			Mapper.CreateMap<MGI.Core.Partner.Data.Location, Biz.Partner.Data.Location>()
				.ForMember(x => x.ProcessorCredentials, o => o.MapFrom(s => s.LocationProcessorCredentials));

			Mapper.CreateMap<MGI.Core.Partner.Data.Location, MGI.Core.Partner.Data.Location>()
				.ForMember(c => c.LocationProcessorCredentials, x => x.Ignore());
		}

		public MGI.Biz.Partner.Data.Location GetByName(long agentSessionId, string locationName, MGIContext mgiContext)
		{
			try
			{
				MGI.Core.Partner.Data.Location sourceManageLocations = LocationService.GetByName(locationName);
				Biz.Partner.Data.Location locations = Mapper.Map<Biz.Partner.Data.Location>(sourceManageLocations);
				return locations;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_GET_FAILED, ex);
			}
		}

		public List<MGI.Biz.Partner.Data.Location> GetAll()
		{
			try
			{
				List<MGI.Core.Partner.Data.Location> sourceManageLocationList = LocationService.GetAll();
				List<Biz.Partner.Data.Location> targetLocationLists = Mapper.Map<List<Biz.Partner.Data.Location>>(sourceManageLocationList);
				return targetLocationLists;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_GET_FAILED, ex);
			}
		}

		public long Create(long agentSessionId, MGI.Biz.Partner.Data.Location sourceManageLocation, MGIContext mgiContext)
		{
			try
			{
				MGI.Core.Partner.Data.Location targetManageLocations = new MGI.Core.Partner.Data.Location();
				targetManageLocations = Mapper.Map<Biz.Partner.Data.Location, MGI.Core.Partner.Data.Location>(sourceManageLocation);
				return LocationService.Create(targetManageLocations);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_CREATE_FAILED, ex);
			}
		}

		public bool Update(long agentSessionId, MGI.Biz.Partner.Data.Location sourceManageLocation, MGIContext mgiContext)
		{
			try
			{
				MGI.Core.Partner.Data.Location targetManageLocations = new MGI.Core.Partner.Data.Location();
				targetManageLocations = Mapper.Map<MGI.Core.Partner.Data.Location>(sourceManageLocation);

				MGI.Core.Partner.Data.Location location = LocationService.GetAll().Find(c => c.Id == sourceManageLocation.Id);
				targetManageLocations.LocationProcessorCredentials = location.LocationProcessorCredentials;
				return LocationService.Update(targetManageLocations);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_UPDATE_FAILED, ex);
			}
		}

		public MGI.Biz.Partner.Data.Location Lookup(long locationId, MGIContext mgiContext)
		{
			try
			{
				MGI.Core.Partner.Data.Location sourceManageLocations = LocationService.Lookup(locationId);
				Biz.Partner.Data.Location locations = Mapper.Map<Biz.Partner.Data.Location>(sourceManageLocations);
				return locations;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_GET_FAILED, ex);
			}
		}

		public List<Data.Location> GetAll(long agentSessionId, MGIContext mgiContext)
		{
			try
			{
				return GetAll().FindAll(loc => loc.ChannelPartnerId == mgiContext.ChannelPartnerId);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizLocationException(BizLocationException.LOCATION_GET_FAILED, ex);
			}
		}
	}
}
