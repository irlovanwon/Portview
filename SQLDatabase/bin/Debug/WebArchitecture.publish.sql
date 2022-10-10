/*
master 的部署脚本

此代码由工具生成。
如果重新生成此代码，则对此文件的更改可能导致
不正确的行为并将丢失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "master"
:setvar DefaultFilePrefix "master"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL11.IRLOVAN\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL11.IRLOVAN\MSSQL\DATA\"

GO
:on error exit
GO
/*
请检测 SQLCMD 模式，如果不支持 SQLCMD 模式，请禁用脚本执行。
要在启用 SQLCMD 模式后重新启用脚本，请执行:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'要成功执行此脚本，必须启用 SQLCMD 模式。';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'已跳过具有键 efc6fc32-b402-4e01-8ba9-469110e75788, 971f5a0c-ccf2-4af8-bdb3-6464889257ba 的重命名重构操作，不会将元素 [dbo].[Event].[Id] (SqlSimpleColumn) 重命名为 EventID';


GO
PRINT N'已跳过具有键 2da72040-b75c-4aa6-bc0b-1940156b1891, 45bff29b-f6e7-42c9-bf5b-662360ba87da 的重命名重构操作，不会将元素 [dbo].[EventDetail].[Id] (SqlSimpleColumn) 重命名为 EventID';


GO
PRINT N'已跳过具有键 a4bf7f38-80af-4091-9809-e23819ae272f 的重命名重构操作，不会将元素 [dbo].[EventDetail].[Indicati] (SqlSimpleColumn) 重命名为 Indication';


GO
PRINT N'正在创建 [dbo].[Event]...';


GO
CREATE TABLE [dbo].[Event] (
    [EventID]     INT           NOT NULL,
    [StartTime]   DATETIME      NOT NULL,
    [EndTime]     DATETIME      NOT NULL,
    [EventName]   VARCHAR (MAX) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [Indication]  VARCHAR (MAX) NULL,
    [EventLevel]  TINYINT       NULL,
    PRIMARY KEY CLUSTERED ([EventID] ASC)
);


GO
PRINT N'正在创建 [dbo].[EventDetail]...';


GO
CREATE TABLE [dbo].[EventDetail] (
    [EventID]     INT           NOT NULL,
    [EventName]   VARCHAR (MAX) NULL,
    [Description] VARCHAR (MAX) NULL,
    [Indication]  VARCHAR (MAX) NULL,
    [EventLevel]  TINYINT       NULL,
    CONSTRAINT [PK_EventDetail] PRIMARY KEY CLUSTERED ([EventID] ASC)
);


GO
PRINT N'正在创建 [dbo].[InsertEvent]...';


GO
CREATE PROCEDURE [dbo].[InsertEvent]
	@id INT,
	@startTime DATETIME,
	@endTime DATETIME
AS
BEGIN
    INSERT INTO Event(EventID,StartTime,EndTime,EventName,Description,Indication,EventLevel) VALUES (@id,@startTime,@endTime) 
	SELECT EventName,Description,Indication,EventLevel
    FROM EventDetail
WHERE EventDetail.EventID=@id
END
GO
-- 正在重构步骤以使用已部署的事务日志更新目标服务器

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'efc6fc32-b402-4e01-8ba9-469110e75788')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('efc6fc32-b402-4e01-8ba9-469110e75788')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2da72040-b75c-4aa6-bc0b-1940156b1891')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2da72040-b75c-4aa6-bc0b-1940156b1891')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a4bf7f38-80af-4091-9809-e23819ae272f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a4bf7f38-80af-4091-9809-e23819ae272f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '45bff29b-f6e7-42c9-bf5b-662360ba87da')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('45bff29b-f6e7-42c9-bf5b-662360ba87da')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '971f5a0c-ccf2-4af8-bdb3-6464889257ba')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('971f5a0c-ccf2-4af8-bdb3-6464889257ba')

GO

GO
PRINT N'更新完成。';


GO
