using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Common.Util
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// masks the string from left until the length specified, based on the mask character. if the string is null or empty or whitespaces, returns an empty string.
        /// if the input string is formatted then the format will be lost. For ex, abc-abc-abcd will be returned as xxxxxxxxabcd, the - was masked to X.
        /// </summary>
        /// <param name="source">string value to be masked</param>
        /// <param name="length">no of characters to mask from left</param>
        ///         /// <param name="mask">character to be used as mask, default is 'X'</param>
        /// <returns></returns>
        public static string MaskLeft(this string source, int length, char mask = 'X')
        {
            if (string.IsNullOrWhiteSpace(source))
            { return string.Empty; }
            else if (source.Length > length)
            { return source.Substring(source.Length - length, length).PadLeft(source.Length, mask); }
            return source;
        }
        /// <summary>
        /// checks whether the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">type of IEnumberable collection</typeparam>
        /// <param name="source">IEnumberable collection to validate if it is null or empty</param>
        /// <returns>true if the IEnumberable collection is true, else returns false</returns>
        public static bool IsNullorEmpty<T>(this IEnumerable<T> source)
        {
            // check if IEnumberable is null
            if (source == null)
                return true;
            // if the above is sucess, then check the IEnumberable count is less than 1 which will be if it is an empty IEnumberable item
            return (source.Count() < 1);
        }

        /// <summary>
        /// Takes the string, then returns the partial string based on the input length, starting from right.if the string is null or whitespaces or empty, returns an empty string. 
        /// if string length is less than the provided length, then returns the original string.
        /// </summary>
        /// <param name="source">source string to parse</param>
        /// <param name="length">number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string source, int length)
        {
            if (string.IsNullOrWhiteSpace(source))
            { return string.Empty; }
            else if (source.Length > length)
            { return source.Substring(source.Length - length, length); }
            return source;
        }

        /// <summary>
        /// Takes the string, then returns the partial string based on the input length, starting from left index of 0. if the string is null or whitespaces or empty, returns an empty string. 
        /// if string length is less than the provided length, then returns the original string.
        /// </summary>
        /// <param name="source">source string to parse</param>
        /// <param name="length">number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string source, int length)
        {
            if (string.IsNullOrWhiteSpace(source))
            { return string.Empty; }
            else if (source.Length > length)
            { return source.Substring(0, length); }
            return source;
        }


        public static object GetValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return source[key];
            else
                return null;
        }
        public static string GetStringValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return source[key].ToString();
            else
                return null;
        }
        public static bool GetBoolValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return Convert.ToBoolean(source[key]);
            else
                return false;
        }

        public static int GetIntValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return Convert.ToInt32(source[key]);
            else
                return 0;
        }
        public static long GetLongValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return Convert.ToInt64(source[key]);
            else
                return 0L;
        }
        public static decimal GetDecimalValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
        {
            if (source != null && source.ContainsKey(key))
                return Convert.ToDecimal(source[key]);
            else
                return 0.0M;
        }

        public static IEnumerable<SelectListItem> GetSelectListItems(object items)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true });

            foreach (var val in items as List<string>)
            {
                list.Add(new SelectListItem() { Text = val, Value = val });
            }

            return list;
        }


        public static void AddIfNotExist(this Dictionary<string, object> context, string key, object value)
        {
            if (!context.ContainsKey(key))
            {
                context.Add(key, value);
            }
        }
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

        public static int ToProductIdEnum(this int type)
        {
            Product productId = 0;

            switch ((ProductType)type)
            {
                case (int)ProductType.BillPay:
                    productId = Product.BillPayment;
                    break;
                case ProductType.Checks:
                    productId = Product.ProcessCheck;
                    break;
                case ProductType.SendMoney:
                case ProductType.ReceiveMoney:
                case ProductType.Refund:
                    productId = Product.MoneyTransfer;
                    break;
                case ProductType.GPRLoad:
                case ProductType.GPRWithdraw:
                case ProductType.GPRActivation:
                case ProductType.AddOnCard:
                    productId = Product.Fund;
                    break;
                case ProductType.CashIn:
                case ProductType.CashOut:
                    productId = Product.Cash;
                    break;
                case ProductType.MoneyOrder:
                    productId = Product.MoneyOrder;
                    break;
                default:
                    break;
            }

            return (int)productId;
        }


	    public static IEnumerable<string> Split(this string message, int length)
        {
            for (int i = 0; i < message.Length; i += length)
            {
                yield return message.Substring(i, Math.Min((message.Length - i), Math.Min(length, message.Length)));
            }
        }
        
        public static bool IsEnumerable(this Type sourceType)
        {
            if ((sourceType != typeof(string)) && sourceType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                { return true; }
            return false;
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
