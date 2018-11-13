namespace MGI.Common.DataAccess.Contract
{
	public interface IAlertRepository
	{
		object Add( string subject, string message, string recipientAddress ); 
	}
}
