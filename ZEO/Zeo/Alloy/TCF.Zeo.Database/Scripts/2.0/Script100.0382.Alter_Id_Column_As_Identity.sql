--- ===============================================================================
-- Author:		<Vivek Swamy>
-- Create date: <03-06-2017>
-- Description:	Alter Id coulum as identity
-- ================================================================================


BEGIN TRY

	-----------------------============================------TABLE Modification-tChannelPartnerConfig----=========================================------------------------------
	-- Identity column modification for [dbo].[tChannelPartnerConfig]	
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tChannelPartnerConfig_Identity](
			[ChannelPartnerPK] [uniqueidentifier] NULL,
			[DisableWithdrawCNP] [bit] NOT NULL CONSTRAINT [DF_tChannelPartnerConfig_DisableWithdraw1]  DEFAULT ((0)),
			[CashOverCounter] [bit] NOT NULL CONSTRAINT [DF_tChannelPartnerConfig_CashOverCounter1]  DEFAULT ((0)),
			[FrankData] [varchar](200) NULL,
			[IsCheckFrank] [bit] NULL,
			[IsNotesEnable] [bit] NULL,
			[IsReferralSectionEnable] [bit] NOT NULL DEFAULT ((0)),
			[IsMGIAlloyLogoEnable] [bit] NOT NULL DEFAULT ('TRUE'),
			[MasterSSN] [nvarchar](15) NOT NULL CONSTRAINT [DF_tChannelPartnerConfig_MasterSSN1]  DEFAULT ('888888888'),
			[IsMailingAddressEnable] [bit] NOT NULL CONSTRAINT [DF_tChannelPartnerConfig_MailingAddressEnable1]  DEFAULT ((1)),
			[CanEnableProfileStatus] [bit] NOT NULL DEFAULT ((1)),
			[CustomerMinimumAge] [int] NULL DEFAULT ((18)),
			[ChannelPartnerID] [smallint] IDENTITY (1,1) NOT NULL,
		 CONSTRAINT [PK_tChannelPartnerConfig1] PRIMARY KEY CLUSTERED 
		(
			[ChannelPartnerID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tChannelPartnerConfig SWITCH TO tChannelPartnerConfig_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tChannelPartnerConfig]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tChannelPartnerConfig_Identity','tChannelPartnerConfig';

		 -- Add Constraints on Table
		ALTER TABLE [dbo].[tChannelPartnerConfig]  WITH CHECK ADD  CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners] FOREIGN KEY([ChannelPartnerID])
		REFERENCES [dbo].[tChannelPartners] ([ChannelPartnerId])

		ALTER TABLE [dbo].[tChannelPartnerConfig] CHECK CONSTRAINT [FK_tChannelPartnerConfig_tChannelPartners]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tChannelPartnerConfig1','PK_tChannelPartnerConfig';
		EXEC sp_rename 'DF_tChannelPartnerConfig_DisableWithdraw1','DF_tChannelPartnerConfig_DisableWithdraw';
		EXEC sp_rename 'DF_tChannelPartnerConfig_MasterSSN1','DF_tChannelPartnerConfig_MasterSSN';
		EXEC sp_rename 'DF_tChannelPartnerConfig_CashOverCounter1','DF_tChannelPartnerConfig_CashOverCounter';
		EXEC sp_rename 'DF_tChannelPartnerConfig_MailingAddressEnable1','DF_tChannelPartnerConfig_MailingAddressEnable';

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tChxr_Trx----=========================================------------------------------
	-- Identity column modification for [dbo].[tChxr_Trx]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tChxr_Trx_Identity](
			[ChxrTrxPK] [uniqueidentifier] NOT NULL,
			[ChxrTrxID] [bigint] IDENTITY (1,1) NOT NULL,
			[Amount] [money] NOT NULL,
			[ChexarAmount] [money] NULL,
			[ChexarFee] [money] NULL,
			[CheckDate] [datetime] NULL,
			[CheckNumber] [nvarchar](20) NULL,
			[RoutingNumber] [nvarchar](20) NULL,
			[AccountNumber] [nvarchar](20) NULL,
			[Micr] [nvarchar](50) NULL,
			[Latitude] [float] NULL,
			[Longitude] [float] NULL,
			[InvoiceId] [int] NULL,
			[TicketId] [int] NULL,
			[WaitTime] [nvarchar](50) NULL,
			[Status] [int] NOT NULL,
			[ChexarStatus] [nvarchar](50) NOT NULL,
			[DeclineCode] [int] NULL,
			[Message] [nvarchar](255) NULL,
			[Location] [nvarchar](50) NOT NULL,
			[ChxrAccountPK] [uniqueidentifier] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[SubmitType] [int] NULL,
			[ReturnType] [int] NULL,
			[ChannelPartnerID] [int] NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[IsCheckFranked] [bit] NOT NULL,
			[ChxrAccountId] [bigint] NOT NULL
		 CONSTRAINT [PK_tChxr_Trx1] PRIMARY KEY CLUSTERED 
		(
			[ChxrTrxID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tChxr_Trx SWITCH TO tChxr_Trx_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tChxr_Trx]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tChxr_Trx_Identity','tChxr_Trx';

		 -- Add Constraints on Table
		ALTER TABLE [dbo].[tChxr_Trx] ADD  CONSTRAINT [DF_tChxr_Trx_ChxrTrxPK]  DEFAULT (newid()) FOR [ChxrTrxPK]

		ALTER TABLE [dbo].[tChxr_Trx] ADD  DEFAULT ((0)) FOR [IsCheckFranked]

		ALTER TABLE [dbo].[tChxr_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tChxr_Trx_tChxr_Account] FOREIGN KEY([ChxrAccountId])
		REFERENCES [dbo].[tChxr_Account] ([ChxrAccountID])

		ALTER TABLE [dbo].[tChxr_Trx] CHECK CONSTRAINT [FK_tChxr_Trx_tChxr_Account]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tChxr_Trx1','PK_tChxr_Trx';


	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_BillPay----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_BillPay]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_BillPay_Identity](
			[TxnPK] [uniqueidentifier] NOT NULL,
			[TransactionID] [bigint] IDENTITY (1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[ProductId] [bigint] NULL,
			[AccountNumber] [nvarchar](50) NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[CustomerSessionId] [bigint] NOT NULL,
			[ProviderAccountId] [bigint] NOT NULL,
			[ProviderId] [int] NOT NULL,
			[CustomerRevisionNo] [int] NULL,
			
		 CONSTRAINT [PK_tTrx_BillPay1] PRIMARY KEY CLUSTERED 
		(
			[TransactionID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_BillPay SWITCH TO tTxn_BillPay_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_BillPay]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_BillPay_Identity','tTxn_BillPay';

		  -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_BillPay] ADD  CONSTRAINT [DF_tTxn_Billpay_txnPK]  DEFAULT (newid()) FOR [TxnPK]

		ALTER TABLE [dbo].[tTxn_BillPay]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
		REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])

		ALTER TABLE [dbo].[tTxn_BillPay] CHECK CONSTRAINT [FK_tTxn_BillPay_tCustomerSessions]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTrx_BillPay1','PK_tTrx_BillPay';

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_Cash----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_Cash]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_Cash_Identity](
			[TxnPK] [uniqueidentifier] NULL,
			[TransactionID] [bigint] IDENTITY (1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[CashType] [int] NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[CustomerSessionId] [bigint] NULL,
		 CONSTRAINT [PK_tTxn_Cash1] PRIMARY KEY CLUSTERED 
		(
			[TransactionID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_Cash SWITCH TO tTxn_Cash_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_Cash]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_Cash_Identity','tTxn_Cash';

		  -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_Cash]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Cash_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
		REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])

		ALTER TABLE [dbo].[tTxn_Cash] CHECK CONSTRAINT [FK_tTxn_Cash_tCustomerSessions]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTxn_Cash1','PK_tTxn_Cash';

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_Check----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_Check]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_Check_Identity](
			[TxnPK] [uniqueidentifier] NOT NULL,
			[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[BaseFee] [money] NULL,
			[DiscountApplied] [money] NULL,
			[AdditionalFee] [money] NULL,
			[DiscountName] [varchar](50) NULL,
			[DiscountDescription] [varchar](100) NULL,
			[IsSystemApplied] [bit] NOT NULL,
			[CustomerSessionId] [bigint] NOT NULL,
			[CustomerRevisionNo] [bigint] NULL,
			[ProviderId] [int] NOT NULL,
			[ProviderAccountId] [bigint] NOT NULL,
			[CheckType] [int] NULL,
			[MICR] [nvarchar](100) NULL,
		 CONSTRAINT [PK_tTxn_Check1] PRIMARY KEY CLUSTERED 
		(
			[TransactionID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		--Drop FK relationship from all Child Tables
		ALTER TABLE [dbo].[tCheckImages] DROP CONSTRAINT [FK_tCheckImages_tTxn_Check]

		ALTER TABLE [dbo].[tMessageCenter] DROP CONSTRAINT [FK_tMessageCenter_TxnId]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_Check SWITCH TO tTxn_Check_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_Check]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_Check_Identity','tTxn_Check';

		 -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_Check] ADD  CONSTRAINT [DF_tTxn_Check_TxnPK]  DEFAULT (newid()) FOR [TxnPK]

		ALTER TABLE [dbo].[tTxn_Check] ADD  DEFAULT ((0)) FOR [IsSystemApplied]

		ALTER TABLE [dbo].[tTxn_Check]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_Check_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
		REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])

		ALTER TABLE [dbo].[tTxn_Check] CHECK CONSTRAINT [FK_tTxn_Check_tCustomerSessions]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTxn_Check1','PK_tTxn_Check';

		--Recreate FK relationship with all Child Tables
		ALTER TABLE [dbo].[tCheckImages]  WITH CHECK ADD  CONSTRAINT [FK_tCheckImages_tTxn_Check] FOREIGN KEY([TransactionId])
		REFERENCES [dbo].[tTxn_Check] ([TransactionID])
		ALTER TABLE [dbo].[tCheckImages] CHECK CONSTRAINT [FK_tCheckImages_tTxn_Check]

		ALTER TABLE [dbo].[tMessageCenter]  WITH CHECK ADD  CONSTRAINT [FK_tMessageCenter_TxnId] FOREIGN KEY([TransactionId])
		REFERENCES [dbo].[tTxn_Check] ([TransactionID])
		ALTER TABLE [dbo].[tMessageCenter] CHECK CONSTRAINT [FK_tMessageCenter_TxnId]

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_Funds----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_Funds]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_Funds_Identity](
			[TxnPK] [uniqueidentifier] NOT NULL,
			[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[FundType] [int] NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[BaseFee] [money] NULL,
			[DiscountApplied] [money] NULL,
			[AdditionalFee] [money] NULL,
			[DiscountName] [varchar](50) NULL,
			[DiscountDescription] [varchar](100) NULL,
			[IsSystemApplied] [bit] NOT NULL,
			[AddOnCustomerId] [bigint] NULL,
			[CustomerSessionId] [bigint] NULL,
			[ProviderId] [int] NULL,
			[ProviderAccountId] [bigint] NULL,
			[CustomerRevisionNo] [int] NULL,
		 CONSTRAINT [PK_tTxn_Funds1] PRIMARY KEY CLUSTERED 
		(
			[TransactionId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_Funds SWITCH TO tTxn_Funds_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_Funds]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_Funds_Identity','tTxn_Funds';

		 -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_Funds] ADD  CONSTRAINT [DF_tTxn_Funds_PK]  DEFAULT (newid()) FOR [TxnPK]

		ALTER TABLE [dbo].[tTxn_Funds] ADD  DEFAULT ((0)) FOR [IsSystemApplied]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTxn_Funds1','PK_tTxn_Funds';

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_MoneyOrder----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_MoneyOrder]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_MoneyOrder_Identity](
			[TxnPK] [uniqueidentifier] NOT NULL,
			[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[BaseFee] [money] NULL,
			[DiscountApplied] [money] NULL,
			[AdditionalFee] [money] NULL,
			[DiscountName] [varchar](50) NULL,
			[DiscountDescription] [varchar](100) NULL,
			[IsSystemApplied] [bit] NULL,
			[CheckNumber] [varchar](50) NULL,
			[AccountNumber] [varchar](20) NULL,
			[RoutingNumber] [varchar](20) NULL,
			[CustomerRevisionNo] [int] NULL,
			[MICR] [varchar](500) NULL,
			[PurchaseDate] [datetime] NULL,
			[CustomerSessionId] [bigint] NOT NULL,
		 CONSTRAINT [PK_tTrxn_MoneyOrder1] PRIMARY KEY CLUSTERED 
		(
			[TransactionID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]


		--Drop FK relationship from all Child Tables
		ALTER TABLE [dbo].[tMoneyOrderImage] DROP CONSTRAINT [FK_tMoneyOrderImage_tTxn_MoneyOrder]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_MoneyOrder SWITCH TO tTxn_MoneyOrder_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_MoneyOrder]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_MoneyOrder_Identity','tTxn_MoneyOrder';

		  -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_MoneyOrder] ADD  CONSTRAINT [DF_tTxn_MoneyOrder_txnPK]  DEFAULT (newid()) FOR [TxnPK]

		ALTER TABLE [dbo].[tTxn_MoneyOrder] ADD  DEFAULT ((0)) FOR [IsSystemApplied]

		ALTER TABLE [dbo].[tTxn_MoneyOrder]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
		REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])

		ALTER TABLE [dbo].[tTxn_MoneyOrder] CHECK CONSTRAINT [FK_tTxn_MoneyOrder_tCustomerSessions]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTrxn_MoneyOrder1','PK_tTrxn_MoneyOrder';

		--Recreate FK relationship with all Child Tables
		ALTER TABLE [dbo].[tMoneyOrderImage]  WITH CHECK ADD  CONSTRAINT [FK_tMoneyOrderImage_tTxn_MoneyOrder] FOREIGN KEY([TransactionId])
		REFERENCES [dbo].[tTxn_MoneyOrder] ([TransactionID])
		ALTER TABLE [dbo].[tMoneyOrderImage] CHECK CONSTRAINT [FK_tMoneyOrderImage_tTxn_MoneyOrder]

	COMMIT TRAN

	-----------------------============================------TABLE Modification-tTxn_MoneyTransfer----=========================================------------------------------
	-- Identity column modification for [dbo].[tTxn_MoneyTransfer]
	BEGIN TRAN
	-- Create new table with Identity column, to be used with SWITCH TO

		CREATE TABLE [dbo].[tTxn_MoneyTransfer_Identity](
			[TxnPK] [uniqueidentifier] NULL,
			[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
			[CXEId] [bigint] NULL,
			[CXNId] [bigint] NULL,
			[CustomerSessionPK] [uniqueidentifier] NULL,
			[AccountPK] [uniqueidentifier] NULL,
			[Amount] [money] NULL,
			[Fee] [money] NULL,
			[Description] [nvarchar](255) NULL,
			[State] [int] NULL,
			[CXNState] [int] NULL,
			[DTTerminalCreate] [datetime] NOT NULL,
			[DTTerminalLastModified] [datetime] NULL,
			[ConfirmationNumber] [varchar](50) NULL,
			[RecipientId] [bigint] NULL,
			[ExchangeRate] [money] NULL,
			[DTServerCreate] [datetime] NULL,
			[DTServerLastModified] [datetime] NULL,
			[TransferType] [int] NULL,
			[TransactionSubType] [varchar](20) NULL,
			[OriginalTransactionID] [bigint] NULL,
			[CustomerSessionId] [bigint] NOT NULL,
			[CustomerRevisionNo] [bigint] NULL,
			[ProviderId] [BIGINT] NOT NULL,
			[ProviderAccountId] [bigint] NOT NULL,
			[Destination] [nvarchar](200) NULL,
		 CONSTRAINT [PK_tTxn_MoneyTransfer1] PRIMARY KEY CLUSTERED 
		(
			[TransactionID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

		-- SWITCH TO NEW TABLE
		ALTER TABLE tTxn_MoneyTransfer SWITCH TO tTxn_MoneyTransfer_Identity;

		-- drop the original (now empty) table
		DROP TABLE [dbo].[tTxn_MoneyTransfer]

		 -- rename new table to old table's name
		 EXEC sp_rename 'tTxn_MoneyTransfer_Identity','tTxn_MoneyTransfer';

		  -- Add Constraints on Table
		ALTER TABLE [dbo].[tTxn_MoneyTransfer]  WITH CHECK ADD  CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions] FOREIGN KEY([CustomerSessionId])
		REFERENCES [dbo].[tCustomerSessions] ([CustomerSessionID])

		ALTER TABLE [dbo].[tTxn_MoneyTransfer] CHECK CONSTRAINT [FK_tTxn_MoneyTransfer_tCustomerSessions]

		-- Rename constraints to original name
		EXEC sp_rename 'PK_tTxn_MoneyTransfer1','PK_tTxn_MoneyTransfer';

	COMMIT TRAN

	-- Change seed value for Identity columns
	BEGIN TRAN

		DECLARE @maxidentity BIGINT

		SELECT @maxidentity = ISNULL(max(TransactionID),1000000000)+1 FROM tTxn_Cash
		DBCC CHECKIDENT('tTxn_Cash', RESEED,@maxidentity)
		
		SELECT @maxidentity = ISNULL(max(TransactionID),1000000000)+1 FROM tTxn_Check
		DBCC CHECKIDENT('tTxn_Check', RESEED,@maxidentity)

		DBCC CHECKIDENT('tChannelPartnerConfig', RESEED)
		DBCC CHECKIDENT('tTxn_BillPay', RESEED,2000000000)
		DBCC CHECKIDENT('tTxn_Funds', RESEED,6000000000)
		DBCC CHECKIDENT('tTxn_MoneyOrder', RESEED,5000000000)
		DBCC CHECKIDENT('tTxn_MoneyTransfer', RESEED,3000000000)
		DBCC CHECKIDENT('tChxr_Trx', RESEED,200000000000)
		DBCC CHECKIDENT('tVisa_Trx', RESEED,103000000000)
		DBCC CHECKIDENT('tWUnion_BillPay_Trx', RESEED,401000000000)
		DBCC CHECKIDENT('tWUnion_Trx', RESEED,301000000000)

	COMMIT TRAN

END TRY
 
BEGIN CATCH
 IF(@@TRANCOUNT > 0)
	SELECT
    ERROR_NUMBER() AS ErrorNumber
    ,ERROR_SEVERITY() AS ErrorSeverity
    ,ERROR_STATE() AS ErrorState
    ,ERROR_PROCEDURE() AS ErrorProcedure
    ,ERROR_LINE() AS ErrorLine
    ,ERROR_MESSAGE() AS ErrorMessage,
    XACT_STATE()as state;
    ROLLBACK TRAN
END CATCH
GO