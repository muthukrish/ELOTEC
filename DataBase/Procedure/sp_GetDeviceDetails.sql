/*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	: 25-09-2019
Sample		: exec sp_GetDeviceDetails 1,1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetDeviceDetails') BEGIN
	DROP PROCEDURE sp_GetDeviceDetails
END
GO

CREATE PROCEDURE [dbo].[sp_GetDeviceDetails] (
	@UserId INT
	,@deviceId INT
	)
AS
BEGIN
	BEGIN
			DECLARE @ItemId VARCHAR(50)
			DECLARE db_cursor CURSOR
			FOR
			SELECT ItemId
			FROM Item_Details

			OPEN db_cursor
			FETCH NEXT
			FROM db_cursor
			INTO @ItemId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF NOT EXISTS (
						SELECT RegistrationId
						FROM Registration_Details
						WHERE DeviceId = @deviceId
							AND ItemId = @ItemId
						)
				BEGIN
					INSERT INTO Registration_Details (
						DeviceId
						,UserId
						,RoomNoId
						,ItemId
						,Axis
						,IsRegistered
						,Updated_Date
						,lastUpdatedBy
						,IsActive
						)
					VALUES (
						@deviceId
						,@UserId
						,1
						,@ItemId
						,''
						,0
						,GETDATE()
						,@UserId
						,1
						)
				END

				FETCH NEXT

				FROM db_cursor

				INTO @ItemId

			END
			CLOSE db_cursor
			DEALLOCATE db_cursor
			SELECT DeviceId
				,(
					SELECT DeviceName
					FROM Device_Details DD
					WHERE DD.DeviceId = RD.DeviceId
					) AS Device
				,UserId
				,RoomNoId
				,ItemId
				,(
					SELECT ItemName
					FROM Item_Details ItmD
					WHERE ItmD.ItemId = RD.ItemId
					) AS Item
				,Axis
				,IsRegistered
				,Convert(DATE, Updated_Date) AS Updated_Date
				,(
					SELECT FirstName
					FROM Users U
					WHERE UserId = RD.lastUpdatedBy
					) AS LastUpdatedUser
				,IsActive
			FROM Registration_Details RD
			WHERE DeviceId = @deviceId
				AND IsActive = 1

	END
END

GO
