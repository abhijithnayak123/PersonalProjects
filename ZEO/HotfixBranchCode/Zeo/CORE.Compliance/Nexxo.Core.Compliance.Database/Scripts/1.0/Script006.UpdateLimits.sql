
UPDATE tLimits SET PerX=9999, PerDay=9999
WHERE Name='Centris Source Cash'

UPDATE tLimits SET Name='Centris GPR Load', PerDay=9999, PerNDays=9999
WHERE Name='Centris Purse Cash Load'

UPDATE tLimits SET Name='Centris GPR Withdrawal'
WHERE Name='Centris Purse Cash Out'
