-- Author: Bineesh Raghavan
-- Date Created: 11/18/2013
-- Rally ID: US1672

-- Change Credencial Para Votar name to Instituto Federal Electoral for Mexico
UPDATE 
	tNexxoIdTypes
SET 
	name = 'INSTITUTO FEDERAL ELECTORAL'
WHERE 
	rowguid = 'EC8E0827-19F0-4538-8C0F-A85E96D50CA2'
GO

-- Migrates existing 'TEMPORARY RESIDENT CARD' customers to 'GREEN CARD / PERMANENT RESIDENT CARD'
UPDATE 
	tProspectGovernmentIdDetails
SET 
	IdTypePK = 'B15D9943-B536-468D-8150-4DC1A12185FB'
WHERE
	IdTypePK = 'BE9B8367-D54A-4B9A-A0FD-7B1130D848BB'
GO
	
-- Updates 'TEMPORARY RESIDENT CARD' to inactive	
UPDATE 
	tNexxoIdTypes
SET 
	isActive = 0
WHERE 
	rowguid = 'BE9B8367-D54A-4B9A-A0FD-7B1130D848BB'
GO