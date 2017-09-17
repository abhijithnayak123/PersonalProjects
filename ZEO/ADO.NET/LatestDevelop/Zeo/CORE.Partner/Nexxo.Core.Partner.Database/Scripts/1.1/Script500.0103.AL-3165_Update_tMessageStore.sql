
--- ================================================================================
-- Author:		<Divya>
-- Create date: <11/24/2015>
-- Description:	<As Alloy, In WU T0425 pop up error message 'Additional Details' message is cropped>
-- Jira ID:		<AL-3165>
-- ================================================================================

-- As per karun's decision, added Errormessage for WU error code T0425 for synovus,carver and Tcf.
--Billpay
IF  EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=28 and AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer')
BEGIN
update tMessageStore set AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction'
where MessageKey='1004.425'and PartnerPK=28
end
go
IF  EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=33 and AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer')
BEGIN
update tMessageStore set AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction'
where MessageKey='1004.425'and PartnerPK=33
end
go
IF  EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=34 and AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer')
BEGIN
update tMessageStore set AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction'
where MessageKey='1004.425'and PartnerPK=34
end
go
IF  EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1004.425'and PartnerPK=1 and AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer')
BEGIN
update tMessageStore set AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction'
where MessageKey='1004.425'and PartnerPK=1
end
go

--SendMoney
IF  EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey='1005.425'and PartnerPK=1 and AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer')
BEGIN
update tMessageStore set AddlDetails='Please add ID, SSN/ITIN, Date of Birth and/or Occupation to customer''s profile and then resubmit transaction'
where MessageKey='1005.425'and PartnerPK=1
end
go