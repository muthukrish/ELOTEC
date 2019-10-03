IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='sp_GetCustomItemList')
	BEGIN
		DROP PROCEDURE sp_GetCustomItemList
	END
GO
CREATE PROCEDURE sp_GetCustomItemList
(
@UserId INT
,@deviceId INT
)
AS
BEGIN
select ITMD.ItemId
,ITMD.ItemName
,ITMD.IsActive
,ITMD.iscustom
,CASE 
		WHEN RD.RegistrationId is not null
			THEN 1
		ELSE 0
		END AS RegStatus
,RD.RegistrationId

 from Item_Details ITMD 
 LEFT JOIN Registration_Details RD ON ITMD.ItemId=RD.ItemId and RD.DeviceId=@deviceId AND RD.IsActive=1
 where ITMD.iscustom=1 and ITMD.IsActive=1
END