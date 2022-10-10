CREATE INDEX Event_DateTime_Index on [dbo].[Event]
([StartTime],[EndTime],[EventLevel]) include([ID])
