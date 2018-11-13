namespace MGI.CXN.MG.Common.Data
{
	public class Country
	{
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string CountryLegacyCode { get; set; }
		public bool SendActive { get; set; }
		public bool ReceiveActive { get; set; }
		public bool DirectedSendCountry { get; set; }
		public bool MgDirectedSendCountry { get; set; }
	}
}
