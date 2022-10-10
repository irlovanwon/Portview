--CREATE INDEX RealtimeData_DateTime_Index on [dbo].[RealtimeData]
--([TimeStamp])
--include( [ID],[Value])
CREATE INDEX RealtimeData_DateTime_Index on [dbo].[RealtimeData]
([TimeStamp])
include([Value],[ID])
