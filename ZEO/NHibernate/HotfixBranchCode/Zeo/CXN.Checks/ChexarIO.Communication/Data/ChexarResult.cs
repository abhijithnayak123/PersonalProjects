using System;
using System.IO;
using System.Xml;

namespace ChexarIO.Communication.Data
{
    public class ChexarResult
    {
        public bool ErrorStatus { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        public ChexarResult()
        {
            this.ErrorCode = 0;
            this.ErrorStatus = false;
            this.ErrorDescription = string.Empty;
        }

        public void ParseResult(string result)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(new StringReader(result));

            try
            {

                var errorCode = xmlDoc.SelectSingleNode("/callcenter/errorcode");

                if (Convert.ToInt32(errorCode.InnerText) != 0)
                {
                    this.ErrorCode = Convert.ToInt32(errorCode.InnerText);
                    this.ErrorStatus = true;
                    this.ErrorDescription = xmlDoc.SelectSingleNode("/callcenter/result").InnerText;
                }

            }
            catch (Exception exception)
            {
                this.ErrorStatus = true;
                this.ErrorCode = -1000;
                this.ErrorDescription = string.Format("Exception occured! Description : {0}", exception.Message);
            }

        }
    }
}
