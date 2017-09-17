using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCF.Zeo.Common.Util
{
    public static partial class Helper
    {
        //Method to differentiate SSN and ITIN
        //AL-1619
        public static TaxIDCode? GetIDCode(string value)
        {
            TaxIDCode? taxIdCode = null;

            if (string.IsNullOrWhiteSpace(value))
                return taxIdCode;

            if (value.Length == 9 && value.Substring(0, 1) == "9")
            {
                int ssnvalue = Convert.ToInt16(value[3].ToString() + value[4].ToString());
                if (ssnvalue >= 70 & ssnvalue <= 88 | ssnvalue >= 90 & ssnvalue <= 92 | ssnvalue >= 94 & ssnvalue <= 99)
                {
                    taxIdCode = TaxIDCode.I;
                }
            }
            else
            {
                taxIdCode = TaxIDCode.S;
            }
            return taxIdCode;
        }

        public static Gender? GetGender(string value)
        {
            Gender? gender = null;
            if (string.IsNullOrWhiteSpace(value))
                return gender;

            switch (value.Trim().ToLower())
            {
                case "m":
                case "male":
                    gender = Gender.MALE;
                    break;
                case "f":
                case "female":
                    gender = Gender.FEMALE;
                    break;
                default:
                    break;
            }
            return gender;
        }

        public static ProfileStatus GetProfileStatus(string status)
        {
            ProfileStatus profileStatus;
            Enum.TryParse(status, true, out profileStatus);

            return profileStatus;
        }

        public static DateTime GetTimeZoneTime(string timeZone)
        {
            DateTime timeZoneDateTime = DateTime.Now;
            try
            {
                timeZoneDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
            }
            catch
            {
            }
            return timeZoneDateTime;
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

        public static object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary != null && dictionary.ContainsKey(key))
                return dictionary[key];
            else
                return null;
        }

        public static decimal GetDecimalDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
        {
            decimal value = 0.0M;
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return Convert.ToDecimal(dictionary[key]);
            }
            return value;
        }

        public static string RoundOffDecimal(decimal amount, int decimalLength)
        {
            decimal finalAmount = (decimal)Math.Round(amount, decimalLength);
            string amountUptoTwoDecimals = string.Format("{0:0.00}", finalAmount);
            return amountUptoTwoDecimals;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string GenerateRandomNumber(int length)
        {
            Random r = new Random(1);
            string num = string.Empty;
            for (int i = 0; i < length; i++)
            {
                num = string.Concat(num, r.Next(0, 9).ToString());
            }
            return num;
        }

        public static string SafeSQLString(string s, bool bln)
        {
            if (s.All(c => Char.IsLetterOrDigit(c) || c != '&' || c != '<' || c != '>' || c != '/' || c != '\\' || c != '"' || c != '\'' || c != '?' || c != '+') && bln)
                return s.Replace("\\", "").Replace("'", "''");
            else
                return "";
        }

        public static bool GetBoolDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
        {
            bool value = false;
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return Convert.ToBoolean(dictionary[key]);
            }
            return value;
        }

        public static string GetDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
        {
            string value = null;
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return Convert.ToString(dictionary[key]);
            }
            return value;
        }

        public static int GetCustomerAgeByDateOfBirth(DateTime dateOfBirth)
        {
            int currentAge = (DateTime.Today.Year - dateOfBirth.Year);
            if (dateOfBirth > DateTime.Today.AddYears(-currentAge))
            { currentAge--; }

            return currentAge;
        }

        public static long GetLongRandomNumber(Int32 iMax)
        {
            RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
            var sBuilder = new StringBuilder();
            while (iMax > 0)
            {
                sBuilder.Append(randomCryptoServiceProvider.Next(10).ToString());
                iMax = iMax - 1;
            }
            string sRandom = sBuilder.ToString();
            long lRandom = Convert.ToInt64(sRandom);
            return lRandom;
        }
    }
}
