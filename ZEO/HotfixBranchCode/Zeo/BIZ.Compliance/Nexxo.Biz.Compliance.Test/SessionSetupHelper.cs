using System;
using System.Data;

using Spring.Data.Core;

namespace MGI.Biz.Compliance.Test
{
	public static class SessionSetupHelper
	{
		public static Guid InsertCompProgram(AdoTemplate ado, string name, bool runOFAC )
		{
			Guid guid = Guid.NewGuid();
			ado.ExecuteNonQuery( CommandType.Text, string.Format( "insert tCompliancePrograms(rowguid, Name, RunOFAC, DTCreate) values('{0}','{1}',{2},getdate())", guid, name, ( runOFAC ? 1 : 0 ) ) );
			return guid;
		}

		public static Guid InsertPartnerCustomer(AdoTemplate adoTemplate, long id )
		{
			Guid g = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text,
				string.Format( "insert tPartnerCustomers(rowguid,id,cxeid,dtcreate) values('{0}',{1},{1},getdate())", g, id ) );
			return g;
		}

		public static Guid InsertCustomerAccount(AdoTemplate adoTemplate, Guid custPK, long id, int provider )
		{
			Guid g = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text,
				string.Format( "insert tAccounts(rowguid,id,cxeid,cxnid,customerpk,providerid,dtcreate) values('{0}',{1},{1},{1},'{2}',{3},getdate())", g, id, custPK, provider ) );
			return g;
		}

		public static Guid SetupCustomerSession( AdoTemplate adoTemplate, Guid custPK, Guid agentSessPK )
		{
			Guid g = Guid.NewGuid();
			//Guid agentPK = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tAgentSessions where agentid='{0}'", agentId ) );
			adoTemplate.ExecuteNonQuery( CommandType.Text, 
				string.Format( @"insert tCustomerSessions(customerSessionRowguid,AgentSessionPK,CustomerPK,DTCreate,DTStart) 
								 values('{0}','{1}','{2}',getdate(),getdate())", g,agentSessPK,custPK) );
			return g;
		}

		public static Guid SetupAgentSession(AdoTemplate adoTemplate, string agentId, string stateCode, int channelPartnerId)
		{
			//if ( (int)adoTemplate.ExecuteScalar( CommandType.Text, string.Format("select count(*) from tAgentSessions where agentid='{0}'",agentId) ) <= 0 )
			//{
			// add location
			Guid locationId = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text, 
				string.Format( @"insert tLocations(rowguid,locationname,address1,phonenumber,channelpartnerid,dtcreate,isactive,state) 
									values('{0}','nex','nex','1234567890',{1},getdate(),1,'{2}')", locationId,channelPartnerId,stateCode ) );

			Guid chPrtPK = (Guid)adoTemplate.ExecuteScalar( CommandType.Text,
				string.Format( "select rowguid from tChannelPartners where id={0}", channelPartnerId ) );

			// add npsterminal
			Guid npsG = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text,
				string.Format( @"insert tNpsTerminals(rowguid,name,ipaddress,locationpk,status,dtcreate,ChannelPartnerPK)
								 values('{0}','bogus','10.111.109.11','{1}','on',getdate(),'{2}')", npsG,locationId,chPrtPK ) );

			// add terminal
			Guid terminalId = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text, 
				string.Format( @"insert tTerminals(rowguid,LocationPK,dtcreate,IPAddress,NPSTerminalPK,ChannelPartnerPK,name) 
								  values('{0}','{1}',getdate(),'10.111.109.10','{2}','{3}','sjskdjwkwkdfjkt')", 
					terminalId, locationId,npsG,chPrtPK ) );

			// add an agent session
			Guid sId = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text, 
				string.Format( "insert tAgentSessions(rowguid, agentid, terminalPK,dtcreate) values('{0}','{2}','{1}',getdate())", sId, terminalId, agentId ) );

			return sId;
			//}
			//return (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format("select rowguid from tAgentSessions where agentid='{0}'",agentId) );
		}

		//public static void SetupBillPayTxn( AdoTemplate adoTemplate, long txnId, long customerSessionId, long accountId )
		//{
		//    Guid csGuid = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select customerSessionRowguid from tCustomerSessions where Id={0}", customerSessionId ) );
		//    Guid acctGuid = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tAccounts where Id={0}", accountId ) );
		//    adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tTxn_BillPay(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,CXEState,CXNState, DTCreate) values(newid(),{0},{0},{0},'{1}','{2}',20,1,'whee',1,1,getdate())", txnId, csGuid, acctGuid ) );
		//}

		public static void SetupCheckTxn( AdoTemplate adoT, long txnId, Guid custSessPK, Guid acctPK, decimal amount, decimal fee )
		{
			SetupCheckTxn( adoT, txnId, custSessPK, acctPK, amount, fee, 4 );
		}

		public static void SetupCheckTxn( AdoTemplate adoT, long txnId, Guid custSessPK, Guid acctPK, decimal amount, decimal fee, int state )
		{
			adoT.ExecuteNonQuery( CommandType.Text, 
				string.Format( @"insert tTxn_Check(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,
													 CXEState,CXNState, DTCreate) 
								 values(newid(),{0},{0},{0},'{1}','{2}',{3},{4},'whee',{5},{5},getdate())", 
								txnId, custSessPK, acctPK, amount, fee, state ) );
		}

		public static void SetupCashInTxn( AdoTemplate adoT, long txnId, Guid custSessPK, Guid acctPK, decimal amount, decimal fee )
		{
			adoT.ExecuteNonQuery( CommandType.Text,
				string.Format( @"insert tTxn_Cash(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,
													 CXEState,CXNState, DTCreate, CashType) 
								 values(newid(),{0},{0},{0},'{1}','{2}',{3},{4},'whee',4,4,getdate(),1)",
								txnId, custSessPK, acctPK, amount, fee ) );
		}

		public static void SetupBPTxn( AdoTemplate adoT, long txnId, Guid custSessPK, Guid acctPK, decimal amount, decimal fee )
		{
			adoT.ExecuteNonQuery( CommandType.Text,
				string.Format( @"insert tTxn_BillPay(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,
													 CXEState,CXNState, DTCreate) 
								 values(newid(),{0},{0},{0},'{1}','{2}',{3},{4},'whee',4,4,getdate())",
								txnId, custSessPK, acctPK, amount, fee ) );
		}

        public static void SetupMOTxn(AdoTemplate adoT, long txnId, Guid custSessPK, Guid acctPK, decimal amount, decimal fee)
        {
            adoT.ExecuteNonQuery(CommandType.Text,
                string.Format(@"insert tTxn_MoneyOrder(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,
													 CXEState,CXNState, DTCreate) 
								 values(newid(),{0},{0},{0},'{1}','{2}',{3},{4},'whee',4,4,getdate())",
                                txnId, custSessPK, acctPK, amount, fee));
        }

		public static Guid SetupCXECustomer(AdoTemplate ado, long alloyId, string fName, string LName, DateTime DOB, string phone,
			string govtId, string occupation )
		{
			return SetupCXECustomer(ado, alloyId, fName, LName, DOB, phone, govtId, occupation, "", "", "", "", "");
		}
		public static Guid SetupCXECustomer(AdoTemplate ado, long alloyId, string fName, string LName, DateTime DateOfBirth, string phone, 
			string govtId, string occupation, string address1, string city, string state, string zip, string gender )
		{
			if (DateOfBirth == DateTime.MinValue)
				DateOfBirth = DateTime.Today.AddYears(-20);
			// setup customer to lookup
			Guid _customerPK = Guid.NewGuid();
			//ado.ExecuteNonQuery( CommandType.Text,
			//    string.Format( "INSERT tCustomers(rowguid,Id,DTCreate) VALUES('{0}',{1},getdate())", _customerPK, alloyId ) );
			ado.ExecuteNonQuery( CommandType.Text,
				string.Format( @"INSERT tCustomers(rowguid,Id,FirstName,LastName,DOB,Phone1,Address1,City,State,ZipCode,Gender,DTCreate) 
								VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',getdate())",
								_customerPK, alloyId, fName, LName, DateOfBirth, phone, address1, city, state, zip, gender));
			ado.ExecuteNonQuery( CommandType.Text,
				string.Format( "INSERT tCustomerEmploymentDetails(CustomerPK,Occupation,DTCreate) VALUES('{0}','{1}',getdate())", _customerPK, occupation ) );
			//if ( !string.IsNullOrEmpty( govtId ) )
			//{
			//	Guid idtypePK = (Guid)ado.ExecuteScalar( CommandType.Text, "select rowguid from tIdTypes where Id=110" );
			//	ado.ExecuteNonQuery( CommandType.Text,
			//		string.Format( "INSERT tCustomerGovernmentIdDetails(CustomerPK,IdTypeId,Identification,DTCreate) VALUES('{0}',110,'{1}',getdate())", _customerPK, govtId ) );
			//}
			return _customerPK;
		}

		public static void SetupFundsOutTxn( AdoTemplate adoT, int txnId, Guid custSessPK, Guid acctPK, int amount )
		{
			adoT.ExecuteNonQuery( CommandType.Text,
				string.Format( @"insert tTxn_Funds(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Description,
													 CXEState,CXNState, DTCreate, FundType) 
								 values(newid(),{0},{0},{0},'{1}','{2}',{3},'whee',4,4,getdate(),0)",
								txnId, custSessPK, acctPK, amount ) );
		}
	}
}
