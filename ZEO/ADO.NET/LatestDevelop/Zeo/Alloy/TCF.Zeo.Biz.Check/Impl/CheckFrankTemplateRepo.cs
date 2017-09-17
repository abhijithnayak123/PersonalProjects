using System;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TCF.Zeo.Biz.Common.Contract;
using System.Configuration;
using TCF.Zeo.Biz.Common.Impl;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Check.Impl
{
    public class CheckFrankTemplateRepo
    {
        public IPrintTemplate PrintTemplateRepo;

        public string GetCheckFrankingTemplate(string partner, Helper.TransactionTypes product, Helper.ProviderId processor, string state)
        {
            string checkPrintFileComplete = string.Format("{0}.{1}.{2}.{3}.CheckFranking.txt", partner, product.ToString(), processor.ToString(), state);

            string checkPrintFileWithProcessor = string.Format("{0}.{1}.{2}.CheckFranking.txt", partner, product.ToString(), processor.ToString());

            string checkPrintFileBase = string.Format("{0}.{1}.CheckFranking.txt", partner, product.ToString());

            List<string> checkPrintfiles = new List<string>() { checkPrintFileComplete, checkPrintFileWithProcessor, checkPrintFileBase };

            return GetCheckPrintFile(checkPrintfiles);
        }


        private string GetCheckPrintFile(List<string> checkPrintsfile)
        {
            PrintTemplateRepo = new PrintTemplate();

            string checkPrintfile = PrintTemplateRepo.GetPrintTemplate(checkPrintsfile);

            if (string.IsNullOrWhiteSpace(checkPrintfile))
                throw new CheckException(CheckException.CHECK_FRANK_TEMPLATE_NOT_FOUND);

            return checkPrintfile;
        }
    }
}
