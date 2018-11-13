-- ============================================================
-- Author:		<Ashok Kumar G>
-- Create date: <10/30/2014>
-- Description:	<Script for insert data into tProducts, tProcessors, tProductProcessorsmapping, tChannelPartnerProductProcessorsMapping tables.>
-- Rally ID:	<US2166>
-- ============================================================

-- Insert Script for tProducts
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('21677DF0-58E3-4C47-A9C3-6EF651ECCDB8', 'ProcessCheck', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('A3EB4C6E-F28C-4DB0-A2C6-DDDEC671D2DF', 'BillPayment', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('10D73929-52D5-4D5E-956F-0771526A3C42', 'MoneyTransfer', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('FE641EB0-68FD-4667-853E-442003023A72', 'ReceiveMoney', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('2606D541-9613-456D-83BA-FFB810E0E417', 'MoneyOrder', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('8F2FA74E-C135-4028-8F4D-36A57F46CF6E', 'ProductCredential', GETDATE())
INSERT INTO tProducts (rowguid, Name, DTCreate) VALUES('8863DF03-5FA7-404A-B142-F38B6A060E21', 'TransactionHistory', GETDATE())
GO
-- Insert Script for tProcessors 
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('ACC748AE-659F-4304-AFC4-45C0403A2E40', 'INGO', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('AAB01C7F-1D62-422D-AC96-A855A5D0E1A1', 'WesternUnion', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('8589148C-C0C7-4BBD-A2B6-45B4B8713421', 'MoneyGram', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('C095332C-6754-422C-ACFD-4925E15F7449', 'VISA', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('45242823-1108-4550-9CEB-E61E9358D6F5', 'TCF', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('74BA9FC6-4121-47A8-83F5-D8BEA5653C21', 'Continental', GETDATE())
INSERT INTO tProcessors(rowguid, Name, DTCreate) VALUES ('5185FE4E-D3B6-42F1-B97A-F2C225C01241', 'TSys', GETDATE())
GO
--Insert Script for tProductProcessorsMapping
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate)  
	VALUES('A33CE7C6-9C28-4AFA-A288-E01818923B44', '8F2FA74E-C135-4028-8F4D-36A57F46CF6E', '5185FE4E-D3B6-42F1-B97A-F2C225C01241', 102, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate)  
	VALUES('A07887AD-2967-4B50-A121-2BEBB78866CD', '8F2FA74E-C135-4028-8F4D-36A57F46CF6E', 'c095332c-6754-422c-acfd-4925e15f7449', 103, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('586D1DE4-EE57-41A4-A1A3-B27866B8DB47', '21677DF0-58E3-4C47-A9C3-6EF651ECCDB8', 'ACC748AE-659F-4304-AFC4-45C0403A2E40', 200, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('8611D2F2-F1C0-4516-9B36-F45511B96CD2', '10D73929-52D5-4D5E-956F-0771526A3C42', 'AAB01C7F-1D62-422D-AC96-A855A5D0E1A1', 301, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('5146E6D8-115B-467C-B396-22B81A77C6B5', '10D73929-52D5-4D5E-956F-0771526A3C42', '8589148C-C0C7-4BBD-A2B6-45B4B8713421', 302, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate)  
	VALUES('F9FCE8C7-D2AB-46CB-A16E-3D2F9AF4D9A5', 'FE641EB0-68FD-4667-853E-442003023A72', 'AAB01C7F-1D62-422D-AC96-A855A5D0E1A1', 301, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('F070307D-6F52-4C38-80D7-EA8C1F6620F5', 'FE641EB0-68FD-4667-853E-442003023A72', '8589148C-C0C7-4BBD-A2B6-45B4B8713421', 302, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('72379E22-C884-45F3-ADE4-801459F6EE25', 'A3EB4C6E-F28C-4DB0-A2C6-DDDEC671D2DF', 'AAB01C7F-1D62-422D-AC96-A855A5D0E1A1', 401, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('F274B519-233D-4424-84BF-88A5702B9902', 'A3EB4C6E-F28C-4DB0-A2C6-DDDEC671D2DF', '8589148C-C0C7-4BBD-A2B6-45B4B8713421', 405, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('1385D15F-B997-4A68-AF67-FE7DE0AE8B08', '2606D541-9613-456D-83BA-FFB810E0E417', '74BA9FC6-4121-47A8-83F5-D8BEA5653C21', 503, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('846587E6-BA2B-479C-AD27-003B0BCF01F4', '2606D541-9613-456D-83BA-FFB810E0E417', '45242823-1108-4550-9CEB-E61E9358D6F5', 504, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('B2CD730B-95EC-41C6-B8C4-5427E8DB21CF', '2606D541-9613-456D-83BA-FFB810E0E417', '8589148C-C0C7-4BBD-A2B6-45B4B8713421', 505, GETDATE())
INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTCreate) 
	VALUES('75401337-7718-40F3-AF8F-D177D25E4DDF', '8863DF03-5FA7-404A-B142-F38B6A060E21', null, 1000, GETDATE())
GO

-- Insert Script for tChannelPartnerProductProcessorsMapping to MGI
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', '586D1DE4-EE57-41A4-A1A3-B27866B8DB47', 1, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', 'F274B519-233D-4424-84BF-88A5702B9902', 2, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', '5146E6D8-115B-467C-B396-22B81A77C6B5', 3, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', 'B2CD730B-95EC-41C6-B8C4-5427E8DB21CF', 4, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('10F2865B-DBC5-4A0B-983C-62E0A0574354', '75401337-7718-40F3-AF8F-D177D25E4DDF', 5, GETDATE())
GO

-- Insert Script for tChannelPartnerProductProcessorsMapping to TCF
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', '586D1DE4-EE57-41A4-A1A3-B27866B8DB47', 1, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', '72379E22-C884-45F3-ADE4-801459F6EE25', 2, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', '8611D2F2-F1C0-4516-9B36-F45511B96CD2', 3, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', 'F9FCE8C7-D2AB-46CB-A16E-3D2F9AF4D9A5', 4, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', 'A07887AD-2967-4B50-A121-2BEBB78866CD', 5, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', 'B2CD730B-95EC-41C6-B8C4-5427E8DB21CF', 6, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('6D7E785F-7BDD-42C8-BC49-44536A1885FC', '75401337-7718-40F3-AF8F-D177D25E4DDF', 7, GETDATE())
GO

-- Insert Script for tChannelPartnerProductProcessorsMapping to Synovus
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '586D1DE4-EE57-41A4-A1A3-B27866B8DB47', 1, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '72379E22-C884-45F3-ADE4-801459F6EE25', 2, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '8611D2F2-F1C0-4516-9B36-F45511B96CD2', 3, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'F9FCE8C7-D2AB-46CB-A16E-3D2F9AF4D9A5', 4, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', 'A33CE7C6-9C28-4AFA-A288-E01818923B44', 5, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '1385D15F-B997-4A68-AF67-FE7DE0AE8B08', 6, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('EC6AAAE3-2BA7-4E0B-898E-D296DB432C17', '75401337-7718-40F3-AF8F-D177D25E4DDF', 7, GETDATE())
GO

-- Insert Script for tChannelPartnerProductProcessorsMapping to Carver
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '586D1DE4-EE57-41A4-A1A3-B27866B8DB47', 1, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '72379E22-C884-45F3-ADE4-801459F6EE25', 2, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '8611D2F2-F1C0-4516-9B36-F45511B96CD2', 3, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', 'F9FCE8C7-D2AB-46CB-A16E-3D2F9AF4D9A5', 4, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '1385D15F-B997-4A68-AF67-FE7DE0AE8B08', 6, GETDATE())
INSERT INTO tChannelPartnerProductProcessorsMapping (ChannelPartnerId, ProductProcessorId, Sequence, DTCreate) 
	VALUES('578AC8FB-F69C-4DBD-A502-57B1EECD41D6', '75401337-7718-40F3-AF8F-D177D25E4DDF', 7, GETDATE())
GO