
IF EXISTS (SELECT name FROM sysobjects
      WHERE name = 'trNpsTerminal')
   DROP TRIGGER trNpsTerminal
GO