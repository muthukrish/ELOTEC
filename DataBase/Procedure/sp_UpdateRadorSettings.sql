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
,@radorCoverageVal int
,@radorCoverageOnOffStatus bit
,@radorSensitivityLevelval int
,@radorSensitivityOnOff bit
,@beepOnoff bit
,@radorIndicatorStatus bit
)
AS

BEGIN
	UPDATE Device_Details set RadorCoverageArea=@radorCoverageVal,
	RadorCoverageStatus=@radorCoverageOnOffStatus,RadorSensitivityLevel=@radorSensitivityLevelval,RadorSensitivityStatus=@radorSensitivityOnOff,BeepStatus=@beepOnoff,RadorLEDIndicatorStatus=@radorIndicatorStatus where DeviceId=@deviceId
END