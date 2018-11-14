using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class MoneyTransferTransactionDetailsViewModel : TransactionDetailsViewModel
    {


        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTrasferTrxMTCN")]
        public string MTCN { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTransferTrxOriginalTransactionId")]
        public string OriginalTransactionID { get; set; }

        //[Range(typeof(Decimal), "0.01", "99999.99", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "SendMoneyAmountRegularExpression")]
        //[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DoddFrankConfirmationTransferAmount")]
        //public decimal TransferAmount { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTransferAmount")]
        public decimal TransferAmount { get; set; }


        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTransferTrxRecvName")]
        public string ReceiverName { get; set; }


        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTransferTrxRecvAddress")]
        public string ReceiverAddress { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTransferTrxSenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestQuestion")]
        public string TestQuestion { get; set; }


        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestAnswer")]
        public string TestAnswer { get; set; }

        //[Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CancelCategory")]
        //public string CancelCategory { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReasonforCancel")]
        public string ReasonforCancel { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CancelCategory")]
        public IEnumerable<SelectListItem> CancelCategory { get; set; }

        public string PayStatus { get; set; }
        
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyRefundStatus")]
        public string RefundStatus { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyRefundCategory")]
        public IEnumerable<SelectListItem> RefundCategory { get; set; }

       // [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReasonforRefundRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ReasonforRefund")]
        public string ReasonforRefund { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyOrderAmount")]
        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MoneyOrderAmountRequired")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{0,2})?$", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MoneyOrderAmountRegularExpression")]
       // [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyFirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyMiddleName")]
        public string MiddleName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyLastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneySecondLastName")]
        public string SecondLastName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestQuestion")]
        public string ModifiedTestQuestion { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTestAnswer")]
        public string ModifiedTestAnswer { get; set; }

        public string RecieverFirstName { get; set; }

        public string RecieverMiddleName { get; set; }

        public string RecieverLastName { get; set; }

         [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "SendMoneyRecvSecLastName")]
        public string RecieverSecondLastName { get; set; }

         public string TestQuestionAvailable { get; set; }

         public string TransactionSubType { get; set; }
       
         public bool isModifiedOrRefunded { get; set; }
         public bool isAddedtoShoppingCart{ get; set; }
		 public string RefundStatusDesc { get; set; }

		 public string PayStatusCodeMessage { get; set; }

         public string FeeRefund { get; set; }

         [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MoneyTrasferTrxRefNo")]
         public string ReferenceNumber { get; set; }

		 public bool isAgentAgree { get; set; }
		 public bool isCashierState { get; set; }
		 public string AgentFirstName { get; set; }
		 public string AgentLastName { get; set; }
    }
}