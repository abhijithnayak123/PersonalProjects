using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
    public class TerminalServiceImpl : ITerminalService
    {
        public IRepository<Terminal> TerminalRepo { private get; set; }

        public Terminal Lookup(long Id)
        {
            try
            {
                return TerminalRepo.FindBy(t => t.Id == Id);
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_NOT_FOUND, ex);
            }
        }

        public Terminal Lookup(Guid PK)
        {
            try
            {
                return TerminalRepo.FindBy(t => t.rowguid == PK);
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_NOT_FOUND, ex);
            }
        }

        public Guid Create(Terminal terminal)
        {
            try
            {
                terminal.rowguid = Guid.NewGuid();
                terminal.DTServerCreate = DateTime.Now;
				terminal.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(terminal.Location.TimezoneID);
                object id = TerminalRepo.AddWithFlush(terminal);
                return (Guid)id;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_CREATE_FAILED, ex);
            }
        }

        public bool Update(Terminal terminal)
        {
            try
            {
                terminal.DTServerLastModified = DateTime.Now;
				terminal.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(terminal.Location.TimezoneID);
                return TerminalRepo.Merge(terminal);
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_UPDATE_FAILED, ex);
            }
        }

		public Terminal Lookup(string terminalName, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			try
			{
				return TerminalRepo.FindBy(t => t.Name == terminalName && t.ChannelPartner == channelPartner);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_NOT_FOUND, ex);
			}
		}
	}
}
