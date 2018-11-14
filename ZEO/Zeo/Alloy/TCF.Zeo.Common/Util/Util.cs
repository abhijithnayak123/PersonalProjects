using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using TCF.Zeo.Common.Logging.Impl;

namespace TCF.Zeo.Common.Util
{
    public static class AlloyUtil
    {
        public static string TrimString(string value, int size)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Length > size)
                    value = value.Substring(0, size);
            }
            return value;
        }

        public static string TrimString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Trim();
        }

        public static string GetProviderCode(string providerName)
        {
            switch (providerName)
            {
                case "sendmoney": return Convert.ToString((int)Helper.ProviderId.WesternUnion);
                case "billpay": return Convert.ToString((int)Helper.ProviderId.WesternUnionBillPay);
                default: return Convert.ToString((int)Helper.ProviderId.WesternUnion);
            }
        }
        
        public static string MassagingValue(string dataValue)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(dataValue))
            {
                result = Regex.Replace(dataValue, @"[^a-zA-Z0-9\s]", " ");
                result = Regex.Replace(result, @"\s{2,}", " ");
            }
            return result;
        }

        public static string GetProductCode(string productName)
        {
            switch (productName)
            {
                case "sendmoney": return Convert.ToString(((int)Helper.ProductCode.MoneyTransfer));
                case "billpay": return Convert.ToString((int)(Helper.ProductCode.BillPay));
                default: return Convert.ToString(((int)Helper.ProductCode.MoneyTransfer));
            }
        }

        public static String AmountToString(double amount, string majorCurrency)
        {
            String words = "";
            bool majorPlural = true;

            int majorAmount = (int)amount;
            int minorAmount = (int)Math.Round((amount - (int)amount) * 100.0);

            //set plural flags
            if (majorAmount == 1)
                majorPlural = false;
            words = NumberToWords(majorAmount);

            words += majorCurrency;
            if (majorPlural == true)
                words += "s";
            words += " and ";
            words += minorAmount.ToString("00") + "/100";

            return words.Substring(0, 1).ToUpper() + words.Substring(1).ToLower();
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero ";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + "million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + "thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + "hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }

                words += " ";
            }

            return words;
        }

        public static String AmountToStringForTCF(double amount)
        {
            string words = "";
            bool majorPlural = true;

            int majorAmount = (int)amount;
            int minorAmount = (int)Math.Round((amount - (int)amount) * 100.0);

            //set plural flags
            if (majorAmount == 1)
                majorPlural = false;

            //--- major ---
            words = NumberToWordsForTCF(majorAmount);

            words += "AND ";
            if (majorPlural == true)
                words += minorAmount.ToString("00") + "/100";

            return "***" + words + "***";
        }

        public static string NumberToWordsForTCF(int number)
        {
            if (number == 0)
                return "zero ";

            if (number < 0)
                return "minus " + NumberToWordsForTCF(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWordsForTCF(number / 1000000) + "million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWordsForTCF(number / 1000) + "thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWordsForTCF(number / 100) + "hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }

                words += " ";
            }

            return words.ToUpper();
        }

        public static T RESTPostCall<T>(string url, X509Certificate certificate, object input)
        {
            Stream responseStream;
            StreamReader responseReader;
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.ClientCertificates.Add(certificate);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //JsonSerializer serializer = new JsonSerializer();
                streamWriter.Write(JsonConvert.SerializeObject(input));
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            responseStream = httpResponse.GetResponseStream();
            responseReader = new StreamReader(responseStream);
            string response = responseReader.ReadToEnd();

            SerializeObjectAndLog<object>(input);
            T responseObject = JsonConvert.DeserializeObject<T>(response);
            SerializeObjectAndLog<T>(responseObject);

            return responseObject;
        }

        public static void SerializeObjectAndLog<T>(T obj)
        {
            NLoggerCommon logwriter = new NLoggerCommon();
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
    }
}
