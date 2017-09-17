using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Security.Voltage.VoltageSecurityService;
using Voltage.Payments.Host;
using System.Configuration;

using System.Text.RegularExpressions;


using MGI.Common.Logging.Contract;



namespace MGI.Security.Voltage
{
    public class SecureData
    {
        
        private ILogger _logger;


        //values should be read from config file in production
        //*********************** SOAP Service Config (Tokenize/Detokenize)****************************
		private static readonly string FORMAT = Convert.ToString(ConfigurationManager.AppSettings["Format"]); 
		private static readonly string IDENTITY = Convert.ToString(ConfigurationManager.AppSettings["Identity"]);
		private static readonly string AUTHINFO = Convert.ToString(ConfigurationManager.AppSettings["Authinfo"]);

        //*********************** HOST DLL Config (Encrypt/Decrypt) ********************************
        private static readonly string DISTRICT_NAME = Convert.ToString(ConfigurationManager.AppSettings["DistrictName"]);
        private static readonly string AUTH_INFO =  Convert.ToString(ConfigurationManager.AppSettings["Authinfo"]);
		private static readonly string POLICY_LOCATOR = Convert.ToString(ConfigurationManager.AppSettings["PolicyLocator"]);
		private static readonly string IDENTITY_STRING = Convert.ToString(ConfigurationManager.AppSettings["IdentityString"]);
		private static readonly string PIE_MERCHANT_ID = Convert.ToString(ConfigurationManager.AppSettings["PieMerchantID"]);
		private static readonly string DECRYPT_TIME = Convert.ToString(ConfigurationManager.AppSettings["DecryptTime"]);
		//2741 : As Alloy, I want the voltage urls to be read from the configuration files, instead of project settings file
		private static readonly string VOLTAGE_URL = Convert.ToString(ConfigurationManager.AppSettings["VoltageUrl"]);
        
        public SecureData()
        {

        }
        public SecureData(ILogger logger )
        {
            _logger = logger;

            // get { return _logger; }
            // set { _logger = value; }
        }


       
		/// <summary>
		/// This method is used for tokenizing the card number(s)
		/// </summary>
		/// <param name="dictionary">dictionary having atlease one card number</param>
		/// <returns>tokenized card number(s)</returns>
        public Dictionary<string, ValuePair> Tokenize(Dictionary<string, ValuePair> dictionary)
        {
            
			try
            {
                VibeSimple serviceProxy = new VibeSimple();
				//2741 : As Alloy, I want the voltage urls to be read from the configuration files, instead of project settings file
				serviceProxy.Url = VOLTAGE_URL;
                foreach (KeyValuePair<string, ValuePair> pair in dictionary)
                {
                    string tokenizedValue = serviceProxy.ProtectFormattedData(pair.Value.FieldValue, FORMAT, IDENTITY, "", AuthMethod.SharedSecret, AUTHINFO);
                    pair.Value.FieldValue = tokenizedValue;
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// This method is used for detokenize the card number(s)
		/// </summary>
		/// <param name="dictionary">dictionary having atleast one tokenzied card number</param>
		/// <returns>detokenized card number</returns>
        public Dictionary<string, ValuePair> Detokenize(Dictionary<string, ValuePair> dictionary)
        {
            try
            {
                VibeSimple serviceProxy = new VibeSimple();
				//2741 : As Alloy, I want the voltage urls to be read from the configuration files, instead of project settings file
				serviceProxy.Url = VOLTAGE_URL;
                foreach (KeyValuePair<string, ValuePair> pair in dictionary)
                {
                    string tokenizedValue = serviceProxy.AccessFormattedData(pair.Value.FieldValue, FORMAT, IDENTITY, "", AuthMethod.SharedSecret, AUTHINFO);
                    pair.Value.FieldValue = tokenizedValue;
                }
                return dictionary;
                //return serviceProxy.AccessFormattedData(data, FORMAT, IDENTITY, "", AuthMethod.SharedSecret, AUTHINFO);
            }
            catch (Exception ex)
			{
				string exceptionMessage = ex.Message.Contains("remote") ? ex.Message.Replace("remote", "Voltage") : ex.Message;
				throw new Exception("Host API exception thrown: " + exceptionMessage);
			}
        }
		
		/// <summary>
		/// This method is used for decrypting card number using cvv
		/// </summary>
		/// <param name="pan">Encrypted Card number</param>
		/// <param name="cvv">Encrypted cvv number associated with card number</param>
		/// <returns>Card Number as plain text</returns>
        public string Decrypt(string pan, string cvv)
        {
            string EMBEDDED_CIPHERTEXT_PAN = pan;
            string EMBEDDED_CIPHERTEXT_CVV = cvv;
            string decryptedData = string.Empty;
            try
            {


                // Create the Host context
                HostContext hostContext = new HostContext(HostContext.FlagsNoStorage, DISTRICT_NAME, POLICY_LOCATOR);

                /* Translating PIE-encrypted data*/

                //This HostPIECardData array holds the PIE-encrypted data to be translated into host-encrypted form
                HostPIECardData[] pieEncryptedCardDataArray = new HostPIECardData[1] {
            new HostPIECardData(HostPIECardData.FLAGS_PIE_TYPE_EMBEDDED, EMBEDDED_CIPHERTEXT_PAN, EMBEDDED_CIPHERTEXT_CVV)};

                // Make the TranslatePIECardData call:  
                // Converts POS-encrypted data into plain data
                HostPIETranslateResult[] pieTranslatedCardDataArray = hostContext.TranslatePIECardData(1, PIE_MERCHANT_ID, HostContext.AuthMethodSharedSecret, AUTH_INFO, DECRYPT_TIME, HostEncryptionSettings.SettingsUnencrypted, null, null, 0, pieEncryptedCardDataArray);

                for (int i = 0; i < pieEncryptedCardDataArray.Length; i++)
                {
                    //Each element of the pieTranslatedCardDataArray array has a 'status' field
                    //The status will be non-zero if any of the input data elements could not be translated
                    if (pieTranslatedCardDataArray[i].GetStatus() == 0)
                    { 
						decryptedData = System.Text.Encoding.Default.GetString(pieTranslatedCardDataArray[i].GetPAN()); 
					}
                }
                return decryptedData;

            }
            catch (HostException exc)
            {
                throw new Exception("Host API exception thrown: error code = " + exc.ErrorCode + "; message = " + exc.Message, exc.InnerException);
            }
        }

       

        //public string MaskSSN(string originalSSN)
        //{
        //    if (originalSSN.Length < 5)
        //        return originalSSN;
        //    var trailingNumbers = originalSSN.Substring(originalSSN.Length-4);
        //    var leadingnumbers = originalSSN.Substring(0,originalSSN.Length-4);
        //    var maskedLeadingnumbers = Regex.Replace(leadingnumbers, @"[0-9]", "*");
        //    return maskedLeadingnumbers + trailingNumbers;

        //}
        //public string MaskCardNumber(string CardNumber)
        //{
        //    string maskCard = string.Empty;
        //    CardNumber = CardNumber.Substring(CardNumber.Length - 4);
        //    maskCard = "**** **** **** " + CardNumber;
        //    return maskCard;
        //}

        //public string Mask(string Card)
        //{
        //    if (Card.Length <= 4)
        //        return Card;
        //    Regex rgx = new Regex(@"(.*?)(\d{4})$");
        //    string result = String.Empty;
        //    if (rgx.IsMatch(Card))
        //    {
        //        for (int i = 0; i < rgx.Matches(Card)[0].Groups[1].Length; i++)
        //            result += "*";
        //        result += rgx.Matches(Card)[0].Groups[2];
        //        return result;
        //    }
        //    return Card;
        //}
        
        //public static string MaskCreditCard(string value)
        //{
        //    const string PATTERN = @"\b(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|" +
        //      @"6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|" +
        //      @"[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})\b";

        //    var replace = Regex.Replace(value, PATTERN, new MatchEvaluator(match =>
        //    {
        //        var num = match.ToString();
        //        return num.Substring(0, 6) + new string('*', num.Length - 10) +
        //          num.Substring(num.Length - 4);
        //    }));

        //    return replace;
        //}
    
    }

    }

