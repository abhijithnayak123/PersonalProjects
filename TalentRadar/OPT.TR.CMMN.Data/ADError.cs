using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public enum ADError
    {
        AD_Success = 0x00,
        AD_InvalidParams = 0x01,
        AD_NotOperational = 0x03,
        AD_InvalidCredentials = 0x04,
        AD_InvalidDomainPath = 0x05,
		AD_InvalidMemberInGroup = 0x06
    }
}
