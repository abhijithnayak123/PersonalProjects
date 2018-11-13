using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Customer.FIS.Data
{
    public class CxnFISEnum
    {

		public enum ConnectionsType
		{
			CNECT,
			PREPD
		}

		public enum TaxCode
		{
			F, // F Federal taxpayer identification number. 
			N, // N No taxpayer identification number. 
			S, // S Social security number. 
			T  // T Individual taxpayer identification number
		}

		//ConnectsDBTaxNbrSrch
		public enum Gender
		{
			F, //Female
			M  //Male
		}


		//ChannelKy	A key indicating the interface channel	Presumably set to 6, but will be confirmed by Synovus [1-6, each will have a different category] 
		//GetAppInfo
		public enum ChannelKy
		{
			//1, //Category1
			// 2, //Category2
			// 3, //Category3
			// 4, //Category4
			//  5, //Category5
			//  6 //Category6
		}

		//CICustTaxNbrSrch
		//CIS More to be Returned Indicator
		public enum CISReturnedIndicator
		{
			N, //No
			Y  //Yes
		}


		//The type of search to be performed.
		//P Partial search; a partial match of search criteria within a keyword will return a customer.
		//F Full search; a whole keyword must match the search criteria to return a customer. 
		public enum CISSearchType
		{
			P,    //P Partial search;
			F    //F Full search
		}


		//CIS Customer Type Indicator
		public enum CISCustomerTypeIndicator
		{
			//Indicates the type of customer to search for.
			I, //I Individual customer.
			O //O Organization/business customer. 
		}



		public enum CISCurrentStandardAddress
		{
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code. 

		}

		//CIS More to be Returned Indicator
		public enum CISMoretobeReturnedIndicator
		{
			Y,  //Y Yes, there is more data to be returned.
			N //N No, there is not more data to be returned. 
		}

		//CIS ATS Service Charge Indicator
		public enum CISATSServiceChargeIndicator
		{
			N, //No accounts are related.
			P, //Primary deposit accounts are related.
			B //Both primary and secondary deposit accounts are related. 
		}

		//CIS W-8 Certificate Indicator
		public enum CISW8CertificateIndicator
		{
			Blank, //Not a foreign resident.
			B, //W-8BEN; no certification date required.
			C, //W-8BEN; certification date and a U.S. taxpayer identification number (TIN) required.
			E, //W-8ECI; no certification date required.
			I, //W-8IMY; no certification date required.
			N, //Not a foreign resident.
			X, //W-8EXP; no certification date required.
			Y //Foreign resident using Form W-8 requiring recertification by 12/31/2000. 
		}

		//CIS Name/Address Line Code
		public enum CISAddressLineCode
		{
			N, //Name.            
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code.
		}

		//CIS Relationship Primary/Secondary Indicator
		public enum CISRelationshipIndicator
		{
			P, //Primary.
			S  //Secondary. 
		}

		//CIS Account Current Name/Address Line Code 1

		public enum CISAccountLineCode
		{
			N, //Name.
			//% Legal title.
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code. 
		}


		//CIS Name/Address Line Code 1
		public enum CISAddressLineCode1
		{
			N, //Name.
			//% Legal title.
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code.
		}

		//CIS Name/Address Line Code 2
		public enum CISAddressLineCode2
		{
			N, //Name.
			//% Legal title.
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code.
		}

		//CIS Name/Address Line Code 3
		public enum CISAddressLineCode3
		{
			N, //Name.
			//% Legal title.
			A, //Mailing information.
			S, //Street address.
			C //City, state, ZIP code.
		}

		public enum ReqSvcParmsApplID
		{
			CI
		}
		
		public enum ReqSvcParmsSvcID
		{
			CIOpenMiscAcct,
			CICustNameAddrMaint,
			CIOpenIndvCust,
			CICustTaxNbrSrch,			
		}

		public enum ReqSvcParmsSvcVer
		{
			Item30,
			Item40,
			Item60
		}



    }
}
