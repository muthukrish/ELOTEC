*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	:25-09-2019

**************************************************************************/
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='RoomNo_Details')
BEGIN
CREATE TABLE [dbo].[RoomNo_Details](
	[RoomNoId] [bigint] IDENTITY(1,1) NOT NULL,
	[RoomNo] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_RoomNo_Details] PRIMARY KEY CLUSTERED 
(
	[RoomNoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO