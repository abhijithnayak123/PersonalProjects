using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MGI.Security.Voltage.TestConsole
{
   public class Program
    {
        static void Main(string[] args)
        {
			SecureData secure = new SecureData();
			//string decryptedData = secure.Decrypt("475675Tw12AubUz7005", "kIHa");

			//Dictionary<string, ValuePair> customDictionary = new Dictionary<string, ValuePair>();
			//ValuePair v = new ValuePair() { Format = "First 4 Last 4 SST", FieldValue = "4855078700026120" };
			//customDictionary.Add("CardNumber", v);
			//foreach (KeyValuePair<string, ValuePair> pair in customDictionary)
			//{
			//	Console.WriteLine("{0}, {1}",
			//	pair.Key,
			//	pair.Value.FieldValue);
			//}
			//Console.ReadLine();

            
           
			//customDictionary = secure.Tokenize(customDictionary);

			//foreach (KeyValuePair<string, ValuePair> pair in customDictionary)
			//{
			//	Console.WriteLine("{0}, {1}",
			//	pair.Key,
			//	pair.Value.FieldValue);
			//}
			//Console.ReadLine();
			Dictionary<string, ValuePair> customDictionaryDetoken = new Dictionary<string, ValuePair>();
			ValuePair va = new ValuePair() { Format = "First 4 Last 4 SST", FieldValue = "4855ZWVLUECE0950" };
			customDictionaryDetoken.Add("CardNumber", va);
			customDictionaryDetoken = secure.Detokenize(customDictionaryDetoken);
			foreach (KeyValuePair<string, ValuePair> pair in customDictionaryDetoken)
			{
				Console.WriteLine("{0}, {1}",
				pair.Key,
				pair.Value.FieldValue);
			}
			Console.ReadLine();
        
		}
    }
}
