using System;

namespace MGI.Cxn.Fund.Data
{
	public class ProcessorResult
	{
		public string ErrorCode { get; set; }

		public string ErrorMessage { get; set; }

		public Exception Exception { get; set; }

		public bool IsSuccess { get; set; }

        public string TransactionType { get; set; }
        public string ConfirmationNumber { get; set; }

		public ProcessorResult()
		{
		}

		public ProcessorResult(bool isSuccess)
		{
			IsSuccess = isSuccess;
		}

		public ProcessorResult(string errorCode, bool isSuccess)
		{
			ErrorCode = errorCode;
			IsSuccess = isSuccess;
		}

		public ProcessorResult(string errorCode, string errorMessage, bool isSuccess, string TransactionType, string ConfirmationNumber)
		{
			if (isSuccess)
			{
				IsSuccess = isSuccess;
                this.ConfirmationNumber = ConfirmationNumber;
                this.TransactionType = TransactionType;
				ErrorCode = string.Empty;
			}
			else
			{
				ErrorCode = errorCode;
				ErrorMessage = errorMessage;
				IsSuccess = isSuccess;
			}
		}

		public ProcessorResult(string errorCode, string errorMessage, bool isSuccess, Exception exception)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
			IsSuccess = isSuccess;
			Exception = exception;
		}
	}
}
