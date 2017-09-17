using TCF.Channel.Zeo.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Models
{
    public class ShoppingCartDetail : BaseModel
    {
        public long Id { get; set; }
        public decimal GeneratedAmount { get; set; }
        public decimal GeneratedFee { get; set; }
        public decimal GeneratedTotal { get; set; }
        public decimal DepletedAmount { get; set; }
        public decimal DepletedFee { get; set; }
        public decimal DepletedTotal { get; set; }
        public decimal TotalFee { get; set; }
        public decimal DueToCustomer { get; set; }
        //[Range(typeof(decimal), "0", "10000", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "LoadAmountLimit")]
        [MinimumInitialLoadAmount("MinimumLoadAmount", "IsCardActivationTrx", "ShoppingCartDetailInitialMinLoadAmt", "ShoppingCardDetailMinimumLoadAmount", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ShoppingCartDetailInitialMinLoadAmt")]
        public decimal LoadToCard { get; set; }
        public decimal CashToCustomer { get; set; }
        public decimal CashCollected { get; set; }
        public bool CardHolder { get; set; }
        public decimal CardBalance { get; set; }
        public decimal LoadToCardToDisplay { get; set; }
        public decimal LoadFee { get; set; }
        public decimal WithdrawFee { get; set; }
        public decimal PreviousCashCollected { get; set; }
        [Range(typeof(decimal), "0", "10000", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "WithdrawAmountLimit")]
        public decimal WithdrawFromCard { get; set; }
        public decimal MinimumLoadAmount { get; set; }
        public bool IsCardActivationTrx { get; set; }
        public decimal CashRecieved { get; set; }
        private List<ShoppingCartItem> _items = new List<ShoppingCartItem>();
        public List<ShoppingCartItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                value = _items;
            }
        }
        public bool IsReferral { get; set; }
        public bool IsReferalSectionEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checks"></param>
        public void CheckDetail(List<ZeoClient.Check> checks)
        {
            if (checks != null)
            {
                _items.AddRange(checks.Select
                    (check => new ShoppingCartItem()
                    {
                        //Product = "process-check",
                        Product = ProductType.Checks.ToString(),
                        TxnId = check.Id,
                        Amount = Convert.ToDecimal(check.Amount.ToString("0.00")),
                        Fee = check.Fee,
                        NetAmount = check.Amount - check.Fee,
                        Status = check.Status,
                        Description = check.StatusDescription,
                        Details = check.StatusMessage,
                        TxnType = ZeoClient.HelperFundType.Credit.ToString(),
                        SummaryTitle = "Process Check",
                        BaseFee = check.BaseFee,
                        DiscountApplied = check.DiscountApplied,
                        NetFee = check.Fee,
                        DmsStatusMessage = check.DmsStatusMessage
                    }
                    ).ToList().OrderBy(xy => xy.TxnId)
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bills"></param>
        public void BillDetail(List<ZeoClient.BillPayTransaction> bills)
        {
            if (bills != null)
            {
                _items.AddRange(bills.Select
                    (bill => new ShoppingCartItem()
                    {
                        //Product = "bill-payment",
                        Product = ProductType.BillPay.ToString(),
                        TxnId = bill.Id,
                        Amount = bill.Amount,
                        Fee = bill.Fee,
                        NetAmount = bill.Amount + bill.Fee,
                        Status = bill.Status,
                        Description = string.Format("{0} A/C {1}", bill.BillerName, bill.AccountNumber),
                        Details = bill.StatusMessage,
                        TxnType = ZeoClient.HelperFundType.Debit.ToString(),
                        SummaryTitle = "Bill Payment"
                    }).ToList().OrderBy(xy => xy.TxnId)
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyTransfers"></param>
        public void MoneyTransferDetail(List<ZeoClient.MoneyTransfer> moneyTransfers)
        {
            if (moneyTransfers != null)
            {
                _items.AddRange(moneyTransfers.Select
                    (moneytransfer => new ShoppingCartItem()
                    {
                        Product = GetProduct(moneytransfer),

                        TxnId = moneytransfer.Id,
                        Amount = moneytransfer.Amount,
                        Fee = moneytransfer.Fee,

                        NetAmount = GetNetAmount(moneytransfer),

                        Status = moneytransfer.Status,

                        Description = GetDescription(moneytransfer),

                        Details = moneytransfer.StatusMessage,

                        TxnType = moneytransfer.TransferType == (int)ZeoClient.HelperMoneyTransferType.Send &&
                        (string.IsNullOrEmpty(moneytransfer.TransactionSubType) ||
                        moneytransfer.TransactionSubType == ((int)Helper.TransactionSubType.Modify).ToString()) ? ZeoClient.HelperFundType.Debit.ToString() : ZeoClient.HelperFundType.Credit.ToString(),

                        SummaryTitle = GetSummaryTitle(moneytransfer),

                        TxnSubType = moneytransfer.TransactionSubType,
                    }).ToList().OrderBy(xy => xy.TxnId)
                );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funds"></param>
        public void FundsDetail(List<ZeoClient.GPRCard> funds)
        {
            if (funds != null)
            {
                //PREPAID_CARD_LOAD
                _items.AddRange(funds.Where(x => x.ItemType == ZeoClient.HelperFundType.Credit.ToString()).Select
                    (gprcard => new ShoppingCartItem()
                    {
                        //Product = "gpr-load",
                        Product = ProductType.GPRLoad.ToString(),
                        TxnId = long.Parse(gprcard.Id),
                        Amount = gprcard.LoadAmount,
                        Fee = gprcard.LoadFee,
                        NetAmount = gprcard.LoadAmount,
                        Status = gprcard.Status,
                        TxnType = ZeoClient.HelperFundType.Credit.ToString(),
                        SummaryTitle = "Prepaid Card",
                        Description = string.Format("Load to ****{0}", ShoppingCartHelper.getAcctLast4Digits(HttpContext.Current.Session["CardNo"] != null ? Convert.ToString(HttpContext.Current.Session["CardNo"]) : string.Empty))
                    }).ToList().OrderBy(xy => xy.TxnId)
                    );

                //PREPAID_CARD_WITHDRAW
                _items.AddRange(funds.Where(x => x.ItemType == ZeoClient.HelperFundType.Debit.ToString()).Select
                    (gprcard => new ShoppingCartItem()
                    {
                        //Product = "gpr-withdraw",
                        Product = ProductType.GPRWithdraw.ToString(),
                        TxnId = long.Parse(gprcard.Id),
                        Amount = gprcard.WithdrawAmount,
                        Fee = gprcard.WithdrawFee,
                        NetAmount = gprcard.WithdrawAmount,
                        Status = gprcard.Status,
                        TxnType = ZeoClient.HelperFundType.Debit.ToString(),
                        SummaryTitle = "Prepaid Card",
                        Description = string.Format("Cash to Customer from ****{0}", ShoppingCartHelper.getAcctLast4Digits(Convert.ToString(gprcard.CardNumber)))
                    }).ToList().OrderBy(xy => xy.TxnId));

                //PREPAID_CARD_ACTIVATE
                _items.AddRange(funds.Where(x => x.ItemType == ZeoClient.HelperFundType.Activation.ToString()).Select
                    (gprcard => new ShoppingCartItem()
                    {
                        //Product = "gpr-activate",
                        Product = ProductType.GPRActivation.ToString(),
                        TxnId = long.Parse(gprcard.Id),
                        Amount = gprcard.InitialLoadAmount,
                        Fee = gprcard.ActivationFee,
                        NetAmount = gprcard.InitialLoadAmount,
                        Status = gprcard.Status,
                        TxnType = ZeoClient.HelperFundType.Credit.ToString(),
                        SummaryTitle = "Prepaid Card",
                        Description = string.Format("Activate ****{0}", ShoppingCartHelper.getAcctLast4Digits(Convert.ToString(gprcard.CardNumber)))

                    }).ToList().OrderBy(xy => xy.TxnId)
                );

                _items.AddRange(funds.Where(x => x.ItemType == ZeoClient.HelperFundType.AddOnCard.ToString()).Select
                    (gprcard => new ShoppingCartItem()
                    {
                        Product = ProductType.AddOnCard.ToString(),
                        TxnId = long.Parse(gprcard.Id),
                        Amount = gprcard.InitialLoadAmount,
                        Fee = gprcard.ActivationFee,
                        NetAmount = gprcard.InitialLoadAmount,
                        Status = gprcard.Status,
                        TxnType = ZeoClient.HelperFundType.AddOnCard.ToString(),
                        SummaryTitle = "Prepaid Card",
                        Description = string.Format("Companion Card Order - [{0}]", gprcard.AddOnCustomerName)

                    }).ToList().OrderBy(xy => xy.TxnId)
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartCash"></param>
        public void CashDetail(List<ZeoClient.CartCash> cartCash)
        {
            if (cartCash != null)
            {
                _items.AddRange(cartCash.Where(x => x.CashType == ZeoClient.HelperCashType.CashIn).Select
                    (cash => new ShoppingCartItem()
                    {
                        Product = ProductType.CashIn.ToString(),
                        TxnId = long.Parse(cash.Id),
                        Amount = cash.Amount,
                        Fee = 0,
                        NetAmount = cash.Amount,
                        SummaryTitle = "Cash In",
                        Status = cash.Status
                    }).ToList().OrderBy(xy => xy.TxnId)
                );

                _items.AddRange(cartCash.Where(y => y.CashType == ZeoClient.HelperCashType.CashIn).Select
                    (cash => new ShoppingCartItem()
                    {
                        Product = ProductType.CashOut.ToString(),
                        TxnId = long.Parse(cash.Id),
                        Amount = cash.Amount,
                        Fee = 0,
                        NetAmount = cash.Amount,
                        SummaryTitle = "Cash Out",
                        Status = cash.Status
                    }).ToList().OrderBy(xy => xy.TxnId)
                );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyOrders"></param>
        public void MoneyOrderDetail(List<ZeoClient.MoneyOrder> moneyOrders)
        {
            if (moneyOrders != null)
            {
                _items.AddRange(moneyOrders.Select
                (moneyOrder => new ShoppingCartItem()
                {
                    Product = ProductType.MoneyOrder.ToString(),
                    TxnId = moneyOrder.Id,
                    Amount = Convert.ToDecimal(moneyOrder.Amount.ToString("0.00")),
                    Fee = moneyOrder.Fee,
                    NetAmount = moneyOrder.Amount + moneyOrder.Fee,
                    Status = moneyOrder.State.ToString(),
                    Description = "Money Order",
                    TxnType = ZeoClient.HelperFundType.Debit.ToString(),
                    SummaryTitle = "Money Order",
                    BaseFee = moneyOrder.BaseFee,
                    DiscountApplied = moneyOrder.DiscountApplied,
                    NetFee = moneyOrder.BaseFee - Math.Abs(moneyOrder.DiscountApplied)
                }
                ).ToList().OrderBy(xy => xy.TxnId)
                );
            }
        }

        private decimal GetNetAmount(ZeoClient.MoneyTransfer moneyTransfer)
        {
            switch (moneyTransfer.TransferType)
            {
                case (int)ZeoClient.HelperMoneyTransferType.Send:
                case (int)ZeoClient.HelperMoneyTransferType.Refund: return moneyTransfer.Amount + moneyTransfer.Fee;

                case (int)ZeoClient.HelperMoneyTransferType.Receive: return moneyTransfer.Amount - moneyTransfer.Fee;

                default: return 0;
            }
        }

        private string GetProduct(ZeoClient.MoneyTransfer moneyTransfer)
        {
            switch (moneyTransfer.TransferType)
            {
                case (int)ZeoClient.HelperMoneyTransferType.Send: return ProductType.SendMoney.ToString();

                case (int)ZeoClient.HelperMoneyTransferType.Receive: return ProductType.ReceiveMoney.ToString();

                case (int)ZeoClient.HelperMoneyTransferType.Refund: return ProductType.Refund.ToString();

                default: return ProductType.SendMoney.ToString();
            }
        }

        private string GetDescription(ZeoClient.MoneyTransfer moneyTransfer)
        {
            switch (moneyTransfer.TransferType)
            {
                case (int)ZeoClient.HelperMoneyTransferType.Send:
                    return string.Format("{3} {0} {1} - {2}", "Send Money to", moneyTransfer.ReceiverFirstName + " " + moneyTransfer.ReceiverLastName, moneyTransfer.DestinationCountry, GetTransactionSubType(moneyTransfer));

                case (int)ZeoClient.HelperMoneyTransferType.Receive:
                    return string.Format("{3} {0} {1} - {2}", "Receive Money from", moneyTransfer.SenderFirstName + " " + moneyTransfer.SenderLastName, moneyTransfer.SourceCountry, GetTransactionSubType(moneyTransfer));

                case (int)ZeoClient.HelperMoneyTransferType.Refund:
                    return string.Format("{3} {0} {1} - {2}", "Refund Money to", moneyTransfer.SenderFirstName + " " + moneyTransfer.SenderLastName, moneyTransfer.SourceCountry, GetTransactionSubType(moneyTransfer));

                default: return ProductType.SendMoney.ToString();
            }
        }

        private string GetTransactionSubType(ZeoClient.MoneyTransfer moneyTransfer)
        {
            string transactionSubType = "";

            if (moneyTransfer.TransactionSubType == ((int)Helper.TransactionSubType.Modify).ToString())
            {
                transactionSubType = "Modified";
            }
            else if (moneyTransfer.TransactionSubType == ((int)Helper.TransactionSubType.Cancel).ToString())
            {
                transactionSubType = "Cancelled";
            }
            else if (moneyTransfer.TransactionSubType == ((int)Helper.TransactionSubType.Refund).ToString())
            {
                transactionSubType = "Refunded";
            }

            return transactionSubType;
        }

        private string GetSummaryTitle(ZeoClient.MoneyTransfer moneyTransfer)
        {
            switch (moneyTransfer.TransferType)
            {
                case (int)ZeoClient.HelperMoneyTransferType.Send:
                    return "Send Money";

                case (int)ZeoClient.HelperMoneyTransferType.Receive:
                    return "Receive Money";

                case (int)ZeoClient.HelperMoneyTransferType.Refund:
                    return "Refund Money";

                default: return ProductType.SendMoney.ToString();
            }
        }
    }
}
