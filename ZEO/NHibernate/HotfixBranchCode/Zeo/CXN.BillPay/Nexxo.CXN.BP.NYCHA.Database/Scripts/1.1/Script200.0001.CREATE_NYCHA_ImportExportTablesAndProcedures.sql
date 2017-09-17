--===========================================================================================
-- Auther:			Bijo James
-- Date Created:	1/13/2014
-- Description:		Script for Create NYCHA Import and export Tables and Procedures
--===========================================================================================
--create [tNYCHAFiles] for process file details
CREATE TABLE [dbo].[tNYCHAFiles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NYCHAFilePath] [nvarchar](max) NOT NULL,
	[NYCHAFileName] [nvarchar](500) NOT NULL,
	[HCreateDate] [datetime] NULL,
	[HCreateTime] [nvarchar](100) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[Status] [bit] NULL,
	[FileType] [char](1) NULL,
	[LCKBATCHNO] [nvarchar](10) NULL,
	[LCKJOBNO] [nvarchar](10) NULL,
	[LCKOPERNO] [nvarchar](10) NULL,
	[LCKSEQNO] [nvarchar](10) NULL,
 CONSTRAINT [PK_tNYCHAFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tNYCHAFiles] ADD  CONSTRAINT [DF_tNYCHAFiles_Status]  DEFAULT ((0)) FOR [Status]
GO

