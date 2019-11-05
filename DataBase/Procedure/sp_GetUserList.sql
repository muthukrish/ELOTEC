/*************************************************************************
Name			: Muthukrishnan
Created Date	: 04-10-2019
Updated Date	: 04-10-2019
Sample		: exec sp_GetUserList
**************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_GetUserList') BEGIN
	DROP PROCEDURE sp_GetUserList
END
GO

CREATE PROCEDURE [dbo].[sp_GetUserList]
AS
BEGIN
 SELECT UserId,FirstName FROM Users WHERE IsActive=1
END