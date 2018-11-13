-- ============================================================
-- Author:		Abhijith
-- Create date: <10/20/2014>
-- Description:	<Script for insert for Legal Codes>
-- Rally ID:	<US2157>
-- ============================================================

INSERT INTO [dbo].[tLegalCodes]
(	
	[rowguid]
    ,[Code]
    ,[Name]
    ,[DTCreate]
)
VALUES
 ( NEWID(),'U','US Citizen',GETDATE())
,( NEWID(),'N','Non Resident Alien',GETDATE())
,( NEWID(),'R','Resident Alien',GETDATE())

GO
