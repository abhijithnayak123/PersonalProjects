using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

#region Alloy Reference

using CoreData = TCF.Zeo.Core;
using ZeoData = TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Contract;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.BillPay.Contract;
using TCF.Zeo.Biz.MoneyOrder.Contract;
using TCF.Zeo.Biz.MoneyOrder.Impl;
using TCF.Zeo.Biz.Fund.Contract;
using TCF.Zeo.Biz.Fund.Impl;
using TCF.Zeo.Biz.BillPay.Impl;
using TCF.Zeo.Biz.MoneyTransfer.Contract;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Impl;
using TCF.Zeo.Biz.Customer.Contract;
using TCF.Zeo.Biz.Customer.Impl;
using TCF.Zeo.Biz.MoneyTransfer.Impl;
using TCF.Zeo.Biz.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Biz.Check.Impl;

#endregion

namespace TCF.Zeo.Biz.Impl
{
    public class ShoppingCartServiceImpl : IShoppingCartService
    {
        #region Configuration

        CoreData.Contract.IShoppingCartService ShoppingCartService = new CoreData.Impl.ShoppingCartServiceImpl();
        Check.Contract.ICPService CPService;
        IBillPayService BillPayService;
        IMoneyOrderServices MoneyOrderservice;
        IFundsEngine FundService;
        IComplianceService ComplianceService;
        IMoneyTransferEngine MoneyTransferService;

        IMapper mapper;

        #endregion

        public ShoppingCartServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.Data.ShoppingCart, ShoppingCart>().
                ForMember(a => a.GPRCards, b => b.MapFrom(c => c.Funds));
             
