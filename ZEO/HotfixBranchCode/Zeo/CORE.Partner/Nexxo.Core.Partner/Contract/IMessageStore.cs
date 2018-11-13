using MGI.Core.Partner.Data;
namespace MGI.Core.Partner.Contract
{
	public interface IMessageStore
	{

		/// <summary>
		/// This method is to add message store details
		/// </summary>
		/// <param name="partnerId">This is channel partner id</param>
		/// <param name="key">This is message key</param>
		/// <param name="language">This is language</param>
		/// <param name="message">This is message</param>
		/// <param name="additionalDetails">This is Additional Details</param>
		long Add(long partnerId, string key, Language language, Message message, string additionalDetails);

		/// <summary>
		/// This method is to get the message by id
		/// </summary>
		/// <param name="Id">This is unique identifier of message</param>
		/// <returns>This is Message</returns>	
		Message Lookup(long Id);

		/// <summary>
		/// This method is to get the message by channel partner id, key and language
		/// </summary>
		/// <param name="partnerId">This is channel partner id</param>
		/// <param name="key">This is message key</param>
		/// <param name="language">This is language</param>
		/// <returns>This is Message</returns>	
		Message Lookup(long partnerId, string key, Language language);
	}
}
