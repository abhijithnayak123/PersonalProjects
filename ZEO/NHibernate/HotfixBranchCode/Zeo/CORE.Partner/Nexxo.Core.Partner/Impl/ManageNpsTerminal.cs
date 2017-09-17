using System;
using System.Linq;
using System.Collections.Generic;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.TimeStamp;

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
                throw new ChannelPartnerException(ChannelPartnerException.NPSTERMINAL_CREATE_FAILED, ex);
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
                throw new ChannelPartnerException(ChannelPartnerException.NPSTERMINAL_UPDATE_FAILED, ex);
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
                throw new ChannelPartnerException(ChannelPartnerException.NPSTERMINAL_NOT_FOUND, ex);
            }
		}

		public NpsTerminal Lookup(string ipAddress)
		{

			return GetAll().FirstOrDefault(nps => nps.IpAddress == ipAddress);
		}

		public NpsTerminal Lookup(Guid Id)
		{
            try
            {
                return NpsTerminalRepository.FindBy(nps => nps.rowguid  == Id);
                //return GetAll().FirstOrDefault(nps => nps.rowguid == Id);
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.NPSTERMINAL_NOT_FOUND, ex);
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
				throw new ChannelPartnerException(ChannelPartnerException.NPSTERMINAL_NOT_FOUND, ex);
			}
		}

		public List<NpsTerminal> GetByLocationID(long locationId)
		{
			return NpsTerminalRepository.All().Where(nps => nps.Location.Id == locationId).ToList();
		}

		public IQueryable<NpsTerminal> GetAll()
		{
			return NpsTerminalRepository.All();
		}

	}
}
