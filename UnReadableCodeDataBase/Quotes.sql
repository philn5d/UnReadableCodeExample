CREATE TABLE [dbo].[Quotes]
(
	[quote_id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [item_id] INT NOT NULL, 
    [date] DATETIME NOT NULL DEFAULT GetDate(), 
    [Cost] MONEY NOT NULL, 
    [account_id] INT NOT NULL, 
    CONSTRAINT [FK_Quotes_Accounts] FOREIGN KEY ([account_id]) REFERENCES [Accounts]([accountId]), 
    CONSTRAINT [FK_Quotes_Items] FOREIGN KEY ([item_id]) REFERENCES [ITEMS]([item_id])
)
