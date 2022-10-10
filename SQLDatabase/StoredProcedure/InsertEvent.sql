--CREATE PROCEDURE [dbo].[InsertEvent]
--	@id INT,
--	@startTime DATETIME,
--	@endTime DATETIME,
--	@eventName varchar(50) OUTPUT,
--	@description varchar(max) OUTPUT,
--	@indication varchar(max) OUTPUT,
--	@eventLevel tinyint OUTPUT
--AS
--BEGIN
--    INSERT INTO Event(EventID,StartTime,EndTime,EventName,Description,Indication,EventLevel) VALUES(@id,@startTime,@endTime ,@eventName,@description ,@indication ,@eventLevel)
--	SELECT @eventName=EventName,@description=Description,@indication=Indication,@eventLevel=EventLevel
--    FROM EventDetail
--WHERE EventDetail.EventID=@id
--END


