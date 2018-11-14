using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace TCF.Channel.Zeo.Web.Controllers
{
    [SkipNoDirectAccess]
    public class HelpController : BaseController
    {
        public FileResult AppScreenPDF()
        {
            byte[] bytes;
            try
            {
                var pdfPath = Server.MapPath("~/Content/docs/WhatsNewDoc.pdf");
                bytes = System.IO.File.ReadAllBytes(pdfPath);
            }
            catch (Exception)
            {
                bytes = new byte[1];
            }
            return File(bytes, "application/pdf");
        }

        public ActionResult DisplaySupportInformation()
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = serviceClient.GetSupportInformation(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.SupportInformation> details = response.Result as List<ZeoClient.SupportInformation>;

                List<ZeoClient.KeyValuePair> providerContactDetails = getProviderContactDetails(details.FindAll(x => x.ContactType?.ToLower() != "rbs"));

                var textInfo = new System.Globalization.CultureInfo("en-US").TextInfo;

                ZeoClient.SupportInformation detail = details.Find(x => x.ContactType?.ToLower() == "rbs");

                SupportInformation contactDetails = new SupportInformation
                {
                    EmailId = detail?.EmailId,
                    Phone1 = detail?.Phone1,
                    Phone2 = detail?.Phone2,
                    Name = textInfo.ToTitleCase(detail?.Name?.ToLower() ?? string.Empty),
                    ProviderContactDetails = providerContactDetails
                };

                return PartialView("_SupportInformationDetails", contactDetails);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        public FileResult ZEOReferenceManual()
        {
            byte[] bytes;
            try
            {
                var pdfPath = Server.MapPath("~/Content/docs/ZEOReferenceManualFINAL.pdf");
                bytes = System.IO.File.ReadAllBytes(pdfPath);
            }
            catch (Exception)
            {
                bytes = new byte[1];
            }
            return File(bytes, "application/pdf");
        }

        private List<ZeoClient.KeyValuePair> getProviderContactDetails(List<ZeoClient.SupportInformation> details)
        {
            List<ZeoClient.KeyValuePair> contactDetails = new List<ZeoClient.KeyValuePair>();

            foreach (var detail in details)
            {
                contactDetails.Add(new ZeoClient.KeyValuePair { Key = detail?.Name, Value = detail?.Phone1 });
            }

            return contactDetails;
        }
    }

}
