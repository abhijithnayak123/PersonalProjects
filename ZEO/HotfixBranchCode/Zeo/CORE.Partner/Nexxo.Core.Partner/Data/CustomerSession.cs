using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Common.DataAccess.Data;
using MGI.Core.Partner.Data.Transactions;

namespace MGI.Core.Partner.Data
{
	public class CustomerSession : NexxoModel
	{
		public virtual AgentSession AgentSession { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual DateTime DTStart { get; set; }
		public virtual Nullable<DateTime> DTEnd { get; set; }
		public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
		//TA3520 -Start
		public virtual bool CardPresent { get; set; }
		//TA3520 - End

        //US1488 Parking Transaction Changes
        private ShoppingCart _parkingShoppingCart = null;
        //Implemeted as part of MVA Implementation
        public virtual string TimezoneID { get; set; }
		public virtual CustomerSessionCounter CustomerSessionCounter {get; set;}
		public CustomerSession()
		{
			ShoppingCarts = new List<ShoppingCart>();
		}

		private ShoppingCart _activeShoppingCart = null;
		/// <summary>
		/// Returns the active ShoppingCart for the session, if there is one
		/// </summary>
		public virtual ShoppingCart ActiveShoppingCart
		{
			get
			{
				if ( _activeShoppingCart == null || !_activeShoppingCart.Active )
				{
					if ( ShoppingCarts.Count > 0 && ShoppingCarts.Any<ShoppingCart>( sc => sc.Active && sc.IsParked == false ) )
						_activeShoppingCart = ShoppingCarts.First<ShoppingCart>( sc => sc.Active && sc.IsParked == false );
					else
						_activeShoppingCart = null;
				}
				return _activeShoppingCart;
			}
		}

		/// <summary>
		/// Whether the session has an active ShoppingCart
		/// </summary>
		/// <returns>true/false</returns>
		public virtual bool HasActiveShoppingCart()
		{
			return ActiveShoppingCart != null;
		}

		/// <summary>
		/// Add a new ShoppingCart for the session
		/// </summary>
		/// <returns>ShoppingCart</returns>
        public virtual ShoppingCart AddShoppingCart(string timezone)
		{
			ShoppingCart cart = new ShoppingCart
			{
				Active = true,
				Customer = this.Customer,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				DTServerCreate = DateTime.Now
			};
			ShoppingCarts.Add( cart );
			return cart;
		}

        // US1488 Parking Transaction Changes
        /// <summary>
        /// 
        /// </summary>
        public virtual ShoppingCart ParkingShoppingCart
        {
            get
            {
                if (_parkingShoppingCart == null)
                {
                    if (ShoppingCarts.Count > 0 && ShoppingCarts.Any<ShoppingCart>(sc => sc.IsParked))
                        _parkingShoppingCart = ShoppingCarts.First<ShoppingCart>(sc => sc.IsParked);
                    else
                        _parkingShoppingCart = null;
                }
                return _parkingShoppingCart;
            }
        }

        // US1488 Parking Transaction Changes
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ShoppingCart AddParkingShoppingCart(string timezone)
        {
            ShoppingCart cart = new ShoppingCart
            {
                Active = true,
                Customer = this.Customer,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
                DTServerCreate = DateTime.Now,
                IsParked = true
            };
            ShoppingCarts.Add(cart);
            return cart;
        }


        // US1488 Parking Transaction Changes
        /// <summary>
        /// Add a new ShoppingCart for the session
        /// </summary>
        /// <returns>ShoppingCart</returns>
        public virtual ShoppingCart AddShoppingCart(ShoppingCart cart, string timezone)
        {
			cart.IsParked = false;
            cart.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            cart.DTServerLastModified = DateTime.Now;
            ShoppingCarts.Add(cart);
            _activeShoppingCart = cart;
            return cart;
        }	

	}
}
