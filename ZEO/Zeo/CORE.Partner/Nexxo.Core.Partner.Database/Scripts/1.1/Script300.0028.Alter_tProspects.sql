-- Author: Sunil Shetty	
-- Date Created: Nov 21 2014
-- Description: Adding new coloum Notes to tProspects Table
-- User Story ID: US2260 Task ID: TA6081

if not exists(select * from sys.columns where Name = N'Notes' and Object_ID = Object_ID(N'tProspects'))
begin 
ALTER TABLE tProspects
ADD Notes varchar(250) NULL
end
GO
