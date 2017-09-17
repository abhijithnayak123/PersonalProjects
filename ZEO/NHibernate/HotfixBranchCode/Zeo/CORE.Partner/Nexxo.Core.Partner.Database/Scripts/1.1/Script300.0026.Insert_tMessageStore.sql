-- ============================================================
-- Author:		Sunil Shetty
-- Create date: <09/17/2014>
-- Description:	<Added value for Check declined reasons>
-- Rally ID:	<US1840>
-- ============================================================
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE name = N'tMessageStore')
	BEGIN
		IF NOT EXISTS(Select * from tMessageStore where MessageKey = '1002.4' and PArtnerPK = 1)
			BEGIN
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.4',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The image of your check could not be read, please rescan the check and try again.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.38',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. A problem occurred during the check processing. Please try again.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.51',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is not endorsed. Please endorse the back of the check and try again.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.52',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME.  Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.7',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. InGo Money has detemined this to be a duplicate check. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.37',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. InGo Money has detemined this to be a duplicate check. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.54',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. InGo Money has detemined this to be a duplicate check for UNIDOS. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.40',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative they are requesting more information from the customer.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.2',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The person who issued your check could not be contacted or verified. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.46',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is not completely filled out. Please return the check to the person who issued the check to completely fill out the check and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.47',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The person who issued your check could not be contacted or verified. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.25',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.18',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.01',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.31' ,1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.24',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.33',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.30',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.34',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.35',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.06',1,'0','YOUR CHECK CANNOT BE APPROVED. The check is out of date.',getdate(),'TBD','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.19',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.29',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.20',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.44',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.5',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.49',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.13',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The dollar amount in numbers and the dollar amount written in letters do not match. Please return the check to the person that issued it and have them correct the amounts.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.42',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is not endorsed by all parties. Please have all parties listed on the front of the check, endorse on the back of the check and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.41',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Another bank or financial institution has already processed this check. Please ask the other bank/financial institution to cancel their process, and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.12',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is not endorsed by the payee, or is endorsed incorrectly. Please endorse the check correctly and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.11',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is not signed by the maker or purchaser. Please return to the maker to have them sign the check and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.26',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check processing center is currently unavailable. Please resubmit your check during business hours.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.50',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The customer has reached their check cashing limit for today. Please have them come back tomorrow.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.14',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The date on the check is missing or incorrect. Please return to the check to the person that issued it to complete/correct the date and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.36',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.23',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.27',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.45',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The person who issued your check could not be contacted or verified. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.43',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The person who issued your check could not be contacted or verified. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.32',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Money Order can not be processed in the next 48hrs. Please have the customer come back in 48hrs.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.3',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please return the check to the customer. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.48',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please return the check to the customer. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.10',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. The check is post dated. Please return the check to the person who issued it to correct the date and resubmit.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.53',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. This check can only be cashed at a Regions Bank location.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.55',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.57',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.58',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.59',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
				INSERT INTO tmessageStore(Rowguid,MessageKey,PArtnerPK,Language,Content,DTCreate,AddlDetails,Processor) VALUES(NewID(),'1002.60',1,'0','YOUR CHECK CANNOT BE APPROVED AT THIS TIME. Please contact an Ingo Money customer service representative if you have further questions.',getdate(),'','Chexar')
			END
		END
		
		