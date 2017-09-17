using MGI.Common.Sys;
using System;

namespace MGI.Cxn.MoneyTransfer.Contract
{
	public class MoneyTransferException : NexxoException
	{
		const int MONEYTRANSFER_EXCEPTION_MAJOR_CODE = 1005;

		public MoneyTransferException(int MinorCode, string Message, Exception innerException)
			: base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}

		public MoneyTransferException(int MinorCode, Exception innerException)
			: base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
		{
		}

		public MoneyTransferException(int MinorCode, string Message)
			: base(MONEYTRANSFER_EXCEPTION_MAJOR_CODE, MinorCode, Message)
		{
		}

		public static int RECEIVER_ALREADY_EXISTED = 2000;//DESC : Receiver already existed with first name & last name combination.
		public static int RECEIVER_NOT_EXISTED = 2001;//DESC : Receiver not existed
		public static int UNKNOWN = 2002; //DESC : Unknown Exception
		public static int ORIGINATING_COUNTRY_CODE_NOT_FOUND = 2003;
		public static int ORIGINATING_CURRENCY_CODE_NOT_FOUND = 2004;
		public static int DESTINATION_COUNTRY_CODE_NOT_FOUND = 2005;
		public static int DESTINATION_CURRENCY_CODE_NOT_FOUND = 2006;
		public static int SVC_CODE_NOT_FOUND = 2007;
		public static int CITYNAME_NOT_FOUND = 2008;
		public static int STATENAME_NOT_FOUND = 2009;
		public static int CUSTOMER_NAME_NOT_MATCH = 2010; //US1784 - WU Gold Card Name Matching
		public static int MODIFY_TRANSACTION_NOT_ALLOWED = 2011; //US1865 WU Modify Transaction
		public static int TRANSACTION_ALREADY_PAID = 2012;      //US1865 WU Modify Transaction
		public static int CHANNEL_PARTNER_NOT_FOUND = 2013;
		public static int SAVE_RECEIVER_FAILED = 2014;
		public static int INVALID_DICTIONARY_KEY = 2015;
		public static int PROVIDER_ERROR = 2016;
        public static int TRANSACTION_STATUS_CANCL = 2017;
        public static int TRANSACTION_STATUS_RECVD = 2018;
        public static int TRANSACTION_STATUS_REFND = 2019;
        public static int OK_FOR_AGENT = 2020;
        public static int OK_FOR_PICKUP = 2021;
	    public static int LOCATION_NOT_SET = 2022;
		public static int COUNTERID_NOT_FOUND = 2023;
		//		 Begin AL-471 Changes
		//       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
		//       Purpose: This method takes only ssn exception message. We have found with ssn we have only 3 exception and below are the one
		public static int INVALID_SOCIAL_SECURITY_NUMBER = 6008;
		public static int REQUIRES_SSN_ITIN = 7490;
		public static int MISSING_SSN_ITIN = 5050;
        	public static int TRANSALATED_COUNTRY_NOT_EXITS = 2024;
        	public static int TRANSALATED_DELIVERY_SERVICE_NOT_EXITS = 2025;
		public static int TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0425; //AL-2967
		public static int DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION = 0415;//AL-3175

		//		END AL-471 changes
	}
}
