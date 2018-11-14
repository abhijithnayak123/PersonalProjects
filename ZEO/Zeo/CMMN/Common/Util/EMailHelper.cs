using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;

namespace MGI.Common.Util
{
    public class EmailHelper
    {
        private const string NetworkCredential_Username = "betechnical4@gmail.com";
        private const string NetworkCredential_Password = "8147118963";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
		/// /// /****************************Begin TA-50 Changes************************************************/
		//       User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 04.03.2015
		//       Purpose: On Vera Code Scan, the below commented methods were found having Hard-coded Password Flaw and none of below methods are being used
		//public static bool SendMail(MailMessage message, string host, int port)
		//{
		//	bool IsSent = false;
		//	try
		//	{
		//		SmtpClient client = new SmtpClient();
		//		client.UseDefaultCredentials = false;
		//		client.Port = port; 
		//		client.Host = host; 
		//		client.Credentials = new NetworkCredential(NetworkCredential_Username, NetworkCredential_Password);
		//		client.EnableSsl = true;
		//		client.Send(message);
		//		IsSent = true;
		//	}
		//	catch (Exception)
		//	{
		//		return IsSent;
		//	}
		//	return IsSent;
		//}
		/// /****************************End 50 Changes************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool SendMail(MailMessage message, SmtpClient client)
        {
            bool IsSent = false;
            try
            {
                client.Send(message);
                IsSent = true;
            }
            catch (Exception)
            {
                return IsSent;
            }
            return IsSent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEmailID"></param>
        /// <param name="subject"></param>
        /// <param name="fromEmailID"></param>
        /// <param name="fromDisplayName"></param>
        /// <param name="body"></param>
        /// <returns></returns>
		/// /// /****************************Begin TA-50 Changes************************************************/
		//       User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 04.03.2015
		//       Purpose: On Vera Code Scan, the below commented methods were found having Hard-coded Password Flaw and none of below methods are being used
		//public static bool SendMail(string toEmailID, string subject, string fromEmailID, string fromDisplayName, string body,string host,int port)
		//{
		//	bool IsSent = false;
		//	try
		//	{
		//		if (!string.IsNullOrEmpty(fromEmailID))
		//		{
		//			MailMessage objEmail = new MailMessage();
		//			objEmail.To.Add(toEmailID);
		//			objEmail.From = new MailAddress(fromEmailID, fromDisplayName);
		//			objEmail.Subject = subject;
		//			objEmail.Body = body;
		//			objEmail.IsBodyHtml = true;
		//			objEmail.BodyEncoding = System.Text.Encoding.UTF8;

		//			SmtpClient myClient = new SmtpClient();     //System.Net.Mail.SmtpClient myClient = new System.Net.Mail.SmtpClient();//”207.97.202.77″, 25
		//			myClient.UseDefaultCredentials = false;
		//			myClient.Port = port; // 587;
		//			myClient.Host = host; //"smtp.gmail.com";
		//			myClient.Credentials = new NetworkCredential(NetworkCredential_Username, NetworkCredential_Password);
		//			myClient.EnableSsl = true;
		//			myClient.Send(objEmail); 
		//			IsSent = true;
		//		}
		//		return IsSent;
		//	}
		//	catch (InvalidOperationException)
		//	{
		//		return IsSent;
		//	}
		//	catch (SmtpFailedRecipientException)
		//	{
		//		return IsSent;
		//	}
		//	catch (SmtpException)
		//	{
		//		return IsSent;
		//	}
		//}
		/****************************End 50 Changes************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailAttach"></param>
        /// <returns></returns>
		/// /****************************Begin TA-50 Changes************************************************/
		//       User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 04.03.2015
		//       Purpose: On Vera Code Scan, the below commented methods were found having External Control of File Name or Path  Flaw and none of below methods are being used
		//public static bool EmailReportWithAttachment(string emailTo, string emailFrom,string emailSubject, string emailBody, string emailAttach)
		//{
		//	bool IsSent = false;
		//	MailAddress from = new MailAddress(emailFrom);
		//	MailAddress to = new MailAddress(emailTo);
		//	MailMessage msg = new MailMessage(from, to);

		//	try
		//	{

		//	msg.Subject = emailSubject;
		//	msg.Body = emailBody;
		//	Attachment data = new Attachment(emailAttach);
		//	msg.Attachments.Add(data);

		//	SmtpClient client = new SmtpClient("smtp.gmail.com");
		//	//client.Credentials = CredentialCache.DefaultNetworkCredentials;
		//	client.Credentials = new NetworkCredential(NetworkCredential_Username, NetworkCredential_Password);
		//	client.Send(msg);
		//	IsSent = true;

		//	}
		//	catch
		//	{
		//	msg.Dispose();
		//	}
		//  return IsSent;
		//}
		/****************************END TA-50 Changes************************************************/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailCC"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        public static bool EmailReportWithoutAttachment(string emailTo, string emailFrom,string emailCC, string emailSubject, string emailBody)
        {
            bool IsSent = false;
            string strToAddress = emailTo;
            string strFromAddress = emailFrom;
            string strCcAddress = emailCC;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(strFromAddress, "Admin");
            mailMessage.To.Add(new MailAddress(strToAddress));
            mailMessage.CC.Add(new MailAddress(strCcAddress));

            mailMessage.Subject = emailSubject; ;
            mailMessage.IsBodyHtml = true;

            mailMessage.Body = emailBody;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            try
            {
            smtp.Send(mailMessage);
            IsSent = true;
            }
            catch (Exception ex)
            {
            throw (ex);
            }
            mailMessage.Dispose();
           return IsSent;
         }     
     
    }
}

