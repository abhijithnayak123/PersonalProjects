 --===========================================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <01/05/2015>
-- Description:	<Update Error Message which has text ''Nexxo Financial/Nexxo''>
-- Rally ID:	
--===========================================================================================
 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = N'tMessageStore')
BEGIN
		update tMessageStore
		set Content = 'MoneyGram Transaction Declined..'
		where MessageKey = '1002.2001' 

		update tMessageStore
		set Content = 'Customer registration in MGi Alloy has failed'
		where MessageKey = '1001.1004' 

		update tMessageStore
		set Content = 'Exceeded MoneyGram Limit Check'
		where MessageKey = '1008.6005' 

		update tMessageStore
		set Content = 'Exceeded MoneyGram Limit Check'
		where MessageKey = '1008.6000' 

		update tMessageStore
		set Content = 'Exceeded MoneyGram Limit Check'
		where MessageKey = '1008.6001' 

		update tMessageStore
		set Content = 'Exceeded MoneyGram Limit Check'
		where MessageKey = '1008.6006' 

		update tMessageStore
		set AddlDetails = 'Receiver profile already exists in MoneyGram database'
		where MessageKey = '1005.2000' 
END