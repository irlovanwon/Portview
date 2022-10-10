CREATE TABLE [dbo].[EventDetail]
(
	[ID] INT NOT NULL, 
	[Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [Indication] VARCHAR(MAX) NULL, 
    [EventLevel] TINYINT NULL, 
   
    CONSTRAINT [PK_EventDetail] PRIMARY KEY ([ID]) 
)
