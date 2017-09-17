-- adding columns AddlDetails,Processor

ALTER TABLE TMessageStore
ADD  AddlDetails nvarchar(4000) null

ALTER TABLE TMessageStore
ADD  Processor nvarchar(20) null

ALTER TABLE TMESSAGESTORE
ALTER COLUMN MessageKey nvarchar(50) null

GO