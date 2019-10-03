/*************************************************************************
Name			: Muthukrishnan
Created Date	: 03-10-2019
Updated Date	: 03-10-2019
Sample		: exec sp_GetDeviceSettingDetails 1,1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetDeviceSettingDetails') BEGIN
	DROP PROCEDURE sp_GetDeviceSettingDetails
END
GO
CREATE PROCEDURE [dbo].[sp_GetDeviceSettingDetails] (
	@UserId INT
	,@deviceId INT
	)
AS
BEGIN
	SELECT DeviceId,DeviceName,IsActive,RadorAdjustLevel,RadorAdjustStatus,DbMeterAdjustLevel,
	DbMeterAdjustStatus,BeepStatus FROM Device_Details WHERE DeviceId=@deviceId AND IsActive=1
END