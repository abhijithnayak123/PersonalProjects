using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Logging.Impl;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Check.Data.Exceptions;
using TCF.Zeo.Cxn.Check.TCF.Contract;
using TCF.Zeo.Cxn.Check.TCF.Data;
using TCF.Zeo.Cxn.Check.TCF.Data.Exceptions;

namespace TCF.Zeo.Cxn.Check.TCF.Impl
{
    public class IO : IIO
    {
        private readonly string PRODCODE = "ZEOSVC";
        private readonly string APPL_CD = "ZEO";
        private readonly string CHECK_PROCESSING = "CheckProcessing";
        private readonly string TYPE = "Inquiry";
        private readonly bool IsHardcodedSSOAttr = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodeSSOAttributes"].ToString());
        private readonly string SSOAttributes = ConfigurationManager.AppSettings["SSOAttributes"].ToString();
        NLoggerCommon logwriter = new NLoggerCommon();

        public IO()
        {
            SetCertificatePolicy();
        }

        public TellerMainFrameResponse TellerInquiry(TCFOnusTransaction tcfonusTrx, RCIFCredential credential, ZeoContext context)
        {
            try
            {
                X509Certificate2 certificate = GetRCIFCredentialCertificate(credential);

                TEL7770OperationRequest TEL7770Operation = new TEL7770OperationRequest();
                TELNexxcTranInfoRequest telnexxc_tran_info = new TELNexxcTranInfoRequest
                {
                  //  telnexxc_nexxo_acct = Convert.ToString(context.CustomerId)?.PadLeft(18, '0'), //"1000000019",
                    telnexxc_tran = CHECK_PROCESSING,
                    telnexxc_type = TYPE,
                    telnexxc_acct = Convert.ToString(tcfonusTrx.AccountNumber)?.PadLeft(13, '0'),//"2000029932", 
                    telnexxc_check_no = tcfonusTrx.CheckNumber?.PadLeft(11, '0'),
                    telnexxc_amt = tcfonusTrx.Amount,
                    telnexxc_fee = tcfonusTrx.Fee,
                    telnexxc_tcfcard = string.Empty,
                    telnexxc_appl = APPL_CD,
                  //  telnexxc_zeo_acct = Convert.ToString(00700000000000000183)?.PadLeft(20, '0'),
                    //telnexxc_initial = "A",
                    telnexxc_routing_no = !string.IsNullOrWhiteSpace(tcfonusTrx.RoutingNumber) ? tcfonusTrx.RoutingNumber.PadLeft(9, '0').Substring(0, 9) : string.Empty,
                    telnexxc_mgi_tranid = Convert.ToString(tcfonusTrx.Id)
                };

                Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

                telnexxc_tran_info.telnexxc_teller_bk = ssoAttributes.GetStringValue("BankNum");
                telnexxc_tran_info.telnexxc_teller_br = ssoAttributes.GetStringValue("BranchNum");
                telnexxc_tran_info.telnexxc_teller = ssoAttributes.GetStringValue("TellerNum");
                telnexxc_tran_info.telnexxc_ampm = ssoAttributes.GetStringValue("AmPmInd");
                telnexxc_tran_info.telnexxc_drawer = ssoAttributes.GetStringValue("CashDrawer");
                telnexxc_tran_info.telnexxc_lu = ssoAttributes.GetStringValue("LU");
                telnexxc_tran_info.telnexxc_lawson = ssoAttributes.GetStringValue("LawsonID");

                TEL7770Operation.telnexxc_tran_info = telnexxc_tran_info;
                TellerMainFrameRequest request = new TellerMainFrameRequest();
                request.TEL7770Operation = TEL7770Operation;

                SerializeObjectAndLog<TellerMainFrameRequest>(request);
                TellerMainFrameResponse response = Zeo.Common.Util.AlloyUtil.RESTPostCall<TellerMainFrameResponse>(credential.TellerInquiryUrl, certificate, request);
                SerializeObjectAndLog<TellerMainFrameResponse>(response);

                return response;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleCheckException(ex);
                throw new CheckException(CheckException.TELLERINQUIRY_FAILED, ex);
            }
        }

        #region Private Methods

        private X509Certificate2 GetRCIFCredentialCertificate(RCIFCredential rcifCredentials)
        {
            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            // Open the store.
            certificateStore.Open(OpenFlags.ReadWrite | OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            // Find the certificate with the specified subject.
            X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, rcifCredentials.ThumbPrint, false);
            certificateStore.Close();

            if (certificates.Count < 1)
            {
                throw new CheckException(CheckException.CERIFICATE_NOTFOUND, null);
            }

            return certificates[0];
        }

        private static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            if (error == SslPolicyErrors.None)
            {
                return true;   // already determined to be valid
            }

            // Thumbprints of the IIB Server Certificates. Once the server cert issue is fixed at IIB - we should remove this code.
            // Made the implementation change to also have an option to read the thumbprint from the config file, just in case if there are multiple certs which we are not aware of.
            string iibCertHash = Convert.ToString(ConfigurationManager.AppSettings["IIBCertThumbprint"]);
            // 1st is NP, 2nd is Prod.
            List<string> iibCertHashList = new List<string>() { "421D0E3675BE934F20AC4B655F06EFE1C8A4BF41" };

            if (!string.IsNullOrWhiteSpace(iibCertHash))
            {
                iibCertHashList.AddRange(iibCertHash.ToUpper().Split('|'));
            }
            return iibCertHashList.Contains(cert.GetCertHashString());
        }

        private void HandleCheckException(Exception ex)
        {
            Exception tcisProviderException = ex as TCFProviderException;
            if (tcisProviderException != null)
            {
                TCFProviderException tcisException = tcisProviderException as TCFProviderException;
                string errorCode = Convert.ToString(tcisException.ProviderErrorCode);
                string errorMsg = Convert.ToString(tcisException.Message);

                throw new TCFProviderException(errorCode, errorMsg, null);
            }
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new TCFProviderException(ProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new TCFProviderException(ProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new TCFProviderException(ProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new TCFProviderException(ProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }

        private Dictionary<string, object> GetSSOAttributes(ZeoContext context)
        {
            Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

            if (!IsHardcodedSSOAttr)
                return context.SSOAttributes;

            List<string> attrs = new List<string>() {"BankNum","BranchNum", "TellerNum",
                    "AmPmInd", "CashDrawer", "LU", "LawsonID", "UserID", "BusinessDate", "MachineName", "DPSBranchID"};

            string[] attributes = SSOAttributes?.Split('|');

            if (attributes != null && attributes.Length > 0)
            {
                foreach (var attr in attributes)
                {
                    string[] keyvalue = attr.Split(':');
                    if (keyvalue.Length > 0 && attrs.Contains(keyvalue[0]))
                    {
                        ssoAttributes.Add(keyvalue[0], keyvalue[1]);
                    }
                }
            }

            return ssoAttributes;
        }

        private void SerializeObjectAndLog<T>(T obj)
        {
            string serializedStr = string.Empty;
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));

            using (var sww = new StringWriter())
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    serializedStr = sww.ToString();
                }
            }

            logwriter.Info(serializedStr);
        }
        #endregion
    }
}
