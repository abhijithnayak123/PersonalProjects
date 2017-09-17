ALTER TABLE tFView_Credential
DROP COLUMN TerminalID, ProcessorID


ALTER TABLE tFView_Credential
ADD ChannelPartnerId bigint NULL

GO

update tFView_Credential set channelPartnerid = 27
GO

ALTER TABLE tFView_Trx
ADD PreviousCardBalance money NULL

ALTER TABLE tFView_Trx
ALTER COLUMN CardAcceptorTerminalID VARCHAR(100)