namespace MGI.Channel.DMS.Web.Models
{
	public class CardBalance
	{
		public decimal Balance { get; set; }
		public string CardStatus { get; set; }
		public string CardStatusDisplay
		{
			get
			{
				return MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(CardStatus.ToString());
			}
		}

		public int CardStatusId { get; set; }
		public string CardType { get; set; }
	}
}
