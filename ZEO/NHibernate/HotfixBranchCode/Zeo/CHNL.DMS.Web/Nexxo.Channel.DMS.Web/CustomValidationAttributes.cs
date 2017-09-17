using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;


namespace MGI.Channel.DMS.Web
{
    //DefaultAttribute
    public class DefaultAttribute : ValidationAttribute, IClientValidatable
    {
        public string DefaultValue { get; set; }
        public string DependingProperty { get; set; }
        public string DependingPropertyValue { get; set; }
        public string CurrentProperty { get; set; }
        public bool IsRequired { get; set; }

        public DefaultAttribute(string defaultValue, string currentProperty, bool isRequired, string dependingProperty = "", string dependingPropertyValue = "")
            : base("{0} is required.")
        {
            DefaultValue = defaultValue;
            DependingProperty = dependingProperty;
            DependingPropertyValue = dependingPropertyValue;
            CurrentProperty = currentProperty;
            IsRequired = isRequired;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var cProperty = context.ObjectInstance.GetType().GetProperty(CurrentProperty);

            if (cProperty != null)
            {
                if (cProperty.GetCustomAttributes(typeof(RequiredAttribute), true).Length > 0)
                {
                    if (value != null)
                    {
                        if (DependingProperty != "" && DependingPropertyValue != "")
                        {
                            var curruntProperty = context.ObjectInstance.GetType().GetProperty(DependingProperty);
                            var curruntPropertyValue = curruntProperty.GetValue(context.ObjectInstance, null);

                            if (curruntPropertyValue.Equals(DependingPropertyValue) && value.Equals(DefaultValue))
                            {
                                if (!string.IsNullOrEmpty(ErrorMessage))
                                    return new ValidationResult(ErrorMessage);
                                else
                                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                            }
                        }
                        else if (value.Equals(DefaultValue))
                        {
                            if (!string.IsNullOrEmpty(ErrorMessage))
                                return new ValidationResult(ErrorMessage);
                            else
                                return new ValidationResult(FormatErrorMessage(context.DisplayName));
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationSelectOneRule(FormatErrorMessage(metadata.DisplayName), DefaultValue, DependingProperty, DependingPropertyValue, IsRequired) };
        }
    }

    //DefaultAttribute
    public class PhoneAttribute : ValidationAttribute, IClientValidatable
    {
        public string DefaultValue { get; set; }
        public string DependingProperty { get; set; }
        public string DependingPropertyValue { get; set; }
        public string CurrentProperty { get; set; }

        public PhoneAttribute(string defaultValue, string currentProperty, string dependingProperty = "", string dependingPropertyValue = "")
            : base("{0} is required.")
        {
            DefaultValue = defaultValue;
            DependingProperty = dependingProperty;
            DependingPropertyValue = dependingPropertyValue;
            CurrentProperty = currentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var cProperty = context.ObjectInstance.GetType().GetProperty(CurrentProperty);

            if (cProperty != null)
            {
                if (cProperty.GetCustomAttributes(typeof(RequiredAttribute), true).Length > 0)
                {
                    if (DependingProperty != "" && DependingPropertyValue != "")
                    {
                        var curruntProperty = context.ObjectInstance.GetType().GetProperty(DependingProperty);
                        var curruntPropertyValue = curruntProperty.GetValue(context.ObjectInstance, null);

                        if (curruntPropertyValue.Equals(DependingPropertyValue) && value.Equals(DefaultValue))
                        {
                            if (!string.IsNullOrEmpty(ErrorMessage))
                                return new ValidationResult(ErrorMessage);
                            else
                                return new ValidationResult(FormatErrorMessage(context.DisplayName));
                        }
                    }
                    else if (value.Equals(DefaultValue))
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                            return new ValidationResult(ErrorMessage);
                        else
                            return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelPhoneSelectOneRules(FormatErrorMessage(metadata.DisplayName), DefaultValue, DependingProperty, DependingPropertyValue) };
        }
    }

    public class ModelPhoneSelectOneRules : ModelClientValidationRule
    {
        public ModelPhoneSelectOneRules(string errorMessage, string defaultValue, string dependingProperty, string dependingPropertyValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "phonevalidation";
            ValidationParameters.Add("defaultvalue", defaultValue);
            ValidationParameters.Add("dependingproperty", dependingProperty);
            ValidationParameters.Add("dependingpropertyvalue", dependingPropertyValue);
        }
    }


    public class ModelClientValidationSelectOneRule : ModelClientValidationRule
    {
        public ModelClientValidationSelectOneRule(string errorMessage, string defaultValue, string dependingProperty, string dependingPropertyValue, bool isRequired)
        {
            ErrorMessage = errorMessage;
            ValidationType = "mycustomvalidation";
            ValidationParameters.Add("defaultvalue", defaultValue);
            ValidationParameters.Add("dependingproperty", dependingProperty);
            ValidationParameters.Add("dependingpropertyvalue", dependingPropertyValue);
            ValidationParameters.Add("isrequired", isRequired);
        }
    }

    //DateRangeAttrbute CustomValidation
    public class DateRangeAttribute : ValidationAttribute, IClientValidatable
    {
        public int MinDate { get; set; }
        public int MaxDate { get; set; }

        /// <summary>
        /// For validating the datetime past dates, and feature dapending on the parameter you pass. For past passitive numbers, For feature negative numbers.
        /// </summary>
        /// <param name="minimum">if u pass +ve that means past back that number of years ago, if u pass +ve that means feature that number of years later</param>
        /// <param name="maximum">if u pass +ve that means past back that number of years ago, if u pass +ve that means feature that number of years later</param>
        public DateRangeAttribute(int minimum, int maximum)
            : base("{0} is not valid.")
        {
            MinDate = minimum;
            MaxDate = maximum;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime dtValue;

            DateTime.TryParse(Convert.ToString(value), out dtValue);

            if (dtValue == DateTime.MinValue)
                return ValidationResult.Success;

            if (!(Convert.ToDateTime(value.ToString()) < DateTime.Now.AddYears(-(MinDate)) && DateTime.Now.AddYears(-(MaxDate)) < Convert.ToDateTime(value.ToString())))
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    return new ValidationResult(ErrorMessage);
                else
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "mydatecustomvalidation";
            rule.ValidationParameters.Add("minvalue", MinDate);
            rule.ValidationParameters.Add("maxvalue", MaxDate);
            yield return rule;
            //return new[] { new ModelDateValidationSelectOneRule(FormatErrorMessage(metadata.DisplayName), MinDate, MaxDate) };
        }
    }

    public class ModelDateValidationSelectOneRule : ModelClientValidationRule
    {
        public ModelDateValidationSelectOneRule(string errorMessage, int minValue, int maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "mydatecustomvalidation";
            ValidationParameters.Add("minvalue", minValue);
            ValidationParameters.Add("maxvalue", maxValue);
        }

        public ModelDateValidationSelectOneRule(string ValType, string errorMessage, int minValue, int maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = ValType;
            ValidationParameters.Add("minvalue", minValue);
            ValidationParameters.Add("maxvalue", maxValue);
        }
    }

    public class DateTimeAttribute : ValidationAttribute, IClientValidatable
    {
        public DateTimeAttribute()
            : base("Please provide {0} in MM/DD/YYYY format.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            bool isValid = false;
            DateTime dtValue;
            if (string.IsNullOrEmpty(str))
                return ValidationResult.Success;

            DateTime.TryParse(str, out dtValue);
            isValid = dtValue == DateTime.MinValue ? false : true;

            if (isValid == false)
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    return new ValidationResult(ErrorMessage);
                else
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            else
                return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "dateofbirthformat";
            rule.ValidationParameters.Add("other", 0);
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeNotFutureAttribute : ValidationAttribute, IClientValidatable
    {
        public bool CheckFutureDate { get; set; }

        public DateTimeNotFutureAttribute()
            : base("{0} cannot be a future date.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            DateTime dtValue;
            if (string.IsNullOrEmpty(str))
                return ValidationResult.Success;

            DateTime.TryParse(str, out dtValue);

            if (dtValue == DateTime.MinValue)
                return ValidationResult.Success;
            if (CheckFutureDate)
            {
                if (dtValue > DateTime.Today)
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                        return new ValidationResult(ErrorMessage);
                    else
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
                else
                    return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "dateofbirthnofuturedate";
            rule.ValidationParameters.Add("other", 0);
            yield return rule;

        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeWithFutureAttribute : ValidationAttribute, IClientValidatable
    {
        public bool CheckFutureDate { get; set; }

        public DateTimeWithFutureAttribute()
            : base("{0} cannot be a past date.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            DateTime dtValue;
            if (string.IsNullOrEmpty(str))
                return ValidationResult.Success;

            string[] days = value.ToString().Split('/');

            DateTime.TryParse(str, out dtValue);

            DateTime todayDate = DateTime.Today;

            if (days.Length == 2)
            {
                dtValue = new DateTime(dtValue.Year, dtValue.Month, DateTime.DaysInMonth(dtValue.Year, dtValue.Month));
                todayDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
            }

            if (days.Length == 3)
            {
                todayDate = DateTime.Today;
            }
            
            if (dtValue == DateTime.MinValue)
                return ValidationResult.Success;
            if (CheckFutureDate)
            {
                if (dtValue < todayDate)
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                        return new ValidationResult(ErrorMessage);
                    else
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
                else
                    return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "expirationdatewithfuturedate";
            rule.ValidationParameters.Add("other", 0);
            yield return rule;

        }
    }

    //ConditionalRequiredAttribute CustomValidationtion
    public class ConditionalRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public string PropertyOne { get; set; }
        public string PropertyTwo { get; set; }

        public ConditionalRequiredAttribute(string propertyOne, string propertyTwo)
            : base("{0} is required.")
        {
            PropertyOne = propertyOne;
            PropertyTwo = propertyTwo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (PropertyTwo.ToLower() == "issuedatecondition")
            {
                var _propertyOne = context.ObjectInstance.GetType().GetProperty(PropertyOne);
                var _propertyOneValue = _propertyOne.GetValue(context.ObjectInstance, null);

                if (Convert.ToString(_propertyOneValue).ToLower() == "driver's license")
                {
                    if (value == null || value.ToString() == "")
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                            return new ValidationResult(ErrorMessage);
                        else
                            return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                    else
                        return ValidationResult.Success;
                }
                else
                    return ValidationResult.Success;
            }

            var propertyOne = context.ObjectInstance.GetType().GetProperty(PropertyOne);
            var propertyOneValue = propertyOne.GetValue(context.ObjectInstance, null);

            var propertyTwo = context.ObjectInstance.GetType().GetProperty(PropertyTwo);
            var propertyTwoValue = propertyTwo.GetValue(context.ObjectInstance, null);

            if (Convert.ToString(propertyOneValue).ToLower() == "united states" && Convert.ToString(propertyTwoValue).ToLower() == "driver's license" || Convert.ToString(propertyOneValue).ToLower() == "united states" && Convert.ToString(propertyTwoValue).ToLower() == "u.s. state identity card"
                || Convert.ToString(propertyOneValue).ToLower() == "mexico" && Convert.ToString(propertyTwoValue).ToLower() == "licencia de conducir")
            {
                if (value != null && Convert.ToString(value).Trim().ToUpper() == "Select".ToUpper())
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                        return new ValidationResult(ErrorMessage);
                    else
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
            }

            if (PropertyTwo == "GovtIdIssueState")
            {
                if (Convert.ToString(propertyOneValue).ToLower() == "united states" || Convert.ToString(propertyOneValue).ToLower() == "us" || Convert.ToString(propertyOneValue).ToLower() == "usa" || Convert.ToString(propertyOneValue).ToLower() == "canada" || Convert.ToString(propertyOneValue).ToLower() == "mx")
                {
                    if (value != null && Convert.ToString(value).Trim().ToUpper() == "Select".ToUpper())
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                            return new ValidationResult(ErrorMessage);
                        else
                            return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
            }

            if (PropertyTwo == "City")
            {
                if (Convert.ToString(propertyOneValue).ToLower() == "mx")
                {
                    if (value != null && Convert.ToString(value).Trim().ToUpper() == "Select".ToUpper())
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                            return new ValidationResult(ErrorMessage);
                        else
                            return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
            }

            if (PropertyTwo == "TestAnswer" || PropertyTwo == "TestQuestion")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(propertyOneValue)))
                {
                    if (string.IsNullOrEmpty(Convert.ToString(propertyTwoValue)))
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                            return new ValidationResult(ErrorMessage);
                        else
                            return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelConditionalRequiredValidationSelectOneRule(FormatErrorMessage(metadata.DisplayName), PropertyOne, PropertyTwo) };
        }
    }

    public class ModelConditionalRequiredValidationSelectOneRule : ModelClientValidationRule
    {
        public ModelConditionalRequiredValidationSelectOneRule(string errorMessage, string propertyOne, string propertyTwo)
        {
            ErrorMessage = errorMessage;
            ValidationType = "myconditionalrequiredcustomvalidation";
            ValidationParameters.Add("propertyone", propertyOne);
            ValidationParameters.Add("propertytwo", propertyTwo);
        }
    }

    public class ConditionalCompareRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
        public string CardLength { get; set; }

        public ConditionalCompareRequiredAttribute(string firstValue, string secondValue, string cardLength)
            : base("{0} is required.")
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
            CardLength = cardLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var firstValue = FirstValue;
            var secondValue = SecondValue;
            var cardLength = CardLength;

            var propertyOne = context.ObjectInstance.GetType().GetProperty(SecondValue);
            var SecondPropertyValue = propertyOne.GetValue(context.ObjectInstance, null);

            var fieldNameCard = context.ObjectInstance.GetType().GetProperty(CardLength);
            var fieldValueCard = fieldNameCard.GetValue(context.ObjectInstance, null);

            if (firstValue.ToLower() == SecondPropertyValue.ToString().ToLower())
            {
                if (value == null || Convert.ToString(value) == "")
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                        return new ValidationResult(ErrorMessage);
                    else
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
                else if (value.ToString().Length != int.Parse(fieldValueCard.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelConditionalCompareRequiredValidationSelectOneRule(FormatErrorMessage(metadata.DisplayName), FirstValue, SecondValue) };
        }
    }


    public class ModelConditionalCompareRequiredValidationSelectOneRule : ModelClientValidationRule
    {
        public ModelConditionalCompareRequiredValidationSelectOneRule(string errorMessage, string firstValue, string secondValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "myconditionalcomparerequiredcustomvalidation";
            ValidationParameters.Add("firstvalue", firstValue);
            ValidationParameters.Add("secondvalue", secondValue);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        // Have to override IsValid
        public override bool IsValid(object value)
        {
            //  Need to use reflection to get properties of "value"...
            var typeInfo = value.GetType();

            var propertyInfo = typeInfo.GetProperties();

            int count = 0;
            foreach (var property in propertyInfo)
            {
                if (property.Name.ToString() == "CardNumber")
                    if (null != property.GetValue(value, null))
                        return true;

				if (property.Name.ToString() == "FirstName" || property.Name.ToString() == "LastName" || property.Name.ToString() == "PhoneNumber" || property.Name.ToString() == "DateOfBirth" || property.Name.ToString() == "CardNumber" || property.Name.ToString() == "GovernmentId" || property.Name.ToString() == "SSN")
                {
                    if (null != property.GetValue(value, null))
                    {
                        // We've found a property with a value
                        count++;
                    }
                }
            }

            if (count < 2)
            {
                if ((string)propertyInfo.First(x => x.Name == "SearchType").GetValue(value, null) == "card")
                {
                    this.ErrorMessageResourceName = "CustomerSearchCardNumberRequired";
                }
                else
                {
                    this.ErrorMessageResourceName = "CustomerSearchAtleastOneAttribute";
                }

                this.ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo);
                return false;
            }
            else
            {
                if (null != propertyInfo.First(x => x.Name == "LastName").GetValue(value, null))
                {
                    if (null != propertyInfo.First(x => x.Name == "PhoneNumber").GetValue(value, null))
                        return true;

                    if (null != propertyInfo.First(x => x.Name == "DateOfBirth").GetValue(value, null))
                        return true;

                    if (null != propertyInfo.First(x => x.Name == "SSN").GetValue(value, null))
                        return true;

					if (null != propertyInfo.First(x => x.Name == "GovernmentId").GetValue(value, null))
                        return true;
                }
				if (null != propertyInfo.First(x => x.Name == "GovernmentId").GetValue(value, null))
                {
                    if (null != propertyInfo.First(x => x.Name == "PhoneNumber").GetValue(value, null))
                        return true;

                    if (null != propertyInfo.First(x => x.Name == "DateOfBirth").GetValue(value, null))
                        return true;

                    if (null != propertyInfo.First(x => x.Name == "SSN").GetValue(value, null))
                        return true;
                }
                if (null != propertyInfo.First(x => x.Name == "DateOfBirth").GetValue(value, null))
                {
                    if (null != propertyInfo.First(x => x.Name == "PhoneNumber").GetValue(value, null))
                        return true;

                    if (null != propertyInfo.First(x => x.Name == "SSN").GetValue(value, null))
                        return true;
                }
                if (null != propertyInfo.First(x => x.Name == "PhoneNumber").GetValue(value, null))
                {
                    if (null != propertyInfo.First(x => x.Name == "SSN").GetValue(value, null))
                        return true;
                }
            }

            this.ErrorMessageResourceName = "CustomerSearchCombinationAttribute";
            this.ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo);
            return false;
        }
    }

    public sealed class DateGreaterThanAttribute : ValidationAttribute//, IClientValidatable
    {
        private const string _defaultErrorMessage = "{0} must be greater than {1}";
        private string _basePropertyName;

        public DateGreaterThanAttribute(string basePropertyName)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = basePropertyName;
        }

        //Override default FormatErrorMessage Method
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, _basePropertyName);
        }

        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            DateTime dtValue;
            if (string.IsNullOrEmpty(str))
                return ValidationResult.Success;

            DateTime.TryParse(str, out dtValue);

            if (dtValue == DateTime.MinValue)
                return ValidationResult.Success;

            ////Get PropertyInfo Object
            var basePropertyInfo = validationContext.ObjectType.GetProperty(_basePropertyName);

            ////Get Value of the property
			var DateOfBirth = basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            string DateofBirth = Convert.ToString(DateOfBirth, CultureInfo.CurrentCulture);

            DateTime DobdtValue;
			DateTime.TryParse(DateofBirth, out DobdtValue);

            //Actual comparision
            if (dtValue < DobdtValue)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }

            //Default return - This means there were no validation error
            return null;
        }

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TempPasswordRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Please enter a valid Temp Password";
        private string _basePropertyName;

        public TempPasswordRequiredAttribute(string baseStatus)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = baseStatus;
        }

        //Override default FormatErrorMessage Method
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, _basePropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var basePropertyInfo = context.ObjectType.GetProperty(_basePropertyName);
            var status = (string)basePropertyInfo.GetValue(context.ObjectInstance, null);

            if ((!string.IsNullOrWhiteSpace(status)) || (!string.IsNullOrWhiteSpace(status)))
                if (status == "2" || status == "3")
                    return ValidationResult.Success;

            string tempPassword = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (status == "1")
            {
                if (string.IsNullOrWhiteSpace(tempPassword))
                {
                    var message = FormatErrorMessage(context.DisplayName);
                    return new ValidationResult(message);
                }
            }

            if (string.IsNullOrWhiteSpace(tempPassword))
            {
                var message = FormatErrorMessage(context.DisplayName);
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "temppwd";
            rule.ValidationParameters.Add("userstatus", _basePropertyName);
            yield return rule;
        }
    }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PhoneNumberSequenceAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Please enter a valid Phone Number";

        private string _basePropertyName;
        public PhoneNumberSequenceAttribute(string baseProperty)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = baseProperty;
        }

