--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-03-2016>
-- Description:	This SP added for Add/Edit receiver using Import button functionality in SendMoney module.
-- Import Functionality :- When the sender has used the WU service before registering into Alloy. He can Import the past receivers by clicking on "Import" button.
--						 Past receivers will be imported using the sender's (logged in customer's) gold card number.
-- Jira ID:		<AL-8324>

/*
EXEC usp_ImportReceivers
'<DocumentElement>
  <Receivers>
    <NameType>D</NameType>
    <FirstName>ERIN</FirstName>
    <LastName>JUNG</LastName>
	<SecondLastName>Test3</SecondLastName>
    <ReceiverIndexNo>002</ReceiverIndexNo>
    <Status>Active</Status>
    <CustomerId>1000000000000020</CustomerId>
    <GoldCardNumber>500585701</GoldCardNumber>
    <DTTerminalDate>2016-12-01T19:02:27.601082+05:30</DTTerminalDate>
    <DTServerDate>2016-12-01T19:02:27.601082+05:30</DTServerDate>
  </Receivers>
  <Receivers>
    <NameType>D</NameType>
    <FirstName>Abhi</FirstName>
    <LastName>Nayak</LastName>
	<SecondLastName>AA</SecondLastName>
    <ReceiverIndexNo>003</ReceiverIndexNo>
    <Status>Active</Status>
    <CustomerId>1000000000000020</CustomerId>
    <GoldCardNumber>500585701</GoldCardNumber>
    <DTTerminalDate>2016-12-01T19:02:27.601082+05:30</DTTerminalDate>
    <DTServerDate>2016-12-01T19:02:27.601082+05:30</DTServerDate>
  </Receivers>
  <Receivers>
    <NameType>D</NameType>
    <FirstName>Abhijith</FirstName>
    <LastName>Nayak1</LastName>
	<SecondLastName>BB</SecondLastName>
    <ReceiverIndexNo>004</ReceiverIndexNo>
    <Status>Active</Status>
    <CustomerId>1000000000000020</CustomerId>
    <GoldCardNumber>500585712</GoldCardNumber>
    <DTTerminalDate>2016-12-01T19:02:27.601082+05:30</DTTerminalDate>
    <DTServerDate>2016-12-01T19:02:27.601082+05:30</DTServerDate>
  </Receivers>
</DocumentElement>'

*/
-- ================================================================================

IF OBJECT_ID(N'usp_ImportReceivers', N'P') IS NOT NULL
DROP PROC usp_ImportReceivers
GO


CREATE PROCEDURE usp_ImportReceivers
(
    @pastReceivers XML
)
AS
BEGIN
	
