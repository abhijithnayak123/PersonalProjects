namespace MGI.Biz.Catalog.Data
{
    public class Presentment
    {
        public long ProductID { get; set; }
        public string BillerName { get; set; }
        public int ProcessorID { get; set; }
        public decimal Fee { get; set; }
        public decimal MinimumLoad { get; set; }
        public decimal MaximumLoad { get; set; }
    }
}
