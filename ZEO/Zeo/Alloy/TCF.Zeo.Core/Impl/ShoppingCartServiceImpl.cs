using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Data.Exceptions;
using P3Net.Data;
using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace TCF.Zeo.Core.Impl
{
    public class ShoppingCartServiceImpl : IShoppingCartService
    {
        #region IShoppingCartService's Methods

        public bool AddShoppingCartTransaction(long customerSessionId, long transactionId, long productId, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure spAddShoppingCartTxn = new StoredProcedure("usp_AddShoppingCartTransaction");

                spAddShoppingCartTxn.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                spAddShoppingCartTxn.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                spAddShoppingCartTxn.WithParameters(InputParameter.Named("productId").WithValue(productId));
                spAddShoppingCartTxn.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                spAddShoppingCartTxn.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(spAddShoppingCartTxn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.ADD_TRANSACTION_TO_CART_FAILED, ex);
            }

        }

        public bool ParkShoppingCartTransaction(long customerSessionId, long transactionId, long productId, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure spParkShoppingCartTxn = new StoredProcedure("usp_ParkShoppingCartTransaction");

                spParkShoppingCartTxn.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                spParkShoppingCartTxn.WithParameters(InputParameter.Named("TransactionId").WithValue(transactionId));
                spParkShoppingCartTxn.WithParameters(InputParameter.Named("ProductId").WithValue(productId));
                spParkShoppingCartTxn.WithParameters(InputParameter.Named("ServerDate").WithValue(DateTime.Now));
                spParkShoppingCartTxn.WithParameters(InputParameter.Named("TerminalDate").WithValue(Helper.GetTimeZoneTime(timeZone)));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(spParkShoppingCartTxn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.PARK_TRANSACTION_FAILED, ex);
            }

        }

        public bool RemoveShoppingCartTransaction(long transactionId, int productId, ZeoContext context)
        {
            try
            {
                StoredProcedure spRemoveShoppingCartTxn = new StoredProcedure("usp_RemoveShoppingCartTransaction");
                //REVIEW-SC: We have to pass modified date also to update the record.
                spRemoveShoppingCartTxn.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                spRemoveShoppingCartTxn.WithParameters(InputParameter.Named("ProductId").WithValue(productId));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(spRemoveShoppingCartTxn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.REMOVE_ITEM_FROM_CART_FAILED, ex);
            }
        }

        public ShoppingCart GetShoppingCart(long customerSessionId, ZeoContext context)
        {
            try
            {
                ShoppingCart cart = new ShoppingCart();

                StoredProcedure spActiveShoppingCart = new StoredProcedure("usp_GetActiveShoppingCart");

                spActiveShoppingCart.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));

                using (IDataReader dr = DataHelper.GetConnectionManager().ExecuteReader(spActiveShoppingCart))
                {
                    while (dr.Read())
                    {
                        cart.Id = dr.GetInt64OrDefault("CartId");
                        cart.IsReferral = dr.GetBooleanOrDefault("IsReferral");
                        cart.IsReferralSectionEnabled = dr.GetBooleanOrDefault("IsReferralSectionEnabled");
                        cart.IsCheckFrank = dr.GetBooleanOrDefault("IsCheckFrank");
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Checks.Add(new Check
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Description = dr.GetStringOrDefault("Description"),
                            DmsDeclineMessage = dr.GetStringOrDefault("DeclineMessage"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            BaseFee = dr.GetDecimalOrDefault("BaseFee"),
                            DiscountApplied = dr.GetDecimalOrDefault("DiscountApplied"),
                            DiscountName = dr.GetStringOrDefault("DiscountName"),
                            ProductProviderCode = (Helper.ProviderId)dr.GetInt64OrDefault("ProviderId")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Bills.Add(new BillPay
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            Description = dr.GetStringOrDefault("Description"),
                            AccountNumber = dr.GetStringOrDefault("AccountNumber"),
                            ProductId = dr.GetInt64OrDefault("BillerId"),
                            BillerNameOrCode = dr.GetStringOrDefault("BillerName"),
                            ConfirmationNumber = dr.GetStringOrDefault("ConfirmationNumber")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.MoneyOrders.Add(new MoneyOrder
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            Description = dr.GetStringOrDefault("Description"),
                            ConfirmationNumber = dr.GetStringOrDefault("ConfirmationNumber"),
                            BaseFee = dr.GetDecimalOrDefault("BaseFee"),
                            DiscountApplied = dr.GetDecimalOrDefault("DiscountApplied"),
                            DiscountName = dr.GetStringOrDefault("DiscountName")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Funds.Add(new Funds
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            Description = dr.GetStringOrDefault("Description"),
                            BaseFee = dr.GetDecimalOrDefault("BaseFee"),
                            DiscountApplied = dr.GetDecimalOrDefault("DiscountApplied"),
                            DiscountName = dr.GetStringOrDefault("DiscountName"),
                            FundsType = (Helper.FundType)dr.GetUInt32OrDefault("Fundtype"),
                            CardNumber = dr.GetStringOrDefault("CardNumber"),
                            // AddOnCustomerName = dr.GetStringOrDefault("AddOnCustomerName") - Only for AddOn Card customer.
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.MoneyTransfers.Add(new MoneyTransfer
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = (Helper.TransactionStates)dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            MoneyTransferType = (Helper.MoneyTransferType)(int.Parse(dr.GetStringOrDefault("TranascationType"))),
                            TransactionSubType = dr.GetStringOrDefault("TransactionSubType") != string.Empty ? (Helper.TransactionSubType?)(int.Parse(dr.GetStringOrDefault("TransactionSubType"))) : null, //Manikandan - Need verify this code.............
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            ConfirmationNumber = dr.GetStringOrDefault("ConfirmationNumber"),
                            Description = dr.GetStringOrDefault("Description"),
                            DestinationCountry = dr.GetStringOrDefault("DestinationCountryCode"),
                            DestinationAmount = dr.GetDecimalOrDefault("DestinationPrincipalAmount"),
                            DestinationCurrency = dr.GetStringOrDefault("DestinationCurrencyCode"),
                            ExchangeRate = dr.GetDecimalOrDefault("ExchangeRate"),
                            MoneyTransferTotal = dr.GetDecimalOrDefault("GrossTotalAmount"),
                            OtherTax = dr.GetDecimalOrDefault("TaxAmount"),
                            ReceiverFirstName = dr.GetStringOrDefault("RecieverFirstName"),
                            ReceiverLastName = dr.GetStringOrDefault("RecieverLastName"),
                            ReceiverAddress = dr.GetStringOrDefault("Address"),
                            ReceiverCity = dr.GetStringOrDefault("City"),
                            ReceiverState = dr.GetStringOrDefault("DestinationState"),
                            SourceCountry = dr.GetStringOrDefault("OriginatingCountryCode"),
                            SourceCurrency = dr.GetStringOrDefault("OriginatingCurrencyCode"),
                            TransferTax = dr.GetDecimalOrDefault("TaxAmount"),
                            OriginalTransactionID = dr.GetInt64OrDefault("OriginalTransactionId"),
                            SenderFirstName = dr.GetStringOrDefault("FirstName"),
                            SenderLastName = dr.GetStringOrDefault("LastName"),
                            SenderMiddleName = dr.GetStringOrDefault("MiddleName"),
                            SenderSecondLastName = dr.GetStringOrDefault("LastName2"),
                            OtherFee = dr.GetDecimalOrDefault("Charges")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Cash.Add(new Cash
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            CashType = (Helper.CashType)dr.GetInt32OrDefault("CashType")
                        });
                    }

                }

                return cart;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.GET_SHOPPINGCART_FAILED, ex);
            }

        }

        public ShoppingCart GetShoppingCartById(long cartId, ZeoContext context)
        {
            try
            {
                ShoppingCart cart = new ShoppingCart();

                StoredProcedure spActiveShoppingCart = new StoredProcedure("usp_GetShoppingCart");

                spActiveShoppingCart.WithParameters(InputParameter.Named("ShoppingCartId").WithValue(cartId));

                using (IDataReader dr = DataHelper.GetConnectionManager().ExecuteReader(spActiveShoppingCart))
                {
                    while (dr.Read())
                    {
                        cart.Checks.Add(new Check
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Bills.Add(new BillPay
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State")
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.MoneyOrders.Add(new MoneyOrder
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Funds.Add(new Funds
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            Fee = dr.GetDecimalOrDefault("Fee"),
                            Description = dr.GetStringOrDefault("Description"),
                            BaseFee = dr.GetDecimalOrDefault("BaseFee"),
                            DiscountApplied = dr.GetDecimalOrDefault("DiscountApplied"),
                            DiscountName = dr.GetStringOrDefault("DiscountName"),
                            FundsType = (Helper.FundType)dr.GetUInt32OrDefault("Fundtype"),
                            CardNumber = dr.GetStringOrDefault("CardNumber"),
                            // AddOnCustomerName = dr.GetStringOrDefault("AddOnCustomerName") - Only for AddOn Card customer.
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.MoneyTransfers.Add(new MoneyTransfer
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = (Helper.TransactionStates)dr.GetInt32OrDefault("State"),
                            MoneyTransferType = (Helper.MoneyTransferType)(int.Parse(dr.GetStringOrDefault("TranascationType"))),
                            TransactionSubType = dr.GetStringOrDefault("TransactionSubType") != string.Empty ? (Helper.TransactionSubType?)(int.Parse(dr.GetStringOrDefault("TransactionSubType"))) : null //Manikandan - Need verify this code.............
                        });
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cart.Cash.Add(new Cash
                        {
                            Id = dr.GetInt64OrDefault("TransactionId"),
                            State = dr.GetInt32OrDefault("State"),
                        });
                    }
                }

                return cart;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.GET_SHOPPINGCARTBYID_FAILED, ex);
            }
        }

        public void CashOutAndUpdateReferral(long customerSessionId, decimal cashToCustomer, int cartStatus, long cartId, string timeZone, bool isReferral, ZeoContext context)
        {
            try
            {
                StoredProcedure spCashOutAndUpdateCart = new StoredProcedure("usp_CashOutAndUpdateReferral");

                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("cartId").WithValue(cartId));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("cartStatus").WithValue(cartStatus));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("isReferral").WithValue(isReferral));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("cashToCustomer").WithValue(cashToCustomer));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));

                DataHelper.GetConnectionManager().ExecuteNonQuery(spCashOutAndUpdateCart);
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.SHOPPINGCART_REFERRAL_UPDATE_FAILED, ex);
            }

        }

        public ShoppingCartCheckOut GetShoppingCartCheckOutDetails(long customerSessionId, long channelParnerId, bool isReferral, ZeoContext context)
        {
            try
            {
                ShoppingCartCheckOut cartCheckOut = new ShoppingCartCheckOut();

                StoredProcedure cartCheckOutProc = new StoredProcedure("usp_GetShoppingCartCheckout");

                cartCheckOutProc.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                cartCheckOutProc.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelParnerId));
                cartCheckOutProc.WithParameters(InputParameter.Named("isReferral").WithValue(isReferral));

                using (IDataReader dr = DataHelper.GetConnectionManager().ExecuteReader(cartCheckOutProc))
                {
                     while (dr.Read())
                     {
                         cartCheckOut.CartId = dr.GetInt64OrDefault("CartId");
                         cartCheckOut.IsCashOverCounter = dr.GetBooleanOrDefault("IsCashOverCounter");
                     }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        cartCheckOut.transactions.Add(new ShoppingCartTransaction()
                        {
                            TransactionId = dr.GetInt64OrDefault("TransactionId"),
                            ProductId = (Helper.Product)dr.GetInt32("ProductId"),
                            State = (Helper.TransactionStates)dr.GetInt32("State"),
                            CheckNumber = dr.GetStringOrDefault("CheckNumber"),
                            TransactionType = dr.GetInt32OrDefault("TransactionType"),
                            TransactionSubType = dr.GetStringOrDefault("TransactionSubType") != string.Empty ? (int.Parse(dr.GetStringOrDefault("TransactionSubType"))) : (int?)null,
                            Amount = dr.GetDecimalOrDefault("Amount"),
                            ProviderId = (Helper.ProviderId)dr.GetInt64("ProviderId")
                        });
                    }
                }

                return cartCheckOut;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.GET_SHOPPINGCART_CHECKOUT_DETAILS_FAILED, ex);
            }
        }

        public void CashOutAndUpdateCartStatus(long cartId, decimal amount, long customerSessionId, int cartStatus, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure spCashOutAndUpdateCart = new StoredProcedure("usp_CashOutAndUpdateCartStatus");

                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("cartId").WithValue(cartId));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("cartStatus").WithValue(cartStatus));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("amount").WithValue(amount));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                spCashOutAndUpdateCart.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));

                DataHelper.GetConnectionManager().ExecuteNonQuery(spCashOutAndUpdateCart);
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.SHOPPINGCART_STATUS_UPDATE_FAILED, ex);
            }
        }

        public bool IsShoppingCartEmpty(long customerSessionId, ZeoContext context)
        {
            try
            {
                bool isCartPresent = false;
                StoredProcedure checkCartProcedure = new StoredProcedure("usp_IsCartPresent");
                checkCartProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(checkCartProcedure))
                {
                    while (reader.Read())
                    {
                        isCartPresent = reader.GetBooleanOrDefault("IsCartPresent");
                    }
                }

                return isCartPresent;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.IS_SHOPPINGCART_EMPTY_FAILED, ex);
            }
        }

        public bool CanCloseCustomerSession(long customerSessionId, string timeZone, ZeoContext context)
        {
            try
            {
                bool isCartPresent = false;
                StoredProcedure checkCartProcedure = new StoredProcedure("usp_CanCloseCustomerSession");
                checkCartProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                checkCartProcedure.WithParameters(InputParameter.Named("DTTerminalModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                checkCartProcedure.WithParameters(InputParameter.Named("DTServerModified").WithValue(DateTime.Now));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(checkCartProcedure))
                {
                    while (reader.Read())
                    {
                        isCartPresent = reader.GetBooleanOrDefault("IsCartItemPresent");
                    }
                }

                return isCartPresent;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.CAN_CLOSE_CUSTOMER_SESSION_FAILED, ex);
            }
        }

        public List<ParkedTransaction> GetAllParkedTransaction(ZeoContext context)
        {
            try
            {
                List<ParkedTransaction> transactions = new List<ParkedTransaction>();

                StoredProcedure cartCheckOutProc = new StoredProcedure("usp_GetAllParkedTransactions");

                using (IDataReader dr = DataHelper.GetConnectionManager().ExecuteReader(cartCheckOutProc))
                {
                    while (dr.Read())
                    {
                        transactions.Add(new ParkedTransaction()
                        {
                            TransactionId = dr.GetInt64OrDefault("TransactionId"),
                            ProductId = (Helper.Product)dr.GetInt32("ProductId"),
                            ChannelPartnerId = dr.GetInt32OrDefault("ChannelPartnerId"),
                            ChannelPartnerName = dr.GetStringOrDefault("ChannelPartnerName"),
                            LocationID = dr.GetInt64OrDefault("LocationID"),
                            LocationName = dr.GetStringOrDefault("LocationName"),
                            BankId = dr.GetStringOrDefault("BankID"),
                            BranchId = dr.GetStringOrDefault("BranchID"),
                            CustomerId = dr.GetInt64OrDefault("CustomerID"),
                            CheckPassword = dr.GetStringOrDefault("CheckPassword"),
                            CheckUserName = dr.GetStringOrDefault("CheckUserName"),
                            CustomerSessionId = dr.GetInt64OrDefault("CustomerSessionId"),
                            TimeZone = dr.GetStringOrDefault("TimeZoneId"),
                            AgentFirstName = dr.GetStringOrDefault("AgentFirstName"),
                            AgentLastName = dr.GetStringOrDefault("AgentLastName"),
                            AgentName = dr.GetStringOrDefault("AgentName"),
                            AgentId =  dr.GetInt64OrDefault("AgentID"),
                            WUCounterId = dr.GetStringOrDefault("WUCounterId"),
                            ProviderId = dr.GetInt32OrDefault("ProviderId")
                        });
                    }
                }

                return transactions;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.GET_SHOPPINGCART_PARKED_ITEM_FAILED, ex);
            }
        }

        public List<long> GetResubmitTransactions(int productId, long customerSessionId, ZeoContext context)
        {
            try
            {
                List<long> resubmitTransactions = new List<long>();

                StoredProcedure resubmitTransaction = new StoredProcedure("usp_GetResubmitTransactions");

                resubmitTransaction.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                resubmitTransaction.WithParameters(InputParameter.Named("ProductId").WithValue(productId));

                using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(resubmitTransaction))
                {
                    while (reader.Read())
                    {
                        resubmitTransactions.Add(reader.GetInt64OrDefault("TransactionId"));
                    }
                }

                return resubmitTransactions;
            }
            catch (Exception ex)
            {
                throw new ShoppingCartException(ShoppingCartException.IS_SHOPPINGCART_EMPTY_FAILED, ex);
            }
        }

        public ShoppingCart GetShoppingCartForReceipts(ZeoContext context)
        {
            ShoppingCart cart = new ShoppingCart();

            StoredProcedure spActiveShoppingCart = new StoredProcedure("usp_GetShoppingCartForReceipts");

            spActiveShoppingCart.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(context.CustomerSessionId));

            using (IDataReader dr = DataHelper.GetConnectionManager().ExecuteReader(spActiveShoppingCart))
            {
                while (dr.Read())
                {
                    cart.Checks.Add(new Check
                    {
                        Id = dr.GetInt64OrDefault("TransactionId")
                    });
                }

                dr.NextResult();

                while (dr.Read())
                {
                    cart.Bills.Add(new BillPay
                    {
                        Id = dr.GetInt64OrDefault("TransactionId")
                    });
                }

                dr.NextResult();

                while (dr.Read())
                {
                    cart.MoneyOrders.Add(new MoneyOrder
                    {
                        Id = dr.GetInt64OrDefault("TransactionId")
                    });
                }

                dr.NextResult();

                while (dr.Read())
                {
                    cart.Funds.Add(new Funds
                    {
                        Id = dr.GetInt64OrDefault("TransactionId"),
                        FundsType = (Helper.FundType)(dr.GetUInt32OrDefault("Fundtype"))
                    });
                }

                dr.NextResult();

                while (dr.Read())
                {
                    cart.MoneyTransfers.Add(new MoneyTransfer
                    {
                        Id = dr.GetInt64OrDefault("TransactionId"),
                        MoneyTransferType = (Helper.MoneyTransferType)(int.Parse(dr.GetStringOrDefault("TransferType"))),
                        TransactionSubType = dr.GetStringOrDefault("TransactionSubType") != string.Empty ? (Helper.TransactionSubType?)(int.Parse(dr.GetStringOrDefault("TransactionSubType"))) : null //Manikandan - Need verify this code.............
                    });
                }

                dr.NextResult();

                while (dr.Read())
                {
                    cart.Cash.Add(new Cash
                    {
                        Id = dr.GetInt64OrDefault("TransactionId")
                    });
                }
            }

            return cart;
        }


        #endregion


    }
}
