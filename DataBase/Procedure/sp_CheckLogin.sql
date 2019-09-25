/*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	: 25-09-2019
Sample		: exec sp_CheckLogin 'm','m1',1
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_CheckLogin') BEGIN
	DROP PROCEDURE sp_CheckLogin
END
GO

CREATE PROCEDURE [dbo].[sp_CheckLogin] (
	@userName nvarchar(max)
	,@password nvarchar(max)
	,@userId INT OUTPUT
	)
AS
BEGIN
	SELECT @userId = UserId FROM Users WHERE UserName = @userName AND Password = @password and IsActive=1
END
RETURN @userId
GO