BEGIN TRY

	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = '#TempReceivers' AND TYPE = 'u')
	DROP TABLE #TempReceivers	
	
	SELECT
		[receiverTable].[receiverColumn].value('NameType[1]', 'VARCHAR(20)') AS 'NameType'
		,[receiverTable].[receiverColumn].value('FirstName[1]', 'VARCHAR(100)') AS 'FirstName'
		,[receiverTable].[receiverColumn].value('LastName[1]', 'VARCHAR(100)') AS 'LastName'
		,[receiverTable].[receiverColumn].value('SecondLastName[1]', 'VARCHAR(100)') AS 'SecondLastName'
		,[receiverTable].[receiverColumn].value('ReceiverIndexNo[1]', 'VARCHAR(5)') AS 'ReceiverIndexNo'
		--,[receiverTable].[receiverColumn].value('Address[1]', 'VARCHAR(250)') AS 'Address'
		,[receiverTable].[receiverColumn].value('CountryCode[1]', 'VARCHAR(50)') AS 'CountryCode'
		,[receiverTable].[receiverColumn].value('Status[1]', 'VARCHAR(20)') AS 'Status'
		,[receiverTable].[receiverColumn].value('PickupCountry[1]', 'VARCHAR(100)') AS 'PickupCountry'
		,[receiverTable].[receiverColumn].value('CustomerId[1]', 'BIGINT') AS 'CustomerId'
		,[receiverTable].[receiverColumn].value('GoldCardNumber[1]', 'VARCHAR(50)') AS 'GoldCardNumber'  -- In WUGoldCardNumberfield, Past Receivers will have of the senders WU Goldcardnumber.
		,[receiverTable].[receiverColumn].value('DTTerminalDate[1]', 'DATETIME') as 'DTTerminalDate'
		,[receiverTable].[receiverColumn].value('DTServerDate[1]', 'DATETIME') as 'DTServerDate'
	INTO #TempReceivers
	FROM @pastReceivers.nodes('/DocumentElement/Receivers') as [receiverTable]([receiverColumn])

	-- If the Past Receiver exists in our Alloy DB. And if the receiver details are updated in WU, then update the receiver details from WU to Alloy DB.
	UPDATE 
		tWUnion_Receiver
	SET
		DTTerminalLastModified = tr.DTTerminalDate
		, DTServerLastModified = tr.DTServerDate
		, Country = tr.PickupCountry
		, ReceiverIndexNo = tr.ReceiverIndexNo
		, SecondLastName = tr.secondLastName
	FROM tWUnion_Receiver wr
	INNER JOIN #TempReceivers tr ON wr.CustomerId = tr.CustomerId 
											AND wr.FirstName = tr.FirstName
											AND wr.LastName = tr.LastName 
	
	WHERE tr.ReceiverIndexNo IS NOT NULL
		
		--Check whether we need to map the second last name and pick country as we don't get both from WU !!
		--AND wr.SecondLastName = 
		--(
		--	CASE 
		--		WHEN tr.SecondLastName IS NOT NULL
		--		THEN tr.SecondLastName
		--		ELSE wr.secondLastName
		--	END
		--)
		--AND wr.PickupCountry =
		--(
		--	CASE 
		--		WHEN tr.SecondLastName IS NULL
		--		THEN tr.PickupCountry  -- Filter on pick up country if the second last name not there for the receiver from WU.
		--		ELSE wr.PickupCountry
		--	END
		--)

	INSERT INTO tWUnion_Receiver
	(	 [FirstName]
	   ,[LastName]
	   ,[SecondLastName]
	   ,[Status]
	   ,[Country]
	   ,[PickupCountry]
	   ,[CustomerId]
	   ,[DTServerCreate]
	   ,[ReceiverIndexNo]
	   ,[GoldCardNumber]
	   ,[DTTerminalCreate]
	 )
	 SELECT 
		 tr.FirstName
		,tr.LastName
		,tr.SecondLastName
		,tr.Status
		,tr.PickupCountry
		,tr.PickupCountry
		,tr.CustomerId
		,tr.DTServerDate
		,tr.ReceiverIndexNo
		,tr.GoldCardNumber
		,tr.DTTerminalDate
	FROM #TempReceivers tr
			LEFT JOIN tWUnion_Receiver wr ON wr.CustomerId = tr.CustomerId 
												AND wr.FirstName = tr.FirstName
												AND wr.LastName = tr.LastName 
	WHERE
		 tr.ReceiverIndexNo IS NOT NULL	
		 AND wr.CustomerId IS NULL
			AND wr.FirstName IS NULL
			AND wr.LastName IS NULL

			--Check whether we need to map the second last name and pick country as we don't get both from WU !!
			--AND wr.SecondLastName = 
			--(
			--	CASE 
			--		WHEN tr.SecondLastName IS NOT NULL
			--		THEN tr.SecondLastName
			--		ELSE wr.secondLastName
			--	END
			--)
			--AND wr.PickupCountry =
			--(
			--	CASE 
			--		WHEN tr.SecondLastName IS NULL
			--		THEN tr.PickupCountry  -- Filter on pick up country if the second last name not there for the receiver from WU.
			--		ELSE wr.PickupCountry
			--	END
			--)
  
END TRY
BEGIN CATCH
 	
 	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
