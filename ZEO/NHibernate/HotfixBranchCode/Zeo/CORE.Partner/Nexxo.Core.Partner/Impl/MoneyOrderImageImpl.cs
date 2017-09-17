using MGI.Common.DataAccess.Contract;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Impl
{
	public class MoneyOrderImageImpl : IMoneyOrderImage
	{
		public IRepository<MoneyOrderImage> PtnrMoneyOrderRepo { private get; set; }
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public long Create(MoneyOrderImage moneyOrderImage, string timezone)
		{
			try
			{
				if (moneyOrderImage.DTTerminalCreate == DateTime.MinValue)
				{
					moneyOrderImage.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
					moneyOrderImage.DTServerCreate = DateTime.Now;
				}

				PtnrMoneyOrderRepo.AddWithFlush(moneyOrderImage);

				return moneyOrderImage.Id;
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in saving moneyOrderImage data in PTNR. " + ex.Message);

				MongoDBLogger.Error<MoneyOrderImage>(moneyOrderImage, "Create", AlloyLayerName.CORE, ModuleName.MoneyOrder,
						"Error in Create - MGI.Core.Partner.Impl.MoneyOrderImageImpl", ex.Message, ex.StackTrace);

				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_MONEYORDERIMAGE_FAILED, ex);
			}
		}

		public void Update(MoneyOrderImage moneyOrderImage, string timezone)
		{
			moneyOrderImage.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			moneyOrderImage.DTServerLastModified = DateTime.Now;
			try
			{
				PtnrMoneyOrderRepo.Merge(moneyOrderImage);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in saving moneyOrderImage data in PTNR. " + ex.Message);

				MongoDBLogger.Error<MoneyOrderImage>(moneyOrderImage, "Update", AlloyLayerName.CORE, ModuleName.MoneyOrder,
								"Error in Update - MGI.Core.Partner.Impl.MoneyOrderImageImpl", ex.Message, ex.StackTrace);

				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_MONEYORDERIMAGE_FAILED, ex);
			}
		}

		public MoneyOrderImage FindMoneyOrderByTxnId(Guid txnId)
		{
			try
			{
				MoneyOrderImage moneyOrderImage = new MoneyOrderImage();

				moneyOrderImage = PtnrMoneyOrderRepo.FindBy(c => c.TrxId == txnId);

				return moneyOrderImage;
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<Guid>(txnId, "FindMoneyOrderByTxnId", AlloyLayerName.CORE, ModuleName.MoneyOrder,
								"Error in FindMoneyOrderByTxnId - MGI.Core.Partner.Impl.MoneyOrderImageImpl", ex.Message, ex.StackTrace);

				throw new PartnerCustomerException(PartnerCustomerException.CUSTOMER_NOT_FOUND, ex);
			}
		}
	}
}
