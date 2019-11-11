*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	: 05-11-2019

**************************************************************************/
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Registration_Details')
BEGIN
CREATE TABLE [dbo].[Registration_Details](
	[RegistrationId] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoomNoId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[Axis] [nvarchar](50) NOT NULL,
	[IsRegistered] [bit] NOT NULL,
	[Updated_Date] [datetime] NOT NULL,
	[lastUpdatedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Registration_Details] PRIMARY KEY CLUSTERED 
(
	[RegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO