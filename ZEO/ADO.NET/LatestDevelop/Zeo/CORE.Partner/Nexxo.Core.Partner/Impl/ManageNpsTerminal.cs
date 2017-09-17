using System;
using System.Linq;
using System.Collections.Generic;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.TimeStamp;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class ManageNpsTerminal : INpsTerminal
	{
		public IRepository<NpsTerminal> NpsTerminalRepository { private get; set; }

		public bool Create(NpsTerminal npsTerminal)
		{
			try
			{
				npsTerminal.rowguid = Guid.NewGuid();
				npsTerminal.DTServerCreate = DateTime.Now;
				npsTerminal.DTServerLastModified = DateTime.Now;
				npsTerminal.DTTerminalCreate = Clock.DateTimeWithTimeZone(npsTerminal.Location.TimezoneID);
				NpsTerminalRepository.AddWithFlush(npsTerminal);

				return true;
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_CREATE_FAILED, ex);
			}
		}

		public bool Update(NpsTerminal npsTerminal)
		{
			try
			{
				npsTerminal.DTServerLastModified = DateTime.Now;
				npsTerminal.DTTerminalCreate = Clock.DateTimeWithTimeZone(npsTerminal.Location.TimezoneID);

				return NpsTerminalRepository.Merge(npsTerminal);
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_UPDATE_FAILED, ex);
			}
		}

		public NpsTerminal Lookup(long Id)
		{
			try
			{
				return NpsTerminalRepository.FindBy(nps => nps.Id == Id);
				//return GetAll().FirstOrDefault(nps => nps.Id == Id);
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

		public NpsTerminal Lookup(string ipAddress)
		{
			try
			{
				return GetAll().FirstOrDefault(nps => nps.IpAddress == ipAddress);
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

		public NpsTerminal Lookup(Guid Id)
		{
			try
			{
				return NpsTerminalRepository.FindBy(nps => nps.rowguid == Id);
				//return GetAll().FirstOrDefault(nps => nps.rowguid == Id);
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

		public NpsTerminal Lookup(string name, ChannelPartner channelPartner)
		{
			try
			{
				return NpsTerminalRepository.FindBy(nps => nps.Name == name && nps.ChannelPartner == channelPartner);
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

		public List<NpsTerminal> GetByLocationID(long locationId)
		{
			try
			{
				return NpsTerminalRepository.All().Where(nps => nps.Location.Id == locationId).ToList();
			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

		public IQueryable<NpsTerminal> GetAll()
		{
			try
			{
				return NpsTerminalRepository.All();

			}
			catch (Exception ex)
			{
				throw new PartnerTerminalException(PartnerTerminalException.NPSTERMINAL_GET_FAILED, ex);
			}
		}

	}
}
