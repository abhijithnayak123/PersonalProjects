///Author: Rahul Anumula
///Date: 14-Mar-2013
///Create Html Helper Methods here to remove the logic from views, reduce html markup and to centralize the code 


using System.Web.Mvc;

namespace Nexxo.HtmlHelpers
{
    public static class Extensions
    {
        private const string Nbsp = "&nbsp;";
        private const string SelectedAttribute = " selected='selected'";

        public static MvcHtmlString NbspIfEmpty(this HtmlHelper helper, string value)
        {
            return new MvcHtmlString(string.IsNullOrEmpty(value) ? Nbsp : value);
        }

        public static MvcHtmlString SelectedIfMatch(this HtmlHelper helper, object expected, object actual)
        {
            return new MvcHtmlString(Equals(expected, actual) ? SelectedAttribute : string.Empty);
        }

        public static MvcHtmlString RequiredField(this HtmlHelper helper, string fieldName)
        {
            return new MvcHtmlString(fieldName + "<span class='error_txt'>*</span>");
        }

        public static MvcHtmlString GreenPlus(this HtmlHelper helper, string fieldName)
        {
            return new MvcHtmlString(fieldName + "<strong> ($)</strong><span style='color:green;font-weight:bold;padding-left:5px;'>+</span>");
        }

        public static MvcHtmlString RedMinus(this HtmlHelper helper, string fieldName)
        {
            return new MvcHtmlString(fieldName + "<strong> ($)</strong><span style='color:red;font-weight:bold;padding-left:5px;'>-</span>");
        }

        public static MvcHtmlString EllipsisWhenOverflow(this HtmlHelper helper, string value, int len)
        {
            string retVal = value;
            if (!string.IsNullOrEmpty(retVal) && retVal.Length > len)
                retVal = value.Substring(0, len) + "...";
            return new MvcHtmlString(retVal);
        }

        public static MvcHtmlString DollarField(this HtmlHelper helper, string fieldName)
        {
            return new MvcHtmlString(fieldName + " ($)");
        }

        public static MvcHtmlString DollarMandatoryField(this HtmlHelper helper, string fieldName)
        {
            return new MvcHtmlString(fieldName + " ($)<span class='error_txt'>*</span>");
        }
    }
}