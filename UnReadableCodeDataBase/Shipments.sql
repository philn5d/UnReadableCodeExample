CREATE TABLE [dbo].[Shipments]
(
	[shipment_id] INT NOT NULL  IDENTITY PRIMARY KEY, 
    [quote_id] INT NOT NULL, 
    [itemid] INT NOT NULL, 
    [date] DATETIME NOT NULL DEFAULT GETDATE()
)
