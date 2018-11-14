using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class WUEnums
	{
		
		public enum name_type
		{
			C,
			D,
			M
		}
		public enum yes_no_flag
		{
			Y,
			N,
		}

		public enum Transaction_type
		{
			Item,		
			WMN,			
			MOD,
			QQC,
			WMF,
			EPY,
			CCM,
			ENR,

			ERN,

			/// <remarks/>
			CCQ,

			/// <remarks/>
			CMN,

			/// <remarks/>
			CMF,

			/// <remarks/>
			CMW,

			/// <remarks/>
			AMO,

			/// <remarks/>
			TMO,

			/// <remarks/>
			CCW,

			/// <remarks/>
			GEN,

			/// <remarks/>
			NTF,

			/// <remarks/>
			MAL,

			/// <remarks/>
			TEL,

			/// <remarks/>
			OVL,

			/// <remarks/>
			SEC,

			/// <remarks/>
			FAC,

			/// <remarks/>
			WMO,

			/// <remarks/>
			CMO,

			/// <remarks/>
			ACM,

			/// <remarks/>
			PAY,

			/// <remarks/>
			CSC,

			/// <remarks/>
			TRN,

			/// <remarks/>
			EBM,

			/// <remarks/>
			CCS,

			/// <remarks/>
			PSA,

			/// <remarks/>
			PSU,

			/// <remarks/>
			OBP,

			/// <remarks/>
			PRA,

			/// <remarks/>
			PRM,

			/// <remarks/>
			PRU,

			/// <remarks/>
			CNV,

			/// <remarks/>
			TYP,

			/// <remarks/>
			PSD,

			/// <remarks/>
			WMB,
		}
		public enum name_prefix
		{
			Mr,
			Ms,
			Mrs,
			Miss,
			Dr,
			Professor,
			Sir,
			Madam,
			M,
			S,
			F
		}
		public enum name_suffix
		{
			Jr,
			Sr,
			I,
			II,
			III,
			Esq
		}
		public enum yes_no
		{
			Y,
			N,
			@true,
			@false,			
			Item0,
			Item1,
			Item,
		}
		
		public enum Payment_type
		{		
			Item,
			CreditCard,
			DebitCard,
			Cash,
			ACH,
			DebitCardInterchange,
			Adjustment,
			Reinstate,
			Refund,
			Cancel,
			RetailMoney,
			Split,
		}

		public enum modifysendmoneyrequestConfirmed_id
		{
			Y,
			N
		}
		
		
	}
}
