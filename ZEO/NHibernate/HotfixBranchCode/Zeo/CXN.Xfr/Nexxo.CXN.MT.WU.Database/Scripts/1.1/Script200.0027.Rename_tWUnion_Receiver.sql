IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUReceiver]'))
BEGIN
	exec sp_rename 'tWUReceiver', 'tWUnion_Receiver'
END
GO
