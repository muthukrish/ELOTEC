/*************************************************************************
Name			: Muthukrishnan
Created Date	: 26-09-2019
Updated Date	: 26-09-2019
Sample		: exec sp_GetDeviceLastUpdatedDetails 1
**************************************************************************/
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name='sp_UpdateRoomNo')
BEGIN
	DROP PROCEDURE sp_UpdateRoomNo
END
GO
create procedure sp_UpdateRoomNo(
@userId int,
@deviceId int,
@roomName nvarchar(max)
)
AS
BEGIN
UPDATE Device_Details set DeviceName=@roomName where DeviceId=@deviceId
END