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
 select RD.ItemId
,(
					SELECT ItemName
					FROM Item_Details ItmD
					WHERE ItmD.ItemId = RD.ItemId
					) AS ItemName
,RD.IsCustom AS IsActive
 from Registration_Details RD where RD.DeviceId=@deviceId AND RD.IsActive=1
 order by ItemId asc


END