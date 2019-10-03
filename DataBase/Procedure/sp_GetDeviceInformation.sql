/*************************************************************************
Name			: Muthukrishnan
Created Date	: 03-10-2019
Updated Date	: 03-10-2019
Sample		: exec sp_GetDeviceInformation 1,1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetDeviceInformation') BEGIN
	DROP PROCEDURE sp_GetDeviceInformation
END
GO

CREATE PROCEDURE [dbo].[sp_GetDeviceInformation] (
	@UserId INT
	,@deviceId INT
	)
AS
BEGIN
SELECT DeviceId
				,(
					SELECT DeviceName
					FROM Device_Details DD
					WHERE DD.DeviceId = RD.DeviceId
					) AS DeviceName
				,UserId
				,RoomNoId
				,ItemId
				,(
					SELECT ItemName
					FROM Item_Details ItmD
					WHERE ItmD.ItemId = RD.ItemId
					) AS Item
				,Axis
				,IsRegistered
				,Convert(DATE, Updated_Date) AS Updated_Date
				,(
					SELECT FirstName
					FROM Users U
					WHERE UserId = RD.lastUpdatedBy
					) AS LastUpdatedUser
				,IsActive
				FROM Registration_Details RD
			WHERE DeviceId = @deviceId
				AND IsActive = 1
END