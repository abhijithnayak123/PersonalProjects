using TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using System.ServiceModel;
using AutoMapper;
using System;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IZeoService
    {
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public string Provider { get; set; }
        public string Details { get; set; }
        public IMapper mapper { set; get; }

        public ZeoServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<TCF.Zeo.Biz.Receipt.Data.Receipt, Receipt>();
                cfg.CreateMap<TCF.Zeo.Biz.Receipt.Data.CashDrawerReceipt, CashDrawerReceipt>();
                cfg.CreateMap<Data.ZeoContext, TCF.Zeo.Common.Data.ZeoContext>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }
    }
}
