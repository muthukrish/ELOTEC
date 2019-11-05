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
			 SELECT DeviceId
			 ,DeviceName
			,IsActive
			,(select max(Updated_Date) from Registration_Details where DeviceId=DD.DeviceId) As Updated_Date
			,(select TOP 1 UserId from Registration_Details where Updated_Date= (select max(Updated_Date) from Registration_Details where DeviceId=DD.DeviceId) and DeviceId=DD.DeviceId and IsActive=1) AS UserId
			,(
			SELECT FirstName
			FROM Users U
			WHERE UserId = (select TOP 1 UserId from Registration_Details where Updated_Date= (select max(Updated_Date) from Registration_Details where DeviceId=DD.DeviceId) and DeviceId=DD.DeviceId and IsActive=1)
			) AS LastUpdatedUser
			,CASE when (select count(*) from Registration_Details where IsRegistered=0 and IsActive=1 and DeviceId=DD.DeviceId ) > 0
			THEN 0
			ELSE 1
			END AS RegisteredStatus
  from Device_Details DD
  ) AS x' + @filterStr
		EXEC (@querystring)	
				
END