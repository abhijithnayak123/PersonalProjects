-- =====================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <02/08/2018>
-- Description:	<This function is to split the string seperated by camma>
-- =====================================================================

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'SplitString') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION SplitString
GO

CREATE FUNCTION SplitString
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
 
      RETURN
END
GO