-- Author: Ashok Kumar G
-- Date Created: May 26 2014
-- Description: Updating tTransactionMinimums minimum to 1 for all MoneyTransfer transactions.
-- User Story ID: US2008


update tTransactionMinimums set Minimum = 1 where TransactionType = 6