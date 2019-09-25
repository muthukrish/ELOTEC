*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	:25-09-2019

**************************************************************************/
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Users')
BEGIN
CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[MobileNo] [varchar](50) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO