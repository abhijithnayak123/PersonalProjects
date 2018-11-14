using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MGI.Security.Voltage;

using TCF.Zeo.Common.DataProtection.Contract;
using TCF.Zeo.Security.Voltage;

namespace TCF.Zeo.Common.DataProtection.Impl
{
	public class DataProtectionSimulator : IDataProtectionService
	{
		#region IDataProtectionService Members

		public string Encrypt(string data, int slot)
		{
			//return new string(data.Reverse().ToArray());
            SecureData secure = new SecureData();
            Dictionary<string, ValuePair> customDictionary = new Dictionary<string, ValuePair>();
            ValuePair v = new ValuePair() { Format = "First 4 Last 4 SST", FieldValue = data };
            customDictionary.Add("CardNumber", v);

            customDictionary = secure.Tokenize(customDictionary);
            ValuePair valuePair = customDictionary["CardNumber"];

            return valuePair.FieldValue;
		}

		public string Decrypt(string cipherText, int slot)
		{
			//return new string(cipherText.Reverse().ToArray());

            SecureData secure = new SecureData();
            Dictionary<string, ValuePair> customDictionary = new Dictionary<string, ValuePair>();
            ValuePair v = new ValuePair() { Format = "First 4 Last 4 SST", FieldValue = cipherText };
            customDictionary.Add("CardNumber", v);

            customDictionary = secure.Detokenize(customDictionary);
            ValuePair valuePair = customDictionary["CardNumber"];

            return valuePair.FieldValue;
            
		}

		#endregion
	}
}
