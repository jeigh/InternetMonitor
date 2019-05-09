CREATE TABLE [dbo].[WanSpeed]
(
	[WanSpeedId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [SpeedUp] DECIMAL NOT NULL, 
    [SpeedDown] DECIMAL NOT NULL, 
    [SpeedTime] SMALLDATETIME NOT NULL DEFAULT getdate()
)