--create [tNYCHAPayments] to export NYCHA Payments
CREATE TABLE [dbo].[tNYCHAPayments](
	[Id] [uniqueidentifier] NOT NULL,
	[SessionId] [int] NULL,
	[AgentId] [int] NULL,
	[LocationId] [uniqueidentifier] NULL,
	[ProductId] [int] NULL,
	[PAN] [bigint] NULL,
	[TenantId] [nvarchar](20) NULL,
	[AccountNumber] [nvarchar](20) NULL,
	[PaymentTypeId] [int] NULL,
	[PaymentStatusId] [int] NULL,
	[PaymentAmount] [money] NULL,
	[FeeRate] [money] NULL,
	[Note] [nvarchar](max) NULL,
	[CXNId] [bigint] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[DTPaymentSent] [datetime] NULL,
 CONSTRAINT [PK_tNYCHAPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

--create [tNYCHATenant] to maintain Tenent details 
CREATE TABLE [dbo].[tNYCHATenant](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NychaFileID] [bigint] NOT NULL,
	[TenantID] [nvarchar](20) NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[Active] [bit] NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tNYCHATenant] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[tNYCHATenant]  WITH CHECK ADD  CONSTRAINT [FK_tNYCHATenant_tNYCHAFiles] FOREIGN KEY([NychaFileID])
REFERENCES [dbo].[tNYCHAFiles] ([ID])
GO

ALTER TABLE [dbo].[tNYCHATenant] CHECK CONSTRAINT [FK_tNYCHATenant_tNYCHAFiles]
GO
/****** Object:  StoredProcedure [dbo].[USP_AddNYCHADetails]    Script Date: 01/28/2014 10:52:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_AddFiledetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_AddFiledetails]
GO
--create [USP_AddFiledetails] to add file details 
CREATE PROCEDURE [dbo].[USP_AddFiledetails]  
 -- Add the parameters for the stored procedure here  
    @NychaFilePath nvarchar(max),  
    @NYCHAFileName nvarchar(500),   
    @HCreateDate datetime,   
    @HCreateTime nvarchar(100),      
    @Id bigint  out  ,
    @FileType char(1) = null,
	@LCKBATCHNO	nvarchar(20) = null,
	@LCKJOBNO	nvarchar(20) = null,
	@LCKOPERNO	nvarchar(20) = null,
	@LCKSEQNO	nvarchar(20) = null
AS  
BEGIN  
 BEGIN TRANSACTION;  
     
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
  Insert into tNYCHAFiles(NYCHAFilePath,NYCHAFileName, HCreateDate,HCreateTime,DTCreate,FileType,LCKBATCHNO,LCKJOBNO,LCKOPERNO,LCKSEQNO)  
  values (@NychaFilePath,@NYCHAFileName,@HCreateDate,@HCreateTime,getdate(),@FileType,@LCKBATCHNO,@LCKJOBNO,@LCKOPERNO,@LCKSEQNO)  
  --SET @Id = (Select ID from tNYCHAFiles where NYCHAFileName = @NYCHAFileName and  )  
  set @Id = @@IDENTITY  
    
   
    IF @@ERROR > 0  
    Begin  
        ROLLBACK TRANSACTION;   
        Set @Id = 0           
    End   
 ELSE  
 BEGIN   
  IF @@TRANCOUNT > 0   
  BEGIN  
  COMMIT TRANSACTION;    
  END  
 END  
  
END  
GO
/****** Object:  StoredProcedure [dbo].[USP_AddNYCHADetails]    Script Date: 01/28/2014 10:52:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_AddNYCHADetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_AddNYCHADetails]
GO
--create [USP_AddNYCHADetails] to import Tenent details from CSV file 
CREATE PROCEDURE [dbo].[USP_AddNYCHADetails]
		@recdts xml,
		@result int out
	AS
	BEGIN
		BEGIN TRANSACTION;
	  
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		
		IF EXISTS (select * from sys.objects where name = '#TempNYCHAFileTable' and type = 'u')
		drop table #TempNYCHAFileTable	
		
		Select NYCHAFile.Items.value('data(TenantId[1])','nvarchar(20)') as Tenantid,
			   NYCHAFile.Items.value('data(AccountNumber[1])','nvarchar(max)') as AccountNumber,	
			   NYCHAFile.Items.value('data(NychaFileid[1])','int') as 	NychaFileID,
			   NYCHAFile.Items.value('data(NychaFilename[1])','nvarchar(500)') as NychaFilename		      
			into #TempNYCHAFileTable
			from @recdts.nodes('/ArrayOfNYCHAFile/NYCHAFile') AS NYCHAFile(Items)
		
		Update NyT Set NyT.DTLastMod = GETDATE()
		From  dbo.tNYCHATenant NyT
		Inner join #TempNYCHAFileTable TempNycha 
		on NyT.TenantID = TempNycha.TenantID and 
		NyT.AccountNumber = TempNycha.AccountNumber
		
		DELETE TempNycha
		FROM #TempNYCHAFileTable TempNycha
		INNER JOIN dbo.tNYCHATenant NyT on NyT.TenantID = TempNycha.TenantID and 
		NyT.AccountNumber = TempNycha.AccountNumber	
		
		Declare @NychaFilename nvarchar(500)
		Declare @NychaFileID int
		set @NychaFilename = (select distinct NychaFilename from #TempNYCHAFileTable)
		set @NychaFileID = (select distinct NychaFileid from #TempNYCHAFileTable)
		
		INSERT into tNYCHATenant(TenantID,AccountNumber,NychaFileID,Active,DTCreate)
		SELECT  Tenantid,AccountNumber,NychaFileID,1,GETDATE() from #TempNYCHAFileTable	
			
		Update tNYCHAFiles set Status = 1 where NYCHAFileName = @NychaFilename and ID=@NychaFileID

		IF @@ERROR > 0
		Begin
			ROLLBACK TRANSACTION;
			set @result = 0
		End 
		ELSE 
		Begin 
		 If @@TRANCOUNT > 0
			BEGIN
			COMMIT TRANSACTION;
			set @result = 1
			END
		END 
	END

GO
/****** Object:  StoredProcedure [dbo].[USP_AddNYCHADetails]    Script Date: 01/28/2014 10:52:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_GetNYCHADetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_GetNYCHADetails]
GO
--create [USP_GetNYCHADetails] to get Process file details
CREATE PROCEDURE [dbo].[USP_GetNYCHADetails] 
		@NYCHAFilenm nvarchar(500),
		@result int out, 
		@IsFile bit out 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from 
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	BEGIN Try     
    
    --Check to see if the file exists 
    IF EXISTS(SELECT  NYCHAFileName from tNYCHAFiles where NYCHAFileName = @NYCHAFilenm and [Status] = 1 )
	BEGIN
	    SELECT  HCreateTime as [HCreateDate]  from  tNYCHAFiles where NYCHAFileName = @NYCHAFilenm and [Status] = 1 
		SET @IsFile = 1	 
	END  
	ELSE
	BEGIN	
		Select max(HCreateDate) as HCreateDate from tNYCHAFiles where [Status] = 1 ;
	    SET @IsFile = 0 
	END 
		SET @result = 1 	 
	END TRY
	BEGIN CATCH
	--SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_SEVERITY() AS ErrorSeverity
	--	   ,ERROR_STATE() AS ErrorState,ERROR_PROCEDURE() AS ErrorProcedure
	--	   ,ERROR_LINE() AS ErrorLine
	--	   ,ERROR_MESSAGE() AS ErrorMessage;
			SET @result = 0 
			SET @IsFile = 0 
    END CATCH
	 
END

GO
--===========================================================================================