                cfg.CreateMap<CoreData.Data.Check, ZeoData.Check>()
                .ForMember(a => a.StatusDescription, b => b.MapFrom(c => c.Description))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.State))
                .ForMember(a => a.StatusMessage, b => b.MapFrom(c => c.DmsDeclineMessage))
                .ForMember(a => a.DmsStatusMessage, b => b.MapFrom(c => c.DmsDeclineMessage));

                cfg.CreateMap<CoreData.Data.BillPay, BillPayTransaction>()
                .ForMember(a => a.BillerName, b => b.MapFrom(c => c.BillerNameOrCode))
                .ForMember(a => a.BillerId, b => b.MapFrom(c => c.ProductId))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.State))
                .AfterMap((s, d) =>
                 {
                     d.AcceptedFee = true;
                     d.BillTotal = s.Amount + s.Fee;
                 });

                cfg.CreateMap<CoreData.Data.MoneyOrder, ZeoData.MoneyOrder>();

                cfg.CreateMap<CoreData.Data.ParkedTransaction, ZeoData.ParkedTransaction>();

                cfg.CreateMap<CoreData.Data.Cash, CartCash>()
                .ForMember(x => x.Status, o => o.MapFrom(s => s.State));

                cfg.CreateMap<CoreData.Data.MoneyTransfer, ZeoData.MoneyTransfer>()
                 .ForMember(a => a.SourceAmount, b => b.MapFrom(c => c.Amount))
                 .ForMember(a => a.Status, b => b.MapFrom(c => (int)c.State))
                 .ForMember(a => a.TransferType, b => b.MapFrom(c => (int)c.MoneyTransferType))
                 .ForMember(a => a.OtherFee, b => b.MapFrom(c => c.Fee))
                 .ForMember(a => a.TransactionSubType, b => b.MapFrom(c => (int?)c.TransactionSubType))
                 .AfterMap((s, d) =>
                  {
                      d.AcceptedFee = true;
                  });

                cfg.CreateMap<CoreData.Data.Funds, GPRCard>()
                .ForMember(x => x.ItemType, o => o.MapFrom(s => s.FundsType));


                cfg.CreateMap<ZeoContext, commonData.ZeoContext>();
            });
            mapper = config.CreateMapper();
        }

        #region IShoppingCartService's Methods

        public bool AddShoppingCartTransaction(long transactionId, long productId, commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.AddShoppingCartTransaction(context.CustomerSessionId, transactionId, productId, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.ADD_SHOPPINGCART_TRANSACTION_FAILED, ex);
            }
        }

        public bool ParkShoppingCartTransaction(long transactionId, long productId, commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.ParkShoppingCartTransaction(context.CustomerSessionId, transactionId, productId, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.PARK_SHOPPINGCART_TRANSACTION_FAILED, ex);
            }
        }

        public bool RemoveShoppingCartTransaction(long transactionId, int productId, commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.RemoveShoppingCartTransaction(transactionId, productId, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.REMOVE_SHOPPINGCART_TRANSACTION_FAILED, ex);
            }
        }

        public ShoppingCart GetShoppingCart(long customerSessionId, commonData.ZeoContext context)
        {
            try
            {
                CoreData.Data.ShoppingCart shoppingCart = ShoppingCartService.GetShoppingCart(customerSessionId, context);

                ShoppingCart cart = mapper.Map<ShoppingCart>(shoppingCart);

                cart.CheckTotal = cart.Checks.Sum(c => c.Amount);
                cart.BillTotal = cart.Bills.Sum(b => (b.Amount + b.Fee));
                cart.MoneyOrderTotal = cart.MoneyOrders.Sum(m => (m.Amount + m.Fee));
                cart.MoneyTransfeTotal = cart.MoneyTransfers.Sum(m => (m.Amount + m.Fee));
                cart.GPRCards = GetGPRCards(shoppingCart);
                cart.GprCardTotal = cart.GPRCards.Sum(f => (f.ActivationFee + f.LoadAmount + f.LoadFee + f.WithdrawAmount + f.WithdrawFee));
                cart.CashInTotal = cart.Cash.Where(i => i.CashType == Helper.CashType.CashIn).Sum(x => x.Amount);
                cart.CustomerSessionId = customerSessionId;

                return cart;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.GET_SHOPPINGCART_FAILED, ex);
            }
        }

        public ShoppingCart GetShoppingCartById(long cartId, commonData.ZeoContext context)
        {
            CoreData.Data.ShoppingCart shoppingCart = ShoppingCartService.GetShoppingCartById(cartId, context);

            ShoppingCart cart = mapper.Map<ShoppingCart>(shoppingCart);
            cart.GPRCards = GetGPRCards(shoppingCart);
            return cart;
        }

        public Helper.ShoppingCartCheckoutStatus ShoppingCartCheckout(decimal cashToCustomer, Helper.ShoppingCartCheckoutStatus cartCheckOutStatus, commonData.ZeoContext context)
        {

            try
            {
                CoreData.Data.ShoppingCartCheckOut cartCheckout = ShoppingCartService.GetShoppingCartCheckOutDetails(context.CustomerSessionId, context.ChannelPartnerId, context.IsReferral, context);

                Preflush(cashToCustomer, cartCheckout, cartCheckOutStatus, context);

                // Check out the funds generating transactions (Check processing, Receive Money, Refund)

                CheckoutFundsGenerating(cartCheckout, context);

                // Withdraw checkout, Check for CashOverCounter true, then process the fund transaction and return the status

                if (IsCashOverCounterWithDraw(cartCheckout, cartCheckOutStatus, context))
                    return Helper.ShoppingCartCheckoutStatus.CashOverCounter;


                // Check out the funds depleting transactions. (BillPay, Send Money, Modify, Money Order)

                CheckoutFundsDepleting(cartCheckout, context);

                if (IsMoneyOrderAuthorized(cartCheckout.transactions, context))
                    return Helper.ShoppingCartCheckoutStatus.MOPrinting;

                // Checkout Load, Activate, AddOn card

                CommitFunds(cartCheckout.transactions, context);


                // Cash Out, Add Or Update Customer Fee adjustment(Referral Referee Promotions) , Update shopping cart status as 'Completed' 

                CashOutAndUpdateReferral(Helper.ShoppingCartCheckoutStatus.Completed, cashToCustomer, cartCheckout.CartId, context);

                return Helper.ShoppingCartCheckoutStatus.Completed;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.SHOPPINGCART_CHECKOUT_FAILED, ex);
            }
        }

        public bool IsShoppingCartEmpty(commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.IsShoppingCartEmpty(context.CustomerSessionId, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.ISSHOPPINGCARTEMPTY_FAILED, ex);
            }
        }

        public bool CanCloseCustomerSession(commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.CanCloseCustomerSession(context.CustomerSessionId, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.CANCLOSECUSTOMERSESSION_FAILED, ex);
            }
        }

        public List<ParkedTransaction> GetAllParkedTransaction(commonData.ZeoContext context)
        {
            try
            {
                return mapper.Map<List<ZeoData.ParkedTransaction>>(ShoppingCartService.GetAllParkedTransaction(context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.GET_ALLPARKEDTRANSACTION_FAILED, ex);
            }
        }

        public Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, commonData.ZeoContext context)
        {
            try
            {
                ShoppingCart cart = mapper.Map<ShoppingCart>(ShoppingCartService.GetShoppingCartForReceipts(context));
                Receipts receipts = new Receipts();
                receipts.receiptType = new List<string>();
                if (cart == null)
                    return receipts;
                foreach (var check in cart.Checks)
                {
                    try
                    {
                        receipts.receiptType.Add(string.Format("Process Check - TrnxID : {0}", check.Id.ToString()));
                    }
                    catch { }
                }
                foreach (var bill in cart.Bills)
                {
                    try
                    {
                        receipts.receiptType.Add(string.Format("BillPay - TrnxID : {0}", bill.Id.ToString()));
                    }
                    catch { }
                }
                if (cart.GPRCards.Any())
                {
                    if (cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Activation.ToString()).Count() > 0)
                    {
                        try
                        {
                            GPRCard fundActivation = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Activation.ToString()).First();

                            receipts.receiptType.Add(string.Format("GPRActivation - TrnxID : {0}", fundActivation.Id.ToString()));
                        }
                        catch { }
                    }
                    else
                    {
                        foreach (var fund in cart.GPRCards)
                        {
                            try
                            {
                                if (fund.ItemType == Helper.FundType.Debit.ToString())
                                {
                                    receipts.receiptType.Add(string.Format("GPRWithdraw - TrnxID : {0}", fund.Id.ToString()));
                                }
                                else if (fund.ItemType == Helper.FundType.AddOnCard.ToString())
                                {
                                    receipts.receiptType.Add(string.Format(string.Format("Companion Card Order - TrnxID : {0}", fund.Id.ToString())));
                                }
                                else
                                {
                                    receipts.receiptType.Add(string.Format(string.Format("GPRLoad - TrnxID : {0}", fund.Id.ToString())));
                                }
                            }
                            catch { }
                        }
                    }
                }
                if (cart.MoneyTransfers.Any())
                {
                    foreach (var xfer in cart.MoneyTransfers)
                    {
                        try
                        {
                            if (xfer.TransferType == (int)Helper.MoneyTransferType.Send && xfer.TransactionSubType != ((int)Helper.TransactionSubType.Cancel).ToString() && xfer.TransactionSubType != ((int)Helper.TransactionSubType.Refund).ToString())
                            {
                                receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", Helper.ProductType.SendMoney.ToString(), xfer.Id.ToString()));
                            }
                            else if (xfer.TransferType == (int)Helper.MoneyTransferType.Receive)
                            {
                                receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", Helper.ProductType.ReceiveMoney.ToString(), xfer.Id.ToString()));
                            }
                            else if (xfer.TransferType == (int)Helper.MoneyTransferType.Refund)
                            {
                                receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", Helper.ProductType.Refund.ToString(), xfer.Id.ToString()));
                            }
                        }
                        catch { }
                    }
                }
                if (cart.MoneyOrders.Any())
                {
                    foreach (var moneyOrder in cart.MoneyOrders)
                    {
                        try
                        {
                            receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", Helper.ProductType.MoneyOrder.ToString(), moneyOrder.Id.ToString()));
                        }
                        catch { }
                    }
                }
                if (receipts.receiptType.Count > 0 || cart.Cash.Any())
                    receipts.receiptType.Add(string.Format("Summary - CartID : {0}", cart.Id));
                //US1800 Referral promotions – Free check cashing to referrer and referee 
                if (cart.IsReferral)
                    receipts.receiptType.Add("Coupon");

                return receipts;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.GENERATERECEIPTSFORSHOPPINGCART_FAILED, ex);
            }

        }

        public List<long> GetResubmitTransactions(int productId, long customerSessionId, commonData.ZeoContext context)
        {
            try
            {
                return ShoppingCartService.GetResubmitTransactions(productId, customerSessionId, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new ShoppingCartException(ShoppingCartException.GETRESUBMITTRANSACTIONS_FAILED, ex);
            }
        }



        #endregion

        #region Private Methods

        private void CheckoutFundsGenerating(CoreData.Data.ShoppingCartCheckOut cartCheckOut, commonData.ZeoContext context)
        {
            CommitChecks(cartCheckOut.transactions, context);

            CommitReceiveMoneys(cartCheckOut.transactions, context);
        }

        private void CheckoutFundsDepleting(CoreData.Data.ShoppingCartCheckOut cartCheckout, commonData.ZeoContext context)
        {
            CommitBillPays(cartCheckout.transactions, context);

            CommitSendMoneys(cartCheckout.transactions, context);

            CommitMoneyOrder(cartCheckout.transactions, context);
        }

        private bool IsCashOverCounterWithDraw(CoreData.Data.ShoppingCartCheckOut cartCheckOut, Helper.ShoppingCartCheckoutStatus cartCheckOutStatus, commonData.ZeoContext context)
        {
            var withdraw = cartCheckOut.transactions.Where(x => x.ProductId == Helper.Product.Fund && x.TransactionType == (int)Helper.FundType.Debit && x.State == Helper.TransactionStates.Authorized).FirstOrDefault();

            if (withdraw != null && withdraw.Amount > 0)
            {
                CommitFund(withdraw.TransactionId, context);

                if (cartCheckOut.IsCashOverCounter && cartCheckOutStatus != Helper.ShoppingCartCheckoutStatus.CashCollected)
                {
                    //CashOut & Update shopping cart status as 'CashOverCounter'
                    ShoppingCartService.CashOutAndUpdateCartStatus(cartCheckOut.CartId, withdraw.Amount, context.CustomerSessionId, (int)Helper.ShoppingCartCheckoutStatus.CashOverCounter, context.TimeZone, context);
                    return true;
                }
            }
            return false;

        }

        private void CashOutAndUpdateReferral(Helper.ShoppingCartCheckoutStatus cartStatus, decimal cashToCustomer, long cartId, commonData.ZeoContext context)
        {
            // 1. Cash Out, if the cash to customer amount > 0
            // 2. Add Or Update Customer Fee adjustment(Referral Referee Promotions) 
            // 3. Update shopping cart status as 'Completed' and State as 'Closed'

            ShoppingCartService.CashOutAndUpdateReferral(context.CustomerSessionId, cashToCustomer, (int)cartStatus, cartId, context.TimeZone, context.IsReferral, context);
        }

        private void CommitChecks(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {

            List<CoreData.Data.ShoppingCartTransaction> checks = cartTransactions.Where(i => i.State == Helper.TransactionStates.Authorized && i.ProductId == Helper.Product.ProcessCheck).ToList();

            foreach (CoreData.Data.ShoppingCartTransaction check in checks)
            {
                if (CPService == null)
                    CPService = new CPServiceImpl();
                CheckProductsLimits(check.Amount, Helper.TransactionType.ProcessCheck, context);
                CPService.Commit(check.TransactionId, context);
            }
        }

        private void CommitBillPays(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {
            List<CoreData.Data.ShoppingCartTransaction> billPays = cartTransactions.Where(i => i.State == Helper.TransactionStates.Authorized && i.ProductId == Helper.Product.BillPayment).ToList();

            foreach (CoreData.Data.ShoppingCartTransaction billPay in billPays)
            {
                CheckProductsLimits(billPay.Amount, Helper.TransactionType.BillPayment, context);
                BillPayService = new BillPayServiceImpl();
                BillPayService.Commit(billPay.TransactionId, context);
            }
        }

        private void CommitMoneyOrder(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {
            CoreData.Data.ShoppingCartTransaction moneyOrder = cartTransactions.Where(i => i.State == Helper.TransactionStates.Processing && i.ProductId == Helper.Product.MoneyOrder && !String.IsNullOrEmpty(i.CheckNumber)).OrderBy(x => x.TransactionId).FirstOrDefault();

            if (moneyOrder != null)
            {
                MoneyOrderservice = new MoneyOrderServicesImpl();
                MoneyOrderservice.Commit(moneyOrder.TransactionId, context);
            }
        }

        private bool IsMoneyOrderAuthorized(List<CoreData.Data.ShoppingCartTransaction> transactions, commonData.ZeoContext context)
        {
            //Scanning the MO if the checknumber is not available even if the trx state is processing.This scenario will happen only when we close the browser while scanning
            if (transactions.Any(x => x.State == Helper.TransactionStates.Processing && x.ProductId == Helper.Product.MoneyOrder && String.IsNullOrEmpty(x.CheckNumber)))            
                return true;   

            CoreData.Data.ShoppingCartTransaction moneyOrder = transactions.FirstOrDefault(x => x.State == Helper.TransactionStates.Authorized && x.ProductId == Helper.Product.MoneyOrder);     
            
            if (moneyOrder != null)
            {
                CheckProductsLimits(moneyOrder.Amount, Helper.TransactionType.MoneyOrder, context);
                MoneyOrderservice = new MoneyOrderServicesImpl();
                MoneyOrderservice.UpdateMoneyOrderStatus(moneyOrder.TransactionId, (int)Helper.TransactionStates.Processing, context);
                return true;
            }

            return false;
        }

        private void CommitSendMoneys(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {
            List<CoreData.Data.ShoppingCartTransaction> moneyTransfers = cartTransactions.Where(i => i.State == Helper.TransactionStates.Authorized && i.ProductId == Helper.Product.MoneyTransfer && i.TransactionType == (int)Helper.MoneyTransferType.Send && i.TransactionSubType == null).ToList();

            foreach (CoreData.Data.ShoppingCartTransaction sendMoney in moneyTransfers)
            {
                CheckProductsLimits(sendMoney.Amount, Helper.TransactionType.MoneyTransfer, context);
                if (MoneyTransferService == null)
                    MoneyTransferService = new MoneyTransferEngineImpl();
                MoneyTransferService.Commit(sendMoney.TransactionId, context);
            }

            moneyTransfers = cartTransactions.Where(i => i.State == Helper.TransactionStates.Authorized && i.ProductId == Helper.Product.MoneyTransfer && i.TransactionType == (int)Helper.MoneyTransferType.Send && i.TransactionSubType == (int)Helper.TransactionSubType.Modify).ToList();

            foreach (CoreData.Data.ShoppingCartTransaction modify in moneyTransfers)
            {
                CheckProductsLimits(modify.Amount, Helper.TransactionType.MoneyTransfer, context);
                if (MoneyTransferService == null)
                    MoneyTransferService = new MoneyTransferEngineImpl();
                MoneyTransferService.Modify(modify.TransactionId, context);
            }
        }

        private void CommitReceiveMoneys(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {
            var receiveMoneys = cartTransactions.Where(i => i.State == Helper.TransactionStates.Authorized && i.ProductId == Helper.Product.MoneyTransfer && (i.TransactionType == (int)Helper.MoneyTransferType.Receive || i.TransactionType == (int)Helper.MoneyTransferType.Refund)).ToList();

            if (receiveMoneys != null)
            {
                foreach (CoreData.Data.ShoppingCartTransaction receiveMoney in receiveMoneys)
                {
                    if (MoneyTransferService == null)
                        MoneyTransferService = new MoneyTransferEngineImpl();
                    MoneyTransferService.Commit(receiveMoney.TransactionId, context);
                }
            }
        }

        private void CommitFunds(List<CoreData.Data.ShoppingCartTransaction> cartTransactions, commonData.ZeoContext context)
        {
            // Commit funnd for Load, Activate, AddOn card

            var funds = cartTransactions.Where(x => x.TransactionType != (int)Helper.FundType.Debit && x.State == Helper.TransactionStates.Authorized && x.ProductId == Helper.Product.Fund).ToList();

            foreach (CoreData.Data.ShoppingCartTransaction fund in funds)
            {
                CommitFund(fund.TransactionId, context);
            }
        }

        private void CommitFund(long transactionId, commonData.ZeoContext context)
        {
            if (FundService == null)
                FundService = new FundsEngineImpl();

            FundService.Commit(transactionId, context);
        }

        private List<GPRCard> GetGPRCards(CoreData.Data.ShoppingCart shoppingCart)
        {
            List<GPRCard> gPRCards = new List<GPRCard>();

            GPRCard gPRCard;

            foreach (var item in shoppingCart.Funds)
            {
                gPRCard = new GPRCard();

                gPRCard.CardNumber = item.CardNumber;
                gPRCard.Id = item.Id.ToString();
                gPRCard.TransactionId = item.Id;
                gPRCard.ItemType = item.FundsType.ToString();
                gPRCard.Status = item.State.ToString();
                gPRCard.StatusDescription = item.Description;

                switch (item.FundsType)
                {
                    case Helper.FundType.Credit:
                        gPRCard.LoadAmount = item.Amount;
                        gPRCard.LoadFee = item.BaseFee - item.DiscountApplied;
                        gPRCard.BaseFee = item.BaseFee;
                        gPRCard.DiscountApplied = item.DiscountApplied;
                        gPRCard.DiscountName = item.DiscountName;
                        gPRCard.NetFee = item.BaseFee - item.DiscountApplied;
                        break;
                    case Helper.FundType.Debit:
                        gPRCard.WithdrawAmount = item.Amount;
                        gPRCard.WithdrawFee = item.Fee;
                        gPRCard.BaseFee = item.BaseFee;
                        gPRCard.DiscountApplied = item.DiscountApplied;
                        gPRCard.DiscountName = item.DiscountName;
                        gPRCard.NetFee = item.BaseFee - item.DiscountApplied;
                        break;
                    case Helper.FundType.AddOnCard:
                        gPRCard.InitialLoadAmount = item.Amount;
                        gPRCard.ActivationFee = item.Fee;
                        //gPRCard.AddOnCustomerName = item.AddOnCustomerName;
                        break;
                    default:
                        gPRCard.InitialLoadAmount = item.Amount;
                        gPRCard.ActivationFee = item.Fee;
                        break;
                }

                gPRCards.Add(gPRCard);

            }
            return gPRCards;
        }

        private void CheckProductsLimits(decimal amount, Helper.TransactionType transactionType, commonData.ZeoContext context)
        {
            string productCode = GetProductCode(transactionType);  

            context.ShouldIncludeShoppingCartItems = false;

            ComplianceService = new ComplianceServiceImpl();

            Limit limit = ComplianceService.GetTransactionLimit(transactionType, context);

            if (amount > limit.PerTransactionMaximum)
            {
                throw new BizComplianceLimitException(productCode, BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, (decimal)limit.PerTransactionMaximum);
            }
        }

        private string GetProductCode(Helper.TransactionType transactionType)
        {
            Helper.ProductCode productCode;
            switch (transactionType)
            {
                case Helper.TransactionType.BillPayment:
                    productCode = Helper.ProductCode.BillPay;
                    break;
                case Helper.TransactionType.MoneyOrder:
                    productCode = Helper.ProductCode.MoneyOrder;
                    break;
                case Helper.TransactionType.MoneyTransfer:
                    productCode = Helper.ProductCode.MoneyTransfer;
                    break;
                case Helper.TransactionType.Fund:
                    productCode = Helper.ProductCode.Funds;
                    break;
                default:
                    productCode = Helper.ProductCode.Compliance;
                    break;
            }
            return ((int)productCode).ToString();
        }

        private void Preflush(decimal cashToCustomer, CoreData.Data.ShoppingCartCheckOut cartCheckout, Helper.ShoppingCartCheckoutStatus cartCheckOutStatus, commonData.ZeoContext context)
        {
            //Get the transaction count. 
            //As "CashIn" and "Cashout" transactions are not counted as these two transaction's status will always be "Committed".
            //This check is applied for "Fund/GPR" transaction as this will call "CheckOut" method multiple times in case of withdrawal.
            //Modified for Defect - "For Committed trx Preflush is called". So added a check for "Authorized" state.
            int trxCount = cartCheckout.transactions.Where(t => t.ProductId != Helper.Product.Cash && t.State == Helper.TransactionStates.Authorized).Count();
            //bool canCallPreFlush = false;


            //canCallPreFlush = (cartCheckOutStatus == Helper.ShoppingCartCheckoutStatus.CashOverCounter || cartCheckOutStatus == Helper.ShoppingCartCheckoutStatus.CashCollected) && cartCheckout.transactions.Where(x => x.ProductId == Helper.Product.Fund && x.TransactionSubType == (int)Helper.FundType.Debit && x.State == Helper.TransactionStates.Committed).Any();

            //if (!canCallPreFlush)
            //{
                //This check is to handle multiple preflush calls when more than one money order are there is shopping cart.
               bool canCallPreFlush = cartCheckout.transactions.Where(x => x.State == Helper.TransactionStates.Processing
                                                          && x.ProductId == Helper.Product.MoneyOrder).Any();
            //}


            if (trxCount > 0 && !canCallPreFlush)
            {
                IFlushService flushSvc = new FlushServiceImpl();
                flushSvc.PreFlush(cashToCustomer, context);
            }
        }


        #endregion


    }

}
