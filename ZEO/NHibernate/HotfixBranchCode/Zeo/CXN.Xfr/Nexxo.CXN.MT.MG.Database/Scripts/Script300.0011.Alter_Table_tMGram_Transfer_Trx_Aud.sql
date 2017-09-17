--===========================================================================================
-- Auther:			Asha
-- Date Created:	20/10/2014
-- Description:		Alter tMGram_Transfer_Trx_Aud 
--===========================================================================================

	ALTER TABLE tMGram_Transfer_Trx_Aud 
	ADD 
	[SenderAddress4] [varchar](80) NULL,
	[ReceiverAddress4] [varchar](80) NULL,
	[AgentCheckNumber] [varchar](12) NULL,
	[AgentCheckAmount] [decimal](18,3) NULL,
	[AgentCheckAuthorizationNumber] [varchar](5) NULL,
	[CustomerCheckNumber] [varchar](12) NULL,
	[CustomerCheckAmount] [decimal](18,3) NULL,
	[OkForAgent] [bit] NULL,
	[TransactionStatus] [varchar](10) NULL,
	[DateTimeSent] [datetime] NULL,
	[OriginatingCountry] [varchar](10) NULL,
	[ValidIndicator] [bit] NULL,
	[IndicativeReceiveAmount] [decimal](18,3) NULL,
	[IndicativeReceiveCurrency] [varchar](10) NULL,
	[IndicativeExchangeRate] [decimal](18,4) NULL,
	[ReceiveAgentAbbreviation] [varchar](10) NULL,
	[OriginalSendAmount] [decimal](18,3) NULL,
	[OriginalSendCurrency] [varchar](10) NULL,
	[OriginalSendFee] [decimal](18,3) NULL,
	[OriginalExchangeRate] [decimal](18,4) NULL,
	[RedirectIndicator] [bit] NULL,
	[RedirectInfoOriginalSendAmount] [decimal](18,3) NULL,
	[RedirectInfoOriginalSendCurrency] [varchar](10) NULL,
	[RedirectInfoOriginalSendFee] [decimal](18,3) NULL,
	[RedirectInfoOriginalExchangeRate] [decimal](18,4) NULL,
	[RedirectInfoOriginalReceiveAmount] [decimal](18,3) NULL,
	[RedirectInfoOriginalReceiveCurrency] [varchar](10) NULL,
	[RedirectInfoOriginalReceiveCountry] [varchar](10) NULL,
	[RedirectInfoNewExchangeRate] [decimal](18,4) NULL,
	[RedirectInfoNewReceiveAmount] [decimal](18,3) NULL,
	[RedirectInfoNewReceiveCurrency] [varchar](10) NULL,
	[RedirectInfoRedirectType] [varchar](20) NULL,
	[OkForPickup] [bit] NULL,
	[NotOkForPickupReasonCode] [varchar](10) NULL,
	[NotOkForPickupReasonDescription] [varchar](1000) NULL,
	[MinutesUntilOkForPickup] [varchar](10) NULL,
	[ReceiveReferenceNumber] [varchar](20) NULL,
	[TimeStamp] [datetime] NULL