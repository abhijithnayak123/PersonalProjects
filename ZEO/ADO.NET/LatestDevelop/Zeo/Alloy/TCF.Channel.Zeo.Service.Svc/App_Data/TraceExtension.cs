using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;
using TCF.Zeo.Common.Logging.Impl;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;

namespace TCF.Channel.Zeo.Service.Svc
{
    public class TraceExtension : SoapExtension
    {
        Stream oldStream;
        Stream newStream;

        public NLoggerCommon NLogger { get; set; }
        public bool logVoltage { get; set; }

        public TraceExtension()
        {
            NLogger = new NLoggerCommon();
            logVoltage = Convert.ToBoolean(ConfigurationManager.AppSettings["LogVoltage"]);
        }

        #region Soap Extension methods

        // Save the Stream representing the SOAP request or SOAP response into  a local memory buffer.      
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
            return;
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.AfterSerialize:
                    WriteOutput(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    WriteInput(message);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private Methods

        private void WriteOutput(SoapMessage message)
        {
            try
            {
                newStream.Position = 0;

                WriteToLog(message, "REQUEST");

                newStream.CopyTo(oldStream);

            }
            catch { }
        }

        private void WriteInput(SoapMessage message)
        {
            try
            {
                oldStream.CopyTo(newStream);

                WriteToLog(message, "RESPONSE");

                newStream.Position = 0;
            }

            catch { }

        }

        private void WriteToLog(SoapMessage message, string type)
        {
            // Encode the Request/Response from the memory stream
            string requestOrResponse = Encoding.UTF8.GetString(((MemoryStream)newStream).GetBuffer());

            XmlDocument document = new XmlDocument();

            document.LoadXml(requestOrResponse);

            //Remove unwanted tag values from XML
            RemoveTagsValue(document);

            string innerXML = string.Empty;

            // In case of invalid XML format, get the innerText to log. Inner text is the valid one for this situation. this issues causing onely few response
            if (document.InnerXml.Contains("&lt;"))
                innerXML = document.InnerText;
            else
                innerXML = SerializeObject(document);


            List<string> removeMethods = new List<string> { "AccessFormattedData", "ProtectFormattedData" };

            bool canLogError = removeMethods.Contains(message.MethodInfo.Name) ? (logVoltage ? true : false) : true;

            if (canLogError)
                NLogger.Info("\n\n" + type + ": " + message.MethodInfo.Name + "\n\n" + innerXML + "\n");

        }

        private void RemoveTagsValue(XmlDocument document)
        {
            // Remove the image vaues(Bytes) to log
            List<string> noLogsAttributes = new List<string>() { "Ckbacktif", "CkBack", "CkFront", "Ckfronttif", "SSAN", "ITIN" };

            foreach (string item in noLogsAttributes)
            {
                XmlNodeList xelements = document.GetElementsByTagName(item);

                foreach (XmlNode node in xelements)
                {
                    if (node.Name == "SSAN" || node.Name == "ITIN")
                    {
                        int itemlength = node.InnerText.Length;
                        if (itemlength > 4)
                        {
                            node.InnerText = node.InnerText.Substring(itemlength - 4);
                            node.InnerText = node.InnerText.PadLeft(itemlength, 'X');
                        }
                    }
                    else
                        node.InnerText = string.Empty;
                }
            }
        }

        private string SerializeObject(XmlDocument toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        #endregion
    }
}