using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.Fund.FIS.ISO8583;
using System.Diagnostics;
using System.Net;
using MGI.Cxn.Fund.Data;

namespace MGI.Cxn.Fund.FIS.Impl
{
    public class IO
    {
        private ISOWebSvc.SO8583WebSvcSoapClient _client;

        public long Authenticate(string cardNumber, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            Random rand = new Random(123456789);

            string transactionId = (rand.Next() * 100).ToString().PadLeft(12, '0');
            Terminal terminal = new Terminal();

            Dictionary<string, string> _fisVars = new Dictionary<string, string>();
            _fisVars.Add("a", "authenticate");

            FISBalanceInquiry isoMessage = new FISBalanceInquiry(_fisVars, DateTime.Now, transactionId, terminal, string.Empty, cardNumber);
            isoMessage.Create();

            string Response = "1|2;3|4;5|6";
            ISOWebSvc.ISO8583Result isoResult = new ISOWebSvc.ISO8583Result();
            Trace.WriteLine(string.Format("Sending message. Transaction #{0}", transactionId), this.GetType().Name);

            _client = new ISOWebSvc.SO8583WebSvcSoapClient();

            try
            {
                int i = _client.Ping();
                isoResult = _client.ExchangeISOMessage(terminal.Id, "appid1", 123456, isoMessage.MessageType, isoMessage.ToString(), "TWK", out Response);
            }
            catch (Exception ex) 
            { }

            long result = 0;
            processorResult = new ProcessorResult();

            if (isoResult.ErrorCode == 0)
            {
                FISISOResponseMessage isoResponse = new FISISOResponseMessage(Response);
                Trace.WriteLine(string.Format("FIS returned ResponseCode {0}", isoResponse.ResponseCode), this.GetType().Name);
            }
            else
            {
                Trace.WriteLine("ISO 8583 Transmission Error in ValidateCardAndPIN(): " + isoResult.ErrorMessage, this.GetType().Name);
                //result = new ExoNexxoPurseResult(ExoNexxoPurseErrorCodes.GeneralError, isoResult.ErrorMessage);
            }

            return result;
        }

        public decimal GetBalance(long accountId, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            decimal result = 0;
            Random rand = new Random(123456789);
            string transactionId = (rand.Next() * 100).ToString().PadLeft(12, '0');
            Terminal terminal = new Terminal();

            //_kiosk = new KioskProfile(_NexxoDB, Request.KioskId, Request.LocationId);
            Dictionary<string, string> _fisVars = new Dictionary<string, string>();
            _fisVars.Add("a", "getbalance");

            FISBalanceInquiry isoMessage = new FISBalanceInquiry(_fisVars, DateTime.Now, transactionId, terminal, string.Empty, "9999999999999999");
            isoMessage.Create();

            string Response = "1|2;3|4;5|6";
            ISOWebSvc.ISO8583Result isoResult = new ISOWebSvc.ISO8583Result();
            Trace.WriteLine(string.Format("Sending message. Transaction #{0}", transactionId), this.GetType().Name);

            _client = new ISOWebSvc.SO8583WebSvcSoapClient();

            try
            {
                isoResult = _client.ExchangeISOMessage(terminal.Id, "appid1", 123456, isoMessage.MessageType, isoMessage.ToString(), "TWK", out Response);
            }
            catch { }

            processorResult = new ProcessorResult();
            if (isoResult.ErrorCode == 0)
            {
                FISISOResponseMessage isoResponse = new FISISOResponseMessage(Response);
                Trace.WriteLine(string.Format("FIS returned ResponseCode {0}", isoResponse.ResponseCode), this.GetType().Name);
            }
            else
            {
                Trace.WriteLine("ISO 8583 Transmission Error in CheckAccountBalance(): " + isoResult.ErrorMessage, this.GetType().Name);
            }

            return result;
        }

        public long Load(long accountId, FundRequest fundRequest, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            long result = 0;
            Random rand = new Random(123456789);
            string transactionId = (rand.Next() * 100).ToString().PadLeft(12, '0');
            Terminal terminal = new Terminal();

            string ConfirmationNum = string.Empty;
            string CaptureDate = string.Empty;

            Dictionary<string, string> _fisVars = new Dictionary<string, string>();
            _fisVars.Add("a", "load");

            BaseFISISOMessage isoMessage = new BaseFISISOMessage(_fisVars, fundRequest.Amount, DateTime.Now, transactionId, terminal, "9999999999999999");
            isoMessage.Create();

            string response = "1|2;3|4;5|6";
            ISOWebSvc.ISO8583Result isoResult = new ISOWebSvc.ISO8583Result();
            _client = new ISOWebSvc.SO8583WebSvcSoapClient();

            try
            {
                isoResult = _client.ExchangeISOMessage(terminal.Id, "appid1", 123456, isoMessage.MessageType, isoMessage.ToString(), "TWK", out response);
            }
            catch { }

            processorResult = new ProcessorResult();
            if (isoResult.ErrorCode == 0)
            {
                FISISOResponseMessage isoResponse = new FISISOResponseMessage(response);
            }
            else
            {
                Trace.WriteLine("ISO 8583 Transmission Error in Credit(): " + isoResult.ErrorMessage, this.GetType().Name);
            }

            return result;
        }

        public long Withdraw(long accountId, FundRequest fundRequest, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            long result = 0;
            Random rand = new Random(123456789);
            string transactionId = (rand.Next() * 100).ToString().PadLeft(12, '0');
            Terminal terminal = new Terminal();

            string ConfirmationNum = string.Empty;
            string CaptureDate = string.Empty;

            Dictionary<string, string> _fisVars = new Dictionary<string, string>();
            _fisVars.Add("a", "withdraw");

            BaseFISISOMessage isoMessage = new BaseFISISOMessage(_fisVars, fundRequest.Amount, DateTime.Now, transactionId, terminal, "9999999999999999");
            isoMessage.Create();

            string response = "1|2;3|4;5|6";
            ISOWebSvc.ISO8583Result isoResult = new ISOWebSvc.ISO8583Result();
            _client = new ISOWebSvc.SO8583WebSvcSoapClient();

            try
            {
                isoResult = _client.ExchangeISOMessage(terminal.Id, "AppId1", 123456, isoMessage.MessageType, isoMessage.ToString(), "TWK", out response);
            }
            catch { }

            processorResult = new ProcessorResult();
            if (isoResult.ErrorCode == 0)
            {
                FISISOResponseMessage isoResponse = new FISISOResponseMessage(response);
            }
            else
            {
                Trace.WriteLine("ISO 8583 Transmission Error in Debit(): " + isoResult.ErrorMessage, this.GetType().Name);
            }

            return result;
        }
    }
}
