/*************************************************************************
Name			: Muthukrishnan
Created Date	: 02-10-2019
Updated Date	: 02-10-2019
Sample		: exec sp_UpdateRadorSettings 1
**************************************************************************/
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name='sp_UpdateRadorSettings')
BEGIN
	DROP PROCEDURE sp_UpdateRadorSettings
END
GO
create procedure sp_UpdateRadorSettings(
@userId int
,@deviceId int
,@radorlevelVal int
,@radorOnOffStatus bit
,@dbMeterLevelval int
,@dbmeterOnOff bit
,@beepOnoff bit
)
AS

BEGIN
	UPDATE Device_Details set RadorAdjustLevel=@radorlevelVal,
	RadorAdjustStatus=@radorOnOffStatus,DbMeterAdjustLevel=@dbMeterLevelval,DbMeterAdjustStatus=@dbmeterOnOff,BeepStatus=@beepOnoff where DeviceId=@deviceId
END