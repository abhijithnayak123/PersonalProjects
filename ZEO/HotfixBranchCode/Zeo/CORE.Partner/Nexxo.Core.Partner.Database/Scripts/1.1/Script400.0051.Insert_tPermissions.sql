--===========================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <Mar 3rd 2015>
-- Description:	<Script to insert a row for creating a permission for transactions unparking>              
-- Jira ID:	<AL-88>
--===========================================================================================

INSERT INTO [dbo].[tPermissions] ([rowguid], [Permission], [DTCreate])                                            
     VALUES('879ADF9D-ED97-4509-AD6D-85F9C9A0CB8F', 'CanUnparkTransactions', GETDATE())           
GO