using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace MGI.Common.Util
{
	public static class ExtensionHelper
	{
		public static DataTable ToDataTable<T>(this IList<T> data)
		{
			PropertyDescriptorCollection props =
				TypeDescriptor.GetProperties(typeof(T));
			DataTable table = new DataTable();
			for (int i = 0; i < props.Count; i++)
			{
				PropertyDescriptor prop = props[i];
				table.Columns.Add(prop.Name, prop.PropertyType);
			}
			object[] values = new object[props.Count];
			foreach (T item in data)
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = props[i].GetValue(item);
				}
				table.Rows.Add(values);
			}
			return table;
		}

		public static IEnumerable<string> Split(this string message, int length)
		{
			for (int i = 0; i < message.Length; i += length)
			{
				yield return message.Substring(i, Math.Min((message.Length - i), Math.Min(length, message.Length)));
			}
		}

        /// <summary>
        /// Adds the specified key and value to the dictionary if key not exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public static void AddIfNotExist(this Dictionary<string, object> context, string key, object value)
        {
            if (!context.ContainsKey(key))
            {
                context.Add(key, value);
            }
        }

        /// <summary>
        /// Updates the specified value for key to the dictionary if key exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public static void UpdateIfExist(this Dictionary<string, object> context, string key, object value)
        {
            if (context.ContainsKey(key))
            {
                context[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary if key not exist, 
        /// or updates the specified value for key to the dictionary if key exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public static void AddOrUpdate(this Dictionary<string, object> context, string key, object value)
        {
            if (context.ContainsKey(key))
            {
                context[key] = value;
            }
            else
            {
                context.Add(key, value);
            }
        }

        public static bool IsCreditCardNumber(this string cardNumber)
        {
            //// check whether input string is null or empty
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            bool isValid = false;

            if (cardNumber.IsValidCreditCard())
            {
                //// 1.	Starting with the check digit double the value of every other digit 
                //// 2.	If doubling of a number results in a two digits number, add up
                ///   the digits to get a single digit number. This will results in eight single digit numbers                    
                //// 3. Get the sum of the digits
                int sumOfDigits = cardNumber.Where((e) => e >= '0' && e <= '9')
                                .Reverse()
                                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                                .Sum((e) => e / 10 + e % 10);

                //// If the final sum is divisible by 10, then the credit card number
                //   is valid. If it is not divisible by 10, the number is invalid.            
                isValid = sumOfDigits % 10 == 0;
            }

            return isValid;
        }

        /// <summary>
        /// AL-530 : Need to perform additional validations on account numbers in WU Bill Payment account number
        /// Method to check the given string is a valid credit card number
        /// </summary>
        /// <param name="creditCardNumber"></param>
        /// <returns></returns>
        private static bool IsValidCreditCard(this string creditCardNumber)
        {
            bool isValid = false;

            //^(?:4[0-9]{12}(?:[0-9]{3})?          # Visa
            // |  5[1-5][0-9]{14}                  # MasterCard
            // |  3[47][0-9]{13}                   # American Express
            // |  3(?:0[0-5]|[68][0-9])[0-9]{11}   # Diners Club
            // |  6(?:011|5[0-9]{2})[0-9]{12}      # Discover
            // |  (?:2131|1800|35\d{3})\d{11}      # JCB
            //)$

            Regex validCreditCardPattern = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|6(?:011|5[0-9]{2})[0-9]{12}|(?:2131|1800|35\d{3})\d{11})$");
            isValid = validCreditCardPattern.IsMatch(creditCardNumber);

            return isValid;
        }

        public static string MaskAccountNumber(this string accountNumber)
        {
            string maskedAccountNumber = accountNumber;

            if (!string.IsNullOrWhiteSpace(maskedAccountNumber) && maskedAccountNumber.IsCreditCardNumber())
            {
                string maskNumber = maskedAccountNumber.Substring(maskedAccountNumber.Length - 4, 4);
                int maskingLength = maskedAccountNumber.Length - 4;

                maskedAccountNumber = maskNumber.PadLeft(maskingLength, '*');
            }
            return maskedAccountNumber;
        }
	}
}
