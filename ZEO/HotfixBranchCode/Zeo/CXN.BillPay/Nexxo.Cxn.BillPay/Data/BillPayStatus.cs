namespace MGI.Cxn.BillPay.Data
{
    public enum BillPayStatus
    {
        Unknown = 0,
        Pending = 1,
        Authorized = 2,
        AuthorizationFailed = 3,
        Committed = 4,
        Failed = 5,
        Canceled = 6,
    }
}