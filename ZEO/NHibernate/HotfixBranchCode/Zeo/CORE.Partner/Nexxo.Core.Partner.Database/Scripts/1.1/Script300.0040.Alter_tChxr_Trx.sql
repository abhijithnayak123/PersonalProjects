--===========================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <Dec 12 2014>
-- Description:	<Increasing the Message Length>
-- Rally ID:	<DE3434>
--===========================================================================================

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE NAME = N'Message' AND OBJECT_ID = OBJECT_ID(N'tChxr_Trx'))
BEGIN
	ALTER TABLE tChxr_Trx
	ALter Column Message nvarchar(1000)
END
GO