namespace MGI.Cxn.Fund.FirstView.Data
{
	public class CardRequest : Request
	{
		public long? deTCIVRPrimaryAccountNumber { get; set; }
		public string deTCIVRCARD_PIN { get; set; }
        public string deBSAccountNumber { get; set; }
	}
}
