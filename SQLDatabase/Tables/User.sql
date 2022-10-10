CREATE TABLE [dbo].[User]
(
    [Name] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [Level] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([Name])
)
