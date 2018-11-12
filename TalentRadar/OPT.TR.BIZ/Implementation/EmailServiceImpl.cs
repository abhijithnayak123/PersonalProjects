using OPT.TalentRadar.BIZ.Contract;
using System;
using System.Net.Mail;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.Common.Util.Util;
using System.IO;

namespace OPT.TalentRadar.BIZ.Implementation
{
    public class EmailServiceImpl : IEmailService
    {
        public bool CandidateMail(Mail mail)
        {
            string subject = "Opteamix Recruiter";
            string body = string.Empty;
            string url = ConfigurationManager.AppSettings.Get("CandidateUrl");
            string path = @"~\Template\" + mail.CandidateStage.ToLower() + ".html";
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", mail.CandidateName);
            body = body.Replace("{link}", "<a href='" + url + mail.Id + "'>Click Here</a>");
            body = body.Replace("{RecruiterName}", mail.RecruiterName);
            body = body.Replace("{PhoneNumber}", mail.RecruiterPhoneNumber);
            mail.Body = body;
            mail.Subject = subject;
            return EmailHelper.SendMail(mail);
        }

        public bool RecruiterMail(Mail mail)
        {
            string subject = "Candidate Response";
            string body = ConfigurationManager.AppSettings.Get("AcceptMail");
            if (!string.IsNullOrWhiteSpace(mail.DeclineReason))
            {
                body = ConfigurationManager.AppSettings.Get("DeclinedMail");
            }
            body = string.Format(body, mail.CandidateName, mail.CandidateStage, mail.DeclineReason);

            mail.Subject = subject;
            mail.Body = body;
            return EmailHelper.SendMail(mail);
        }

    }
}
