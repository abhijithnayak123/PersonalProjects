using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Common.Util;

using MGI.Common.DataAccess.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class BillPaySetupServiceImpl : IBillPaySetupService
	{
		public IRepository<CustomerPreferedProduct> CustomerPreferedProductRepo { private get; set; }
        public NLoggerCommon NLogger { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }

		public BillPaySetupServiceImpl()
		{

		}

		public long Create(CustomerPreferedProduct customerPrefered)
		{
			try
			{
				CustomerPreferedProductRepo.AddWithFlush(customerPrefered);
				return customerPrefered.Id;
			}
			catch (Exception ex)
			{
                NLogger.Error("Error while adding Customer Prefered Product. " + ex.Message);
                //AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CustomerPreferedProduct>(customerPrefered, "Create", AlloyLayerName.CXE,
				ModuleName.BillPayment, "Error in Create -MGI.Core.CXE.Impl.BillPaySetupServiceImpl", ex.Message, ex.StackTrace);
				throw new Exception("Error while adding Customer Prefered Product",ex);
			}
		}

		public bool Update(CustomerPreferedProduct customerPrefered, string timezone)
		{
			try
			{
				customerPrefered.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				customerPrefered.DTServerLastModified = DateTime.Now;
				return CustomerPreferedProductRepo.UpdateWithFlush(customerPrefered);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error while updating Customer Prefered Product. " + ex.Message);
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<CustomerPreferedProduct>(customerPrefered, "Update", AlloyLayerName.CXE, ModuleName.BillPayment, 
				"Error in Update -MGI.Core.CXE.Impl.BillPaySetupServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error while updating Customer Prefered Product",ex);
			}
		}

		public CustomerPreferedProduct Get(long ProductId, long alloyId)
		{
			try
			{
				return CustomerPreferedProductRepo.FindBy(x => x.ProductId == ProductId && x.AlloyID == alloyId);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error while getting Customer Prefered Product." + ex.Message);
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Product Id :" + Convert.ToString(ProductId));
				details.Add("Alloy Id :" + Convert.ToString(alloyId));
				MongoDBLogger.ListError<string>(details, "Get", AlloyLayerName.CXE, ModuleName.BillPayment,
				"Error in Get -MGI.Core.CXE.Impl.BillPaySetupServiceImpl", ex.Message, ex.StackTrace);
				
				throw new Exception("Error while getting Customer Prefered Product",ex);
			}
		}


        /// <summary>
        /// This is new method added for US1646 for Past Billers.
        /// </summary>
        /// <param name="billerName">Receiver Index</param>
		/// <param name="customerID">Customer ID</param>
        /// <returns>CustomerPreferedProduct for a particular receiver index.</returns>
        public CustomerPreferedProduct GetBillerReceiverIndex(long productId,long customerID)
        {
            try
            {
				return CustomerPreferedProductRepo.FindBy(x => x.ProductId == productId && x.AlloyID == customerID);
            }
            catch (Exception ex)
            {
                NLogger.Error("Error while getting Customer Prefered Product for Receiver Index. " + ex.Message);
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Product Id :" + Convert.ToString(productId));
				details.Add("Customer Id :" + Convert.ToString(customerID));
				MongoDBLogger.ListError<string>(details, "GetBillerReceiverIndex", AlloyLayerName.CXE, ModuleName.BillPayment, 
				"Error in GetBillerReceiverIndex -MGI.Core.CXE.Impl.BillPaySetupServiceImpl", ex.Message, ex.StackTrace);
				
                throw new Exception("Error while getting Customer Prefered Product for Receiver Index.",ex);
            }
        }


		public long[] GetPrefered(long AlloyID)
        {
            try
            {
				var productslist = CustomerPreferedProductRepo.FilterBy(x => x.AlloyID == AlloyID && x.Enabled == true)
                    .OrderByDescending(x => x.DTTerminalLastModified == null ? x.DTTerminalCreate : x.DTTerminalLastModified)
                    .Select(x => x.ProductId).ToArray();

                //if (productslist.Count() > 4)
                //    return productslist.Take(4).ToArray();
                //else
                    return productslist.ToArray();
            }
            catch (Exception ex)
            {
                NLogger.Error("Error while getting top 4 Customer Prefered Product. " + ex.Message);
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(AlloyID), "GetPrefered", AlloyLayerName.CXE, ModuleName.BillPayment,
				"Error in GetPrefered -MGI.Core.CXE.Impl.BillPaySetupServiceImpl", ex.Message, ex.StackTrace);
				throw new Exception("Error while getting top 4 Customer Prefered Product", ex);
            }     
      
        }
    }
}
