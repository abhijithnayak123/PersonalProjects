using AutoMapper;

namespace MGI.Core.Catalog.Data
{
    public class Presentment
    {
        public virtual long ProductID { get; set; }
        public virtual int ProcessorID { get; set; }
        public virtual string BillerName { get; set; }
        public virtual System.Nullable<decimal> Fee { get; set; }
        public virtual System.Nullable<decimal> MinimumLoad { get; set; }
        public virtual System.Nullable<decimal> MaximumLoad { get; set; }      

        public Presentment()
        {
            Mapper.CreateMap<Product, Presentment>()
                .ForMember(cv => cv.ProductID, m => m.MapFrom(src => src.Id));                
        }
    }
}
