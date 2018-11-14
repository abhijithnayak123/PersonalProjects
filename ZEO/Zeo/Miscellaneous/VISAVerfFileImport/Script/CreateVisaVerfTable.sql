/****** Object:  Table [dbo].[VISAVerfRecords]    Script Date: 1/12/2016 5:55:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS(Select * from sys.tables t where t.name = 'VISAVerfRecords')
	BEGIN
	DECLARE @time varchar(50)
	DECLARE @tbl varchar(50)
	select @time = Convert(varchar(20),GETDATE(),112)+REPLACE(Convert(varchar(20),GETDATE(),108),':','')
	select @tbl = 'VISAVerfRecords'+ @time
	exec sp_rename 'VISAVerfRecords',  @tbl
END
GO
--//ALIAS_ID, EXTERNAL_KEY, PSEUDO_DDA, EXPIRATION_DATE,PAN

IF NOT EXISTS(Select * from sys.tables t where t.name = 'VISAVerfRecords')
	BEGIN
		CREATE TABLE [dbo].[VISAVerfRecords]
		(
			[AliasID] [nvarchar](19) NULL,
			[ProxyID] [nvarchar](19) NULL,
			[PseudoDDA] [nvarchar](28) NULL,
			[ExpirationDate] [datetime] NULL,
			[PrimaryAccountNumber] [nvarchar](19) NULL
		)
	END

--/****** Object:  Table [dbo].[VISAVerfRecords]    Script Date: 1/12/2016 5:55:10 PM ******/
--IF NOT EXISTS(Select * from sys.tables t where t.name = 'VISAVerfRecords')
--	BEGIN
--		CREATE TABLE [dbo].[VISAVerfRecords](
--			[RecordType] [nvarchar](10) NULL,
--			[CardProgramType] [nvarchar](1) NULL,
--			[CardProgramID] [nvarchar](16) NULL,
--			[CardTypeID] [nvarchar](1) NULL,
--			[SubClientIdentifier] [nvarchar](16) NULL,
--			[ReportingGroupID] [nvarchar](10) NULL,
--			[AliasID] [nvarchar](19) NULL,
--			[PrimaryAccountNumber] [nvarchar](19) NULL,
--			[ProfileType] [nvarchar](2) NULL,
--			[RecordCreationDate] [datetime] NULL,
--			[BuyerAliasID] [nvarchar](19) NULL,
--			[CustomerIndicatorField] [nvarchar](1) NULL,
--			[DateofLastWebContact] [datetime] NULL,
--			[CompanyName] [nvarchar](50) NULL,
--			[IssuerCompanyID] [nvarchar](16) NULL,
--			[LegalFirstName] [nvarchar](20) NULL,
--			[MiddleName] [nvarchar](1) NULL,
--			[LegalLastName] [nvarchar](20) NULL,
--			[Suffix] [nvarchar](5) NULL,
--			[Title] [nvarchar](4) NULL,
--			[GovernmentIDType] [nvarchar](3) NULL,
--			[GovernmentIssuedID] [nvarchar](30) NULL,
--			[DriversLicenseStateProvince] [nvarchar](2) NULL,
--			[GovernmentIDCountry] [nvarchar](3) NULL,
--			[BirthDate] [datetime] NULL,
--			[Filler1] [nvarchar](9) NULL,
--			[Address1] [nvarchar](75) NULL,
--			[Address2] [nvarchar](75) NULL,
--			[Address3] [nvarchar](75) NULL,
--			[City] [nvarchar](40) NULL,
--			[StateProvince] [nvarchar](4) NULL,
--			[PostalCode] [nvarchar](14) NULL,
--			[CountryCode] [nvarchar](3) NULL,
--			[PrimaryPhoneNumber] [nvarchar](20) NULL,
--			[PrimaryPhoneNumberType] [nvarchar](1) NULL,
--			[TextMessageDeviceNumber] [nvarchar](20) NULL,
--			[EmailAddress] [nvarchar](80) NULL,
--			[BuyerPrimaryFundingAccountType] [nvarchar](2) NULL,
--			[FundingAccountNumber] [nvarchar](28) NULL,
--			[RoutingandTransitNumber] [nvarchar](9) NULL,
--			[CardStockID] [nvarchar](8) NULL,
--			[CardShippingType] [nvarchar](2) NULL,
--			[CardOrderType] [nvarchar](2) NULL,
--			[IssuanceType] [nvarchar](2) NULL,
--			[EmbossedMessage] [nvarchar](26) NULL,
--			[IssuanceDate] [datetime] NULL,
--			[ExpirationDate] [datetime] NULL,
--			[ActivationDate] [datetime] NULL,
--			[CardIssuedActive] [nvarchar](1) NULL,
--			[CardHolderPromotionCode] [nvarchar](16) NULL,
--			[InitialBalance] [nvarchar](12) NULL,
--			[InitialBalanceCurrency] [nvarchar](3) NULL,
--			[AvailableBalance] [nvarchar](12) NULL,
--			[AvailableBalanceCurrency] [nvarchar](3) NULL,
--			[SignofAvailableBalance] [nvarchar](1) NULL,
--			[LedgerBalance] [nvarchar](12) NULL,
--			[LedgerBalanceCurrency] [nvarchar](3) NULL,
--			[SignofLedgerBalance] [nvarchar](1) NULL,
--			[Status] [nvarchar](2) NULL,
--			[StatusLastUpdated] [datetime] NULL,
--			[EmployeeID] [nvarchar](25) NULL,
--			[PseudoDDA] [nvarchar](28) NULL,
--			[PrimaryCardAliasID] [nvarchar](19) NULL,
--			[AccountHolderAliasID] [nvarchar](19) NULL,
--			[StoredValueAccountAliasID] [nvarchar](19) NULL,
--			[CompanyWebURL] [nvarchar](50) NULL,
--			[PendingBalance] [nvarchar](12) NULL,
--			[PendingBalanceCurrency] [nvarchar](3) NULL,
--			[SignofPendingBalance] [nvarchar](1) NULL,
--			[ClientTrackingID] [nvarchar](12) NULL,
--			[UpgradePAN] [nvarchar](19) NULL,
--			[PreviousPAN] [nvarchar](19) NULL,
--			[ProxyID] [nvarchar](19) NULL,
--			[EmbossedMessage2] [nvarchar](26) NULL,
--			[OrderConfirmationNumber] [nvarchar](15) NULL,
--			[OrderBatchID] [nvarchar](18) NULL,
--			[BudgetingEnrolled] [nvarchar](1) NULL,
--			[RESERVED] [nvarchar](12) NULL,
--			[FeeScheduleID] [nvarchar](16) NULL,
--			[FormFactor] [nvarchar](8) NULL,
--			[AlternateFundingStatus] [nvarchar](1) NULL,
--			[Filler] [nvarchar](751) NULL,
--			[FileLoadNumber] [int] NULL DEFAULT (1),
--			[DtCreated] [datetime] NULL DEFAULT (getdate()),
--			[DtLastModified] [datetime] NULL DEFAULT (getdate())

--		) ON [PRIMARY]
--	END
GO