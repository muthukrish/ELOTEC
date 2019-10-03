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
IF @RegStatus=1
	BEGIN
		IF NOT EXISTS (SELECT RegistrationId FROM Registration_Details WHERE DeviceId = @deviceId AND ItemId = @itemId and IsActive=1)
			BEGIN
				INSERT INTO Registration_Details (
							DeviceId
							,UserId
							,RoomNoId
							,ItemId
							,axis
							,IsRegistered
							,Updated_Date
							,IsActive
							)
						VALUES (
							@deviceId
							,@userId
							,1
							,@itemId
							,''
							,0
							,GETDATE()
							,1
							)
			END
	END
ELSE IF @RegStatus=1
	BEGIN
		DELETE FROM Registration_Details WHERE DeviceId=@deviceId AND ItemId=@itemId
	END

END