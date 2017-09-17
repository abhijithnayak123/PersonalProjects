-- Author: Bineesh Raghavan
-- Date Created: 11/14/2013
-- Description: Updating new column IsActive to limit the IdTypes available Create/Edit Customer Identification screen
-- Rally ID: US1672
UPDATE 
	tNexxoIdTypes 
SET 
	IsActive = 1
WHERE 
	Country = 'UNITED STATES' 
	AND Name IN 
	(
		'DRIVER''S LICENSE',
		'EMPLOYMENT AUTHORIZATION CARD (EAD0',
		'GREEN CARD / PERMANENT RESIDENT CARD',
		'MILITARY ID','PASSPORT',
		'TEMPORARY RESIDENT CARD'
		,'U.S. STATE IDENTITY CARD'
	)  
GO

UPDATE 
	tNexxoIdTypes 
SET 
	IsActive = 1
WHERE 
	Country = 'Mexico' 
	AND Name IN 
	(
		'CREDENCIAL PARA VOTAR',
		'LICENCIA DE CONDUCIR',
		'MATRICULA CONSULAR',
		'PASAPORTE'
	)  
GO
