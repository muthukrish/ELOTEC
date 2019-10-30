/*************************************************************************
Name			: Muthukrishnan
Created Date	: 03-10-2019
Updated Date	: 03-10-2019
Sample		: exec sp_GetDeviceList 1,1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetDeviceList') BEGIN
	DROP PROCEDURE sp_GetDeviceList
END
GO

CREATE PROCEDURE [dbo].[sp_GetDeviceList] (
	@filterStr nvarchar(max)
	)
AS
BEGIN
	DECLARE @querystring NVARCHAR(max)
	SET @querystring = 'select * from (
				SELECT RegistrationId,DeviceId
				,(
					SELECT DeviceName
					FROM Device_Details DD
					WHERE DD.DeviceId = RD.DeviceId
					) AS DeviceName
				,UserId
				,IsRegistered
				,Convert(DATE, Updated_Date) AS Updated_Date
				,(
					SELECT FirstName
					FROM Users U
					WHERE UserId = RD.lastUpdatedBy
					) AS LastUpdatedUser
				,IsActive
			FROM Registration_Details RD) AS x' + @filterStr
EXEC (@querystring)
				
END