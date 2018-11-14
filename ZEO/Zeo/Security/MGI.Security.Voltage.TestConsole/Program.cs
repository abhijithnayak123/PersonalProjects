using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Security.Voltage.TestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            TCF.Zeo.Security.Voltage.SecureData secure = new TCF.Zeo.Security.Voltage.SecureData();
            ////string decryptedData = secure.Decrypt("475675Tw12AubUz7005", "kIHa");

            Dictionary<string, ValuePair> customDictionary = new Dictionary<string, ValuePair>();
            TCF.Zeo.Security.Voltage.ValuePair v = new TCF.Zeo.Security.Voltage.ValuePair() { Format = "alloy4x4", FieldValue = "4855078900061882" };
            customDictionary.Add("CardNumber", v);
            ////foreach (KeyValuePair<string, ValuePair> pair in customDictionary)
            ////{
            ////    Console.WriteLine("{0}, {1}",
            ////    pair.Key,
            ////    pair.Value.FieldValue);
            ////}
            ////Console.ReadLine();



            //customDictionary = secure.Tokenize(customDictionary);

            //foreach (KeyValuePair<string, ValuePair> pair in customDictionary)
            //{
            //    Console.WriteLine("{0}, {1}",
            //    pair.Key,
            //    pair.Value.FieldValue);
            //}
            //Console.ReadLine();

            string cardNumbers = "4756WUGEJJQU1587";

            string[] arrCardNumbers = cardNumbers.Split(',');

            foreach (string cn in arrCardNumbers)
            {
                if (cn != "NULL")
                {
                    Dictionary<string, ValuePair> customDictionaryDetoken = new Dictionary<string, ValuePair>();
                    TCF.Zeo.Security.Voltage.ValuePair va = new TCF.Zeo.Security.Voltage.ValuePair() { Format = "First 4 Last 4 SST", FieldValue = cn };
                    customDictionaryDetoken.Add("CardNumber", va);
                    customDictionaryDetoken = secure.Detokenize(customDictionaryDetoken);
                    foreach (KeyValuePair<string, ValuePair> pair in customDictionaryDetoken)
                    {
                        Console.WriteLine("{0}, {1}",
                        cn,
                        pair.Value.FieldValue);
                    }
                }
                else
                {
                    Console.WriteLine("{0}, {1}",
                      cn,
                     "NULL");
                }
            }


            Console.ReadLine();

        }
    }
}
