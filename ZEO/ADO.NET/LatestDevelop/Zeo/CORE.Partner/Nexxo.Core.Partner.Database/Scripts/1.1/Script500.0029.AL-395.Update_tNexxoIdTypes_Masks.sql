--===========================================================================================
-- Author:		<Chinar Kulkarni>
-- Created date: <June 3rd 2015>
-- Description:	<Update the regexes for ID Types (1. specific for washington state Driver's license and 2. generic for US drivers license and state ID)>           
-- Jira ID:	<AL-522>
--===========================================================================================

-- Allow hyphen and Asterisk for US driver's license and State Identification cards
UPDATE tNexxoIdTypes SET Mask = '^[\w-*]{4,15}$'
WHERE Name IN ('U.S. STATE IDENTITY CARD','DRIVER''S LICENSE')
  AND Mask = '^\w{4,15}$'

-- Regex for Washington state Driver's license (Exactly 12 characters; alphabets or asterisk for first seven characters; alphanumeric or asterisk for last 5 characters.)
UPDATE tNexxoIdTypes SET Mask = '^[a-zA-Z*]{7}[\w*]{5}$'
WHERE Name = 'DRIVER''S LICENSE'
  AND State = 'WASHINGTON'