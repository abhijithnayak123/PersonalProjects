--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07-22-2016>
-- Description:	 Create table to log the erros
-- Jira ID:		<AL-7582>
-- ================================================================================
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tErrorLog]') AND type in (N'U'))
DROP TABLE tErrorLog
GO

CREATE TABLE tErrorLog(
	Id BIGINT IDENTITY(1,1) NOT NULL,
	ErrorNumber INT NOT NULL,
	ErrorSeverity INT NULL,
	ErrorState INT NULL,
	ErrorProcedure NVARCHAR(500) NULL,
	ErrorLine INT NULL,
	ErrorMessage NVARCHAR(max) NOT NULL,
	DTServerCreate DATETIME NULL,
 CONSTRAINT PK_tErrorLog PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
