using TCF.Zeo.Common.Util;
using System.Collections.Generic;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Biz.Common.Impl;
using System.Configuration;
using TCF.Zeo.Biz.Common;

namespace TCF.Zeo.Biz.MoneyOrder.Impl
{
    public class MoneyOrderCheckPrintTemplateRepo
    {
        public IPrintTemplate PrintTemplateRepo;

        public string GetCheckPrintTemplate(string channelPartnerName, Helper.TransactionTypes transactionType, Helper.ProviderId provider, string state)
        {
            string checkPrintFileComplete = string.Format("{0}.{1}.{2}.{3}.CheckPrint.txt", channelPartnerName, transactionType.ToString(), provider.ToString(), state);

            string checkPrintFileWithProcessor = string.Format("{0}.{1}.{2}.CheckPrint.txt", channelPartnerName, transactionType.ToString(), provider.ToString());

            string checkPrintFileBase = string.Format("{0}.{1}.CheckPrint.txt", channelPartnerName, transactionType.ToString());

            List<string> checkPrintfiles = new List<string>() { checkPrintFileComplete, checkPrintFileWithProcessor, checkPrintFileBase };

            return GetCheckPrintFile(checkPrintfiles);
        }

        public string GetMoneyOrderDiagnosticsTemplate()
        {
            List<string> moneyOrderTemplates = new List<string>() { "MoneyOrder.CheckPrint.Test.txt" };

            return GetCheckPrintFile(moneyOrderTemplates);
        }

        private string GetCheckPrintFile(List<string> checkPrintsfile)
        {
            PrintTemplateRepo = new PrintTemplate();

            string checkPrintfile = PrintTemplateRepo.GetPrintTemplate(checkPrintsfile);

            if (string.IsNullOrWhiteSpace(checkPrintfile))
                throw new MoneyOrderException(MoneyOrderException.CHECKPRINT_TEMPLATE_NOT_FOUND);

            return checkPrintfile;
        }
    }
}
