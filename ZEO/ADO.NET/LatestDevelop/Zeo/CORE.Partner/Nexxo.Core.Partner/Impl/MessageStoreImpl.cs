// -----------------------------------------------------------------------
// <copyright file="MessageStoreImpl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.DataAccess.Contract;
using System;
using System.Linq.Expressions;
using MGI.Common.Util;

using Spring.Transaction.Interceptor;

namespace MGI.Core.Partner.Impl
{


	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class MessageStoreImpl : IMessageStore
	{
		private IRepository<Message> _messageRepo;
		private IRepository<ChannelPartner> _partnerRepo;
		public NLoggerCommon NLogger = new NLoggerCommon();
		public IRepository<ChannelPartner> PartnerRepo
		{
			get { return _partnerRepo; }
			set { _partnerRepo = value; }
		}

		public IRepository<Message> MessageRepo
		{
			get { return _messageRepo; }
			set { _messageRepo = value; }
		}

		public long Add(long partnerId, string key, Language lang, Message message, string addlDetails)
		{
			try
			{
				Message newMessage = new Message
				{
					Partner = partnerId,
					MessageKey = key,
					Language = lang,
					Content = message.Content,
					DTServerCreate = DateTime.Now,
					DTServerLastModified = DateTime.Now,
					AddlDetails = message.AddlDetails,
					Processor = message.Processor
				};

				_messageRepo.AddWithFlush(newMessage);
				return newMessage.Id;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_ADD_FAILED,ex);
			}
		}

		public Message Lookup(long Id)
		{
			return _messageRepo.FindBy(x => x.Id == Id);
		}

		private static readonly long DEFAULT_PARTNER = 1;
		private static readonly Language DEFAULT_LANG = Language.EN;

		/// <summary>
		/// partner - key - lang
		/// default - key - lang
		/// partner - key - en [if lang diff than en]
		/// default - key - en
		/// null.
		/// </summary>
		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public Message Lookup(long partnerId, string key, Language lang)
		{
			Message message = _GetMessage(partnerId, key, lang);

			if (message == null)
			{
				message = _GetMessage(DEFAULT_PARTNER, key, lang);
			}

			if (message == null && !lang.Equals(DEFAULT_LANG))
			{
				message = _GetMessage(partnerId, key, DEFAULT_LANG);

				if (message == null)
				{
					message = _GetMessage(DEFAULT_PARTNER, key, DEFAULT_LANG);
				}
			}

			return message != null ? message : null;
		}

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        public Message Lookup(long partnerId, string productKey, string providerKey, string errorKey, Language lang)
        {
            // get error specific message
            Message message = _messageRepo.FindBy(c => c.Partner == partnerId && c.Language == lang &&  c.ProductKey == productKey && c.ProviderKey == providerKey && c.ErrorKey == errorKey);

            return message;
        }

		private Message _GetMessage(long partnerId, string key, Language lang)
		{
			try
			{
				return _messageRepo.FindBy(x => x.Partner == partnerId
						&& x.MessageKey == key && x.Language == lang);

			}
			catch (InvalidOperationException ex)
			{
				NLogger.Error(string.Format("Error in retrieving : {0} \n Stack Trace: {1}", ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));
				return null;
			}
		}
	}
}
