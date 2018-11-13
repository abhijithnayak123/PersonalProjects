using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
    public class Channel
    {
        public ChannelType Type { get { return ChannelType.H2H; } }
        public string Name { get; set; }
        public string Version { get; set; }
        public bool TypeSpecified { get { return true; } }
    }

    public class ForeignRemoteSystem
    {
        public string Identifier { get; set; }
        public string CoutnerId { get; set; }
        public string ReferenceNo { get; set; }
    }

	public class SwbFlaInfo
	{
		public string SwbOperatorId { get; set; }
		public SwbFlaInfoReadPrivacyNoticeFlag ReadPrivacyNoticeFlag { get; set; }
		public SwbFlaInfoFlaCertificationFlag FlagCertificationFlag { get; set; }
		public bool ReadPrivacyNoticeFlagSpecified { get; set; }
		public bool FlagCertificationFlagSpecified { get; set; }
		public GeneralName name { get; set; }
	}

	public enum SwbFlaInfoReadPrivacyNoticeFlag
	{
		Y,
		N,
	}

	public enum SwbFlaInfoFlaCertificationFlag
	{
		Y,
		N,
	}

	public class GeneralName
	{
		public NameType Type;
		public bool NameTypeSpecified;
		public string FirstName;
		public string LastName;
	}

	public enum NameType
	{
		C,
		D,
		M,
	}
    public enum ChannelType
    {
        CSC,
        AGT,
        H2H,
        MTBP
    }
}
