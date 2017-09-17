--- ================================================================================
-- Author:		<Priya Rajan>
-- Create date: <04/11/2016>
-- Description:	 MO Fee for Carver should be 1$ from May 1st
-- Jira ID:		<AL-6290>
-- ================================================================================

declare @mocarverfee uniqueidentifier

select @mocarverfee = [PricingGroupPK]
     FROM [tPricingGroups]  
	 where PricingGroupName='MO-Carver'

update [tPricing] set Value = 1.00, DTServerLastModified = GETDATE(), DTTerminalLastModified = GETDATE()
      where PricingGroupPK=@mocarverfee;
	  
GO