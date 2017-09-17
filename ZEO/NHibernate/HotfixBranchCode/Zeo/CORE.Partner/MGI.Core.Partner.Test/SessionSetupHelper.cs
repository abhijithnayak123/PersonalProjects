using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;

using Spring.Data.Core;

using MGI.Core.Partner.Data.Fees;

namespace MGI.Core.Partner.Test
{
	public static class SessionSetupHelper
	{
		public static Guid CreateCustomerAndAccount(AdoTemplate adoTemplate, long customerId, long accountId, long channelPartnerId)
		{
			Guid channelPartnerPK = (Guid)adoTemplate.ExecuteScalar(CommandType.Text, string.Format("select rowguid from tChannelPartners where id={0}", channelPartnerId));
			Guid customerRowguid = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tPartnerCustomers(rowguid,id,cxeid,channelPartnerId, dtcreate) values('{0}',{1},{1},'{2}', getdate())", customerRowguid, customerId, channelPartnerPK ) );
			adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tAccounts(rowguid,id,cxeid,cxnid,customerpk,ProviderId,dtcreate) values(newid(),{0},{0},{0},'{1}',1,getdate())", accountId, customerRowguid ) );
			return customerRowguid;
		}

		public static long SetupCustomerSession( AdoTemplate adoTemplate, Guid customerGuid, string agentId )
		{
			Guid customerSession = Guid.NewGuid();
			Guid agentPK = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tAgentSessions where agentid='{0}'", agentId ) );
			adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tCustomerSessions(customerSessionRowguid,AgentSessionPK,CustomerPK,DTCreate,DTStart) values('{0}','{1}','{2}',getdate(),getdate())", customerSession,agentPK,customerGuid ) );
			return (long)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select Id from tCustomerSessions where customerSessionRowguid='{0}'", customerSession ) );
		}

		public static long SetupAgentSession(AdoTemplate adoTemplate, string agentId, int channelPartnerId)
		{
			if ( (int)adoTemplate.ExecuteScalar( CommandType.Text, string.Format("select count(*) from tAgentSessions where agentid='{0}'",agentId) ) <= 0 )
			{
				// add location
				Guid locationId = Guid.NewGuid();
				adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tLocations(rowguid,LocationName,IsActive,address1,Address2,City,State,Zipcode, phoneNumber,dtcreate,DTLastMod,channelpartnerid,BankID,BranchID,TimeZoneID,NoOfCounterIDs) values('{0}','TCF Service Desk',1,'801 Marquette Avenue','Address','Minneapolis','MN','55402','1234567890',getdate(),getdate(),{1},'99','1','Central Standard Time',3)", locationId, channelPartnerId));

				Guid npsTerminalPK = Guid.NewGuid();
				adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tNpsTerminals(rowguid,Name,Description,IPAddress,Port,LocationPK,Status,dtcreate,DTLastMod,PeripheralServiceUrl,ChannelPartnerPK) values('{0}','nex','','123.456.789','123','{1}','awesome',getdate(),getdate(),'https://nps.nexxofinancial.com:18732/Peripheral/','10F2865B-DBC5-4A0B-983C-62E0A0574354')", npsTerminalPK, locationId));

				// add terminal
				Guid channelPartnerPK = (Guid)adoTemplate.ExecuteScalar(CommandType.Text, string.Format("select rowguid from tChannelPartners where id={0}", channelPartnerId));
				Guid terminalId = Guid.NewGuid();
				adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tTerminals(rowguid,IPAddress,LocationPK,NpsTerminalPK,ChannelPartnerPK,dtcreate) values('{0}','123.456.789','{1}','{2}','{3}',getdate())", terminalId, locationId, npsTerminalPK, channelPartnerPK));

				// add an agent session
				adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tAgentSessions(rowguid, agentid, terminalPK,dtcreate) values(newid(),'1234','{0}',getdate())", terminalId ) );

			}
			return (long)adoTemplate.ExecuteScalar( CommandType.Text, string.Format("select id from tAgentSessions where agentid='{0}'",agentId) );
		}

		public static void SetupBillPayTxn( AdoTemplate adoTemplate, long txnId, long customerSessionId, long accountId )
		{
			Guid csGuid = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select customerSessionRowguid from tCustomerSessions where Id={0}", customerSessionId ) );
			Guid acctGuid = (Guid)adoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tAccounts where Id={0}", accountId ) );
			adoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "insert tTxn_BillPay(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,Fee,Description,CXEState,CXNState, DTCreate) values(newid(),{0},{0},{0},'{1}','{2}',20,1,'whee',1,1,getdate())", txnId, csGuid, acctGuid ) );
		}

		public static void SetupFundsTxn(AdoTemplate adoTemplate, long txnId, long customerSessionId, long accountId)
		{
			Guid csGuid = (Guid)adoTemplate.ExecuteScalar(CommandType.Text, string.Format("select customerSessionRowguid from tCustomerSessions where Id={0}", customerSessionId));
			Guid acctGuid = (Guid)adoTemplate.ExecuteScalar(CommandType.Text, string.Format("select rowguid from tAccounts where Id={0}", accountId));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tTxn_Funds(txnRowguid,Id,CXEId,CXNId,CustomerSessionPK,AccountPK,Amount,BaseFee,DiscountApplied,Fee,Description,FundType,CXEState,CXNState, DTCreate) values(newid(),{0},{0},{0},'{1}','{2}',20,1,0,1,'whee',1,1,1,getdate())", txnId, csGuid, acctGuid));
		}

		public static void SetupCheckTypeDiscount(AdoTemplate adoTemplate, int channelPartnerId)
		{
			Guid channelPartnerPK = (Guid)adoTemplate.ExecuteScalar(CommandType.Text, string.Format("select rowguid from tChannelPartners where id={0}", channelPartnerId));
			string adjustmentName = "check type discount";
			Guid adjustmentPK = Guid.NewGuid();

			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, channelPartnerPK, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 3,'1,2,3',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));
		}
	}
}
