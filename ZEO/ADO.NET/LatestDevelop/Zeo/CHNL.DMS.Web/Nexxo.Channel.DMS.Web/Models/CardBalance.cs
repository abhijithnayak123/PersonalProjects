namespace TCF.Channel.Zeo.Web.Models
{
	public class CardBalance
	{
		public decimal Balance { get; set; }
		public string CardStatus { get; set; }
		public string CardStatusDisplay
		{
			get
			{
				if ( !string.IsNullOrEmpty(CardStatus) )
				{
					return TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(System.Convert.ToString(CardStatus));
				}
				return string.Empty;
			}
		}
		public int CardStatusId { get; set; }
		public string CardType { get; set; }
	}
}
