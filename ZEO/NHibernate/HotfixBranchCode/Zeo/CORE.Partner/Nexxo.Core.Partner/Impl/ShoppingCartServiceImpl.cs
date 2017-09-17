using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Impl
{
    public class ShoppingCartServiceImpl : IShoppingCartService
    {
        private IRepository<ShoppingCart> _scartRepo;

        public IRepository<ShoppingCart> ShoppingCartRepo
            { set { _scartRepo = value; } }

        public ShoppingCart Lookup(long id)
        {
            return _scartRepo.FindBy(x => x.Id == id);
        }

		public void Update(long id, ShoppingCartStatus status)
		{
			ShoppingCart shoppingCart = _scartRepo.FindBy(x => x.Id == id);
			shoppingCart.Status = status;
			//Updating ShoppingCart to persist Status
			_scartRepo.Update(shoppingCart);
		}

		/// <summary>
		/// US1800 Referral & Referree Promotions
		/// Updating ShoppingCart IsReferral Flag 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="IsReferral"></param>
		public void Update(long id, bool IsReferral)
        {
            ShoppingCart shoppingCart = _scartRepo.FindBy(x => x.Id == id);
            shoppingCart.IsReferral = IsReferral;
			_scartRepo.Update(shoppingCart);
        }

		/// <summary>
		/// US1800 Referral & Referree Promotions
		/// Get ShoppingCarts for Customer AlloyID No
		/// </summary>
		/// <param name="AlloyID"></param>
		/// <returns></returns>
		public List<ShoppingCart> LookupForCustomer(long alloyId)
		{
			var shoppingCarts = _scartRepo.FilterBy(x => x.Customer.CXEId == alloyId);
			if (shoppingCarts != null)
				return shoppingCarts.ToList();
			else
				return null;
		}

        public IList<ShoppingCart> GetAllParkedShoppingCarts()
        {
            var parkedShoppingCarts = _scartRepo.FilterBy(c => c.IsParked == true && c.Active == true);

            return parkedShoppingCarts.ToList();
        }
	}
}