        //Override default FormatErrorMessage Method
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, _basePropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string phoneNumber = Convert.ToString(value, CultureInfo.CurrentCulture);

            Regex phoneRgx1 = new Regex("^[2-9]\\d{9}$");
            Regex phoneRgx2 = new Regex("2{9}|3{9}|4{9}|5{9}|6{9}|7{9}|8{9}|9{9}");

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                phoneNumber = phoneNumber.Replace("-", "");

                bool result = (!string.IsNullOrEmpty(phoneNumber) && (!phoneRgx1.IsMatch(phoneNumber ?? "") || phoneRgx2.IsMatch(phoneNumber ?? "")));

                if (result)
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                else
                    return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "phonenumbersequence"
            };
            rule.ValidationParameters.Add("phone", _basePropertyName);
            yield return rule;
        }
    }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SSNNumberRegexAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Please enter valid SSN/ITIN number";
        private string _basePropertyName;

        public SSNNumberRegexAttribute(string actualSSN)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = actualSSN;
        }

        //Override default FormatErrorMessage Method
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, _basePropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var basePropertyInfo = context.ObjectType.GetProperty(_basePropertyName);
            var ssnNo = (string)basePropertyInfo.GetValue(context.ObjectInstance, null);

            Regex ssnRgx = new Regex("^(?!(000|666))([0-9]\\d{2}|7([0-9]\\d|7[012]))-?(?!00)\\d{2}-?(?!0000)\\d{4}$");

            if (!string.IsNullOrWhiteSpace(ssnNo))
            {
                bool result = (!ssnRgx.IsMatch(ssnNo ?? ""));

                if (!result)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "ssnnumber";
            rule.ValidationParameters.Add("acutalssn", _basePropertyName);
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumInitialLoadAmountAttribute : ValidationAttribute, IClientValidatable
    {      
        private string _minimumInitialLoadAmount;
		private string CardActivationTrx;
        private const string _defaultErrorMessage = "Load amount should be greater than the min amount";
        private string _errorMessageForInitialMinLoadAmount;
        private string _errorMessageForMinLoadAmount;

        public MinimumInitialLoadAmountAttribute(string minimumLoadAmount, string isCardActivationTrx, string ShoppingCartDetailInitialMinLoadAmtMsg, string ShoppingCardDetailMinimumLoadAmountMsg)
            : base(_defaultErrorMessage)
        {
            _minimumInitialLoadAmount = minimumLoadAmount;
			CardActivationTrx = isCardActivationTrx;
            _errorMessageForInitialMinLoadAmount = ShoppingCartDetailInitialMinLoadAmtMsg;
            _errorMessageForMinLoadAmount = ShoppingCardDetailMinimumLoadAmountMsg;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var loadAmountInfo = context.ObjectType.GetProperty(_minimumInitialLoadAmount.ToString());
			var minimumLoadAmount = (decimal)loadAmountInfo.GetValue(context.ObjectInstance, null);
			var isCardActivationTrx = context.ObjectType.GetProperty(CardActivationTrx.ToString());
			var IsCardActivationTrx = (bool)isCardActivationTrx.GetValue(context.ObjectInstance, null);

			if (IsCardActivationTrx && minimumLoadAmount <= Convert.ToDecimal(value))
				return ValidationResult.Success;
			else if (!IsCardActivationTrx && (minimumLoadAmount <= Convert.ToDecimal(value) || Convert.ToDecimal(value) == 0))
            {
                return ValidationResult.Success;
            }
            else
            {
                if (IsCardActivationTrx)
                {
                    this.ErrorMessageResourceName = _errorMessageForInitialMinLoadAmount;
                }
                else
                    this.ErrorMessageResourceName = _errorMessageForMinLoadAmount;
            
               return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();           

            var IsCardActivationTrx = ((MGI.Channel.DMS.Web.Models.ShoppingCartDetail)(((System.Web.Mvc.ViewContext)(context)).ViewData.Model)).IsCardActivationTrx;
            if (IsCardActivationTrx)
            {
                this.ErrorMessageResourceName = _errorMessageForInitialMinLoadAmount;
            }
            else
                this.ErrorMessageResourceName = _errorMessageForMinLoadAmount;

            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);

            rule.ValidationType = "minimumloadamount";
            rule.ValidationParameters.Add("actualloadamount", _minimumInitialLoadAmount);
          
            yield return rule;
        }
    }

    	
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class RequiredForChannelPartnerAttribute : ValidationAttribute, IClientValidatable
	{
		private string _channelPartner;
		private const string _defaultErrorMessage = "Field is mandatory.";
		private string _channelpartnervalue;
		private string _channelpartnerpropvalue;

		public RequiredForChannelPartnerAttribute(string channelPartnerProp, string channelPartnerValue)
			: base(_defaultErrorMessage)
		{
			_channelPartner = channelPartnerProp;
			_channelpartnervalue = channelPartnerValue;
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var channel = context.ObjectType.GetProperty(_channelPartner.ToString());
			var ChannelPartner = channel.GetValue(context.ObjectInstance, null);
			_channelpartnerpropvalue = ChannelPartner.ToString();

			if ((ChannelPartner.ToString() != _channelpartnervalue) && (ChannelPartner.ToString() == null || ChannelPartner.ToString() == ""))
			{
                if (value.ToString().Trim() != "")
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                else
				    return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(FormatErrorMessage(context.DisplayName));
			}
		}
		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			_channelpartnerpropvalue = HttpContext.Current.Session["ChannelPartnerName"].ToString();
			var rule = new ModelClientValidationRule();
			rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
			rule.ValidationType = "channelpartner";
			rule.ValidationParameters.Add("channelpartner", _channelpartnervalue);
			rule.ValidationParameters.Add("channelpartnerpropvalue", _channelpartnerpropvalue);
			yield return rule;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class RequiredIfNotChannelPartnerAttribute : ValidationAttribute, IClientValidatable
	{
		private string _channelPartnerName;

		public RequiredIfNotChannelPartnerAttribute(string channelpartnername) 
		{
			_channelPartnerName = channelpartnername;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var channelpartner = validationContext.ObjectType.GetProperty("ChannelPartnerName");
			var currentChannelPartner = channelpartner.GetValue(validationContext.ObjectInstance, null);

			if (currentChannelPartner != null && string.Compare(Convert.ToString(currentChannelPartner), _channelPartnerName, true) == 0)
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			_channelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
			var rule = new ModelClientValidationRule();
			rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
			rule.ValidationType = "requiredifnotchannelpartner";
			rule.ValidationParameters.Add("channelpartnername", _channelPartnerName);
			yield return rule;
		}
	}

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CouponPromoAliasAttribute : ValidationAttribute, IClientValidatable
    {
        private string _basePropertyName;
        private const string _defaultErrorMessage = "Please enter valid Coupon/Promo/Alias Code";

        public CouponPromoAliasAttribute(string baseProperty)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = baseProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var code = context.ObjectType.GetProperty(_basePropertyName.ToString());
            var CouponPromoCode = Convert.ToString(value);
            var regexItem = new Regex("^[a-zA-Z0-9-]*$");
            var regex2 = new Regex("^[a-zA-Z]*$");
            var regex3 = new Regex("^[0-9]");
            var regex4 = new Regex("^[a-zA-Z0-9]*$");

            if (CouponPromoCode == null || CouponPromoCode.Trim() == "")
                return ValidationResult.Success;

            if (regexItem.IsMatch(CouponPromoCode) && (CouponPromoCode.Length >= 3 || CouponPromoCode.Length <= 20))
            {
                string cpcode = CouponPromoCode.ToString();
                if (cpcode.Contains("-") && cpcode.Length == 15)
                {
                    if (regex2.IsMatch(cpcode.Substring(0, 1)) && regex3.IsMatch(cpcode.Substring(1, 4)) && cpcode.Substring(5, 1).Equals("-"))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
                else if (regexItem.IsMatch(cpcode) && cpcode.Length == 5)
                {
                    if (regex2.IsMatch(cpcode.Substring(0, 1)) && regex3.IsMatch(cpcode.Substring(1, 4)))
                    {
                        return ValidationResult.Success;
                    }
                }
                if ((cpcode.Length >= 3 || cpcode.Length <= 20) && regex4.IsMatch(cpcode) && regex2.IsMatch(cpcode.Substring(0, 1)))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "couponpromoaliascode";
            rule.ValidationParameters.Add("couponpromoalias", _basePropertyName);
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SendMoneyAttribute : ValidationAttribute, IClientValidatable
    {
        private string _basePropertyName;
        private const string _defaultErrorMessage = "Please enter an amount in the Amount in (USD) or Destination Amount fields";
        public SendMoneyAttribute(string baseProperty)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = baseProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var code = context.ObjectType.GetProperty(_basePropertyName.ToString());
            var Amount1 = code.GetValue(context.ObjectInstance, null);
            var Amount2 = Convert.ToString(value);

            if (!((Amount1 == null || Convert.ToString(Amount1) == "") && (Amount2 == null || Amount2 == "")))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "sendmoneyamount";
            rule.ValidationParameters.Add("sendmoneyamount", _basePropertyName);
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ConditionalTestQuestionAttribute : ValidationAttribute, IClientValidatable
    {
        private string _basePropertyName;
        private const string _defaultErrorMessage = "Please enter Test Question Answer.";
        public ConditionalTestQuestionAttribute(string testQuestionOption)
            : base(_defaultErrorMessage)
        {
            _basePropertyName = testQuestionOption;
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var code = context.ObjectType.GetProperty(_basePropertyName.ToString());
            var prop = code.GetValue(context.ObjectInstance, null);
            string str = Convert.ToString(prop);
            if (str.ToLower() == "y" && value == null)
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "testquestion";
            rule.ValidationParameters.Add("testquestion", _basePropertyName);
            yield return rule;
        }
    }

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class ConditionalReceiverHasPhotoIDAttribute : ValidationAttribute, IClientValidatable
	{
		private string _basePropertyName;
		private const string _defaultErrorMessage = "Please enter Test Question Answer.";
		public ConditionalReceiverHasPhotoIDAttribute(string photoIDOption)
			: base(_defaultErrorMessage)
		{
			_basePropertyName = photoIDOption;
		}
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var code = context.ObjectType.GetProperty(_basePropertyName.ToString());
			var prop = code.GetValue(context.ObjectInstance, null);
			bool val = Convert.ToBoolean(prop);

			string propertyVal = (value != null)? value.ToString() : string.Empty ;

			if (!val && string.IsNullOrEmpty(propertyVal))
			{
				var x =  new ValidationResult(FormatErrorMessage(context.DisplayName));
				return x;
			}
			else
			{
				return ValidationResult.Success;
			}			
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule = new ModelClientValidationRule();
			rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
			rule.ValidationType = "photoidtestquestion";
			rule.ValidationParameters.Add("photoidtestquestion", _basePropertyName);
			yield return rule;
		}
	}

    //Drop down is required if it is not empty
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DropDownRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public DropDownRequiredAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.DisplayName);
            rule.ValidationType = "dropdownrequired";
            rule.ValidationParameters.Add("other", 0);
            yield return rule;

        }
    }
}