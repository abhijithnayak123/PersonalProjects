using Spring.Transaction.Interceptor;
using MGI.Channel.DMS.NpsInstallerServer.Data;
using MGI.Channel.DMS.NpsInstallerServer.Contract;
using System.Collections.Generic;
using MGI.Common.DataAccess.Contract;
using System.Linq;
using System;

namespace MGI.Channel.DMS.NpsInstallerServer.Impl
{
	public class NpsInstallerEngine : INpsInstallerService
	{
        public IRepository<NpsTerminal> NpsTerminalRepo { private get; set; }
        public IRepository<Location> NpsLocationRepo { private get; set; }
        public IRepository<ChannelPartner> NpsChannelPartnerRepo { private get; set; }

        public bool CreateTerminal(NpsTerminal npsTerminal)
        {
            var _npsTermial = NpsTerminalRepo.FindBy(c => c.Name == npsTerminal.Name && c.ChannelPartner.Id == npsTerminal.ChannelPartnerId);

            if (_npsTermial != null)
            {
                _npsTermial.PeripheralServiceUrl = npsTerminal.PeripheralServiceUrl;
                _npsTermial.IpAddress = npsTerminal.IpAddress;
                _npsTermial.Port = npsTerminal.Port;
                _npsTermial.ChannelPartnerId = npsTerminal.ChannelPartnerId;
                _npsTermial.Status = npsTerminal.Status;
                _npsTermial.DTLastMod = DateTime.Now;
                NpsTerminalRepo.UpdateWithFlush(_npsTermial);
            }
            else
            {
                ChannelPartner channelPartner = NpsChannelPartnerRepo.FindBy(c => c.Id == npsTerminal.ChannelPartnerId);
                npsTerminal.rowguid = Guid.NewGuid();
                npsTerminal.ChannelPartner = channelPartner;
                npsTerminal.DTCreate = DateTime.Now;
                NpsTerminalRepo.AddWithFlush(npsTerminal);
            }

            return true;
        }


        public bool UpdateTerminal(NpsTerminal npsTerminal)
        {
            var _npsTermial = NpsTerminalRepo.FindBy(c => c.Name == npsTerminal.Name && c.ChannelPartner.Id == npsTerminal.ChannelPartnerId);

            if (_npsTermial != null)
            {
                _npsTermial.PeripheralServiceUrl = npsTerminal.PeripheralServiceUrl;
                _npsTermial.IpAddress = npsTerminal.IpAddress;
                _npsTermial.Port = npsTerminal.Port;
                _npsTermial.ChannelPartnerId = npsTerminal.ChannelPartnerId;
                _npsTermial.Status = npsTerminal.Status;
                _npsTermial.DTLastMod = DateTime.Now;
                NpsTerminalRepo.UpdateWithFlush(_npsTermial);
                return true;
            }
            return false;
        }


        public NpsTerminal LookupTerminal(string ipAddress)
        {
            NpsTerminal npsTerminal = NpsTerminalRepo.FindBy(c => c.IpAddress == ipAddress);
            return npsTerminal;
        }

        // this will get all location irrespective of channelpartner
        public List<Location> GetAllLocations()
        {
            var locations = NpsLocationRepo.All().ToList();
            return locations;
        }

        // we are not using agentName and password to verify
        public List<string> Locations(string agentName, string agentPassword, string channelPartnerName)
        {
            int channelPartnerId = (int)NpsChannelPartnerRepo.FindBy(c => c.Name == channelPartnerName).Id;
            var locations = NpsLocationRepo.FilterBy(c => c.ChannelPartnerId == channelPartnerId);

            var locs = locations.Select(c => c.LocationName).ToList();
            return locs;
        }
    }
}
