/*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	: 25-09-2019
Sample		: exec sp_UpdateRegistrationDetails 
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_UpdateRegistrationDetails') BEGIN
	DROP PROCEDURE sp_UpdateRegistrationDetails
END
GO
CREATE PROCEDURE [dbo].[sp_UpdateRegistrationDetails] (
	@userId INT
	,@deviceId INT
	,@itemId INT
	,@IsReg BIT
	,@axis NVARCHAR(50)
	)
AS
BEGIN
	IF EXISTS (
			SELECT RegistrationId
			FROM Registration_Details
			WHERE DeviceId = @deviceId
				AND ItemId = @itemId and IsActive=1
			)
	BEGIN
		UPDATE Registration_Details
		SET IsRegistered = CONVERT(BIT, @IsReg),axis = @axis,Updated_Date = GETDATE()
		WHERE DeviceId = @deviceId
			AND ItemId = @itemId and IsActive=1
	END
	ELSE
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
			,@axis
			,@IsReg
			,GETDATE()
			,1
			)
	END
END


