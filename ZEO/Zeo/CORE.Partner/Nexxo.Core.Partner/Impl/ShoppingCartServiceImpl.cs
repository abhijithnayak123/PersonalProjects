using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.Partner.Impl
{
    public class ShoppingCartServiceImpl : IShoppingCartService
    {
        private IRepository<ShoppingCart> _scartRepo;

        public IRepository<ShoppingCart> ShoppingCartRepo
            { set { _scartRepo = value; } }

        public TLoggerCommon MongoDBLogger { get; set; }


        public ShoppingCart Lookup(long id)
        {
            try
            {
                return _scartRepo.FindBy(x => x.Id == id);
            }
            catch(Exception ex)
            {
                MongoDBLogger.Error(Convert.ToString(id), "Lookup", AlloyLayerName.CORE, ModuleName.ShoppingCart,
                           "Error in Lookup - MGI.Core.Partner.Impl.ShoppingCartServiceImpl", ex.Message, ex.StackTrace);

                throw new ShoppingCartServiceException(ShoppingCartServiceException.SHOPPINGCART_LOOKUP_FAILED, ex);
            }
        }

		public void Update(long id, ShoppingCartStatus status)
		{
            try
            {
			    ShoppingCart shoppingCart = _scartRepo.FindBy(x => x.Id == id);
			    shoppingCart.Status = status;
			    //Updating ShoppingCart to persist Status
			    _scartRepo.Update(shoppingCart);
            }
            catch(Exception ex)
            {
                List<string> details = new List<string>();
                details.Add(Convert.ToString(id));
                details.Add(Convert.ToString(status));
                MongoDBLogger.ListError(details, "Update", AlloyLayerName.CORE, ModuleName.ShoppingCart,
                          "Error in Updating Status - MGI.Core.Partner.Impl.ShoppingCartServiceImpl", ex.Message, ex.StackTrace);

                throw new ShoppingCartServiceException(ShoppingCartServiceException.SHOPPINGCART_STATUS_UPDATE_FAILED, ex);
            }
		}

		/// <summary>
		/// US1800 Referral & Referree Promotions
		/// Updating ShoppingCart IsReferral Flag 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="IsReferral"></param>
		public void Update(long id, bool IsReferral)
        {
            try
            {
                ShoppingCart shoppingCart = _scartRepo.FindBy(x => x.Id == id);
                shoppingCart.IsReferral = IsReferral;
			    _scartRepo.Update(shoppingCart);
            }
            catch(Exception ex)
            {
                List<string> details = new List<string>();
                details.Add(Convert.ToString(id));
                details.Add(Convert.ToString(IsReferral));
                MongoDBLogger.ListError(details, "Update", AlloyLayerName.CORE, ModuleName.ShoppingCart,
                          "Error in Updating IsReferral - MGI.Core.Partner.Impl.ShoppingCartServiceImpl", ex.Message, ex.StackTrace);

                throw new ShoppingCartServiceException(ShoppingCartServiceException.SHOPPINGCART_REFERRAL_UPDATE_FAILED, ex);
            }
        }

		/// <summary>
		/// US1800 Referral & Referree Promotions
		/// Get ShoppingCarts for Customer AlloyID No
		/// </summary>
		/// <param name="AlloyID"></param>
		/// <returns></returns>
		public List<ShoppingCart> LookupForCustomer(long alloyId)
		{
            try
            {
			    var shoppingCarts = _scartRepo.FilterBy(x => x.Customer.CXEId == alloyId);
			    if (shoppingCarts != null)
				    return shoppingCarts.ToList();
			    else
				    return null;
            }
            catch(Exception ex)
            {
                MongoDBLogger.Error(Convert.ToString(alloyId), "LookupForCustomer", AlloyLayerName.CORE, ModuleName.ShoppingCart,
                         "Error in LookupForCustomer - MGI.Core.Partner.Impl.ShoppingCartServiceImpl", ex.Message, ex.StackTrace);

                throw new ShoppingCartServiceException(ShoppingCartServiceException.SHOPPINGCART_LOOKUP_CUSTOMER_FAILED, ex);
            }
		}

        public IList<ShoppingCart> GetAllParkedShoppingCarts()
        {
            try
            {
                var parkedShoppingCarts = _scartRepo.All();

                return parkedShoppingCarts.ToList();
            }
            catch(Exception ex)
            {
                MongoDBLogger.Error(string.Empty, "GetAllParkedShoppingCarts", AlloyLayerName.CORE, ModuleName.ShoppingCart,
                        "Error in GetAllParkedShoppingCarts - MGI.Core.Partner.Impl.ShoppingCartServiceImpl", ex.Message, ex.StackTrace);

                throw new ShoppingCartServiceException(ShoppingCartServiceException.SHOPPINGCART_GET_PARKEDITEMS_FAILED, ex);
            }
        }
	}
}
