/*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	: 25-09-2019
Sample		: exec sp_GetDeviceLastUpdatedDetails 1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetDeviceLastUpdatedDetails') BEGIN
	DROP PROCEDURE sp_GetDeviceLastUpdatedDetails
END
GO
CREATE PROCEDURE [dbo].[sp_GetDeviceLastUpdatedDetails] (@deviceId INT)
AS
BEGIN
SELECT TOP 1 Updated_Date
	,(
		SELECT FirstName
		FROM Users U
		WHERE UserId = RD.lastUpdatedBy and IsActive=1
		) AS LastUpdatedUser
		,DeviceId
		,(SELECT DeviceName FROM Device_Details DD WHERE DD.DeviceId = RD.DeviceId) AS DeviceName
FROM Registration_Details RD
WHERE DeviceId = @deviceId and IsActive=1
ORDER BY Updated_Date DESC
END