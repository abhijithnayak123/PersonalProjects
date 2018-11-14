--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Created date: <06/24/2015>
-- Description:	<Script to create new table tVisa_CardClass,tVisa_ShippingFee and Inserting values>           
-- Jira ID:	<AL-244>
--===========================================================================================

CREATE TABLE [dbo].[tVisa_CardClass](
	[VisaCardClassPK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[StateCode] VARCHAR(5) NOT NULL ,
	[CardClass] INT  NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	CONSTRAINT IX_tVisa_CardClass_StateCode UNIQUE NONCLUSTERED(StateCode)
	)

BEGIN
	INSERT INTO [dbo].[tVisa_CardClass]
		([VisaCardClassPK],[StateCode],[CardClass],[DTServerCreate],[DTServerLastModified]) 
	VALUES
		(NewID(),'MI',3,getdate(),Null),
		(NewID(),'CO',5,getdate(),Null),
		(NewID(),'AZ',7,getdate(),Null),
		(NewID(),'WI',9,getdate(),Null),
		(NewID(),'ILIN',11,getdate(),Null),
		(NewID(),'MN',13,getdate(),Null)
	END
GO

CREATE TABLE [dbo].[tVisa_ShippingFee](
	[VisaShippingFeePK] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000,1) NOT NULL,
	[ShippingType] INT NOT NULL ,
	[Fee] MONEY  NOT NULL,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	CONSTRAINT IX_tVisa_ShippingFee_ShippingType UNIQUE (ShippingType)
	)
BEGIN
	INSERT INTO [dbo].[tVisa_ShippingFee]
		([VisaShippingFeePK],[ShippingType],[Fee],[DTServerCreate],[DTServerLastModified]) 
	VALUES
		(NewID(),0,24.95,getdate(),Null),
		(NewID(),2,0.00,getdate(),Null)
	END
GO


