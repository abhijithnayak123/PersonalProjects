using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPT.TalentRadar.Common.Data;
using System.Net.Mail;

namespace OPT.TalentRadar.Common.Util.Util
{
   public class EmailHelper
    {
        public static bool SendMail(Mail mail)
        {
            string hostName = ConfigurationManager.AppSettings.Get("EmailHost");
            int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SmtpPort"));
            string userName = ConfigurationManager.AppSettings.Get("UserName");
            string password = ConfigurationManager.AppSettings.Get("Password");

            MailMessage mailMessage = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(hostName, portNumber);
            SmtpServer.UseDefaultCredentials = false;
            mailMessage.From = new MailAddress(mail.FromEmailId);
            mailMessage.To.Add(mail.ToEmailId);
            mailMessage.Subject = mail.Subject;
            mailMessage.Body = mail.Body;
            SmtpServer.Credentials = new System.Net.NetworkCredential(userName, password);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.EnableSsl = true;
            mailMessage.IsBodyHtml = true;

            try
            {
                SmtpServer.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
