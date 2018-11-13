-- Author: Sunil Shetty	
-- Date Created: Nov 21 2014
-- Description: Adding new coloum IsNotesEnable to tChannelPartnerConfig Table
-- User Story ID: US2260 Task ID: TA6081

if not exists(select * from sys.columns where Name = N'IsNotesEnable' and Object_ID = Object_ID(N'tChannelPartnerConfig'))
begin 
ALTER TABLE tChannelPartnerConfig
ADD IsNotesEnable bit NULL
end
GO
