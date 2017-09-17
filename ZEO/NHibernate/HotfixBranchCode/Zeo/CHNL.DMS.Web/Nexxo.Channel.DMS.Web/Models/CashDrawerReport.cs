using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MGI.Channel.DMS.Web.Models
{
    public class CashDrawerReport : BaseModel 
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementLocation")]
        public string LocationName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementTellerName")]
        public string TellerName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementReportingDate")]
        public string ReportDate { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementReportingTime")]
        public string ReportTime { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementNetCashInAmount")] 
        public decimal CashIn { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementNetCashOutAmount")] 
        public decimal CashOut { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementNetAmount")]
        public decimal SubTotal { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "CashManagementNetAmount")]
        public decimal SubTotalToDisplay { get; set; }

        public string PrintData { get; set; }

        public void PrepareReportForPrint(string reportTemplate)
        {
            StringBuilder reportdata = new StringBuilder();
            reportdata.Append("{LocationName}|" + this.LocationName);
            reportdata.Append("|{TellerName}|" + this.TellerName);
            reportdata.Append("|{Date}|" + this.ReportDate);
            reportdata.Append("|{Time}|" + this.ReportTime);
            reportdata.Append("|{CashIn}|" + this.CashIn.ToString("$0.00"));
            reportdata.Append("|{CashOut}|" + this.CashOut.ToString("$0.00"));
            reportdata.Append("|{SubTotal}|" + this.SubTotal.ToString("$0.00"));

			reportdata.Append("|{data}|" + reportTemplate);
			this.PrintData = reportdata.ToString();
        }
    }
}