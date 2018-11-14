using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.Data;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Contract
{
    public interface IIO
    {
        #region Customer
        List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, RCIFCredential credential, ZeoContext context);

        string CreateCustomer(CustomerProfile customer, RCIFCredential credential, ZeoContext context);
        #endregion

        #region Pre/Post Flush
        bool PreFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context);

        void PostFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context);
        #endregion

        #region Final Commits

        bool TellerMainFrameCommit(Transaction transaction, RCIFCredential credential, ZeoContext context);

        Tuple<long, long, long, bool> TellerMiddleTierCommit(CustomerTransactionDetails cart, Transaction transaction, ref string CIF7454TemplateType, RCIFCredential credential, ZeoContext context);

        void RCIFFinalCommit(bool isVisaTrx, string CIF7454TemplateType, Tuple<long, long, long> riskScores, CustomerTransactionDetails cart, Transaction transaction, RCIFCredential credential, ZeoContext context);
        #endregion

        #region Customer Registration Without IIB

        bool EWSScanningCustomerRegistration(CustomerProfile customer, RCIFCredential credential, ZeoContext context);

        string RCIFCustomerRegistration(CustomerProfile customer, RCIFCredential credential, ZeoContext context);

        #endregion
    }
}
