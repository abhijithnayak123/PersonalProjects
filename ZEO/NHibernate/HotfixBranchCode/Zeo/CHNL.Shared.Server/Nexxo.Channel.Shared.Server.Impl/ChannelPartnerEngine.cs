using ChannelPartnerService = MGI.Biz.Partner.Contract.IChannelPartnerService;

namespace MGI.Channel.Shared.Server.Impl
{
	public partial class SharedEngine
    {
        #region Injected Services

        public ChannelPartnerService ChannelPartnerService { get; set; }

        #endregion

        #region ChannelPartner Data Mapper

        internal static void ChannelPartnerConverter()
		{

        }

        #endregion

    } 
}
