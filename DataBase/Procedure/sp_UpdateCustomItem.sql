IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='sp_UpdateCustomItem')
	BEGIN
		DROP PROCEDURE sp_UpdateCustomItem
	END
GO
CREATE PROCEDURE sp_UpdateCustomItem
(
@UserId INT
,@deviceId INT
,@itemId INT
,@RegStatus bit
)
AS
BEGIN
update Registration_Details set IsCustom=@RegStatus where DeviceId=@deviceId AND ItemId=@itemId
END