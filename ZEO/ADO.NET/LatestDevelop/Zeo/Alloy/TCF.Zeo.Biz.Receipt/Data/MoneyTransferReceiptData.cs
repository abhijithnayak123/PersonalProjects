using System;

namespace TCF.Zeo.Biz.Receipt.Data
{
    public class MoneyTransferReceiptData : BaseReceiptData
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderCity { get; set; }
        public string SenderState { get; set; }
        public string SenderZip { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderMobileNumber { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverZip { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverDOB { get; set; }
        public string ReceiverOccupation { get; set; }
        public string DestinationCountryCode { get; set; }
        public string PayoutState { get; set; }
        public decimal TransferAmount { get; set; }
        public string OriginatingCountry { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TransferFee { get; set; }
        public decimal TransferTaxes { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal DstnTransferAmount { get; set; }
        public string DstnCurrencyCode { get; set; }
        public string MTCN { get; set; }
        public string GCNumber { get; set; }
        public string CardPoints { get; set; }
        public string PayoutCity { get; set; }
        public decimal EstTransferAmount { get; set; }
        public decimal EstOtherFee { get; set; }
        public decimal EstTotalToReceiver { get; set; }
        public string DeliveryOption { get; set; }
        public string DeliveryOptionDesc { get; set; }
        public string DeliveryServiceDesc { get; set; }
        public string TransalatedDeliveryServiceName { get; set; }
        public DateTime DTAvailableForPickup { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal TotalToReceiver { get; set; }
        public string AgencyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string PersonalMessage { get; set; }
        public string MessageArea { get; set; }
        public decimal PaySideTaxes { get; set; }
        public decimal Taxes { get; set; }
    }
}
