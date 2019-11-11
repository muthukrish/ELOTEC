*************************************************************************
Name			: Muthukrishnan
Created Date	: 25-09-2019
Updated Date	:05-11-2019

**************************************************************************/
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Device_Details')
BEGIN
CREATE TABLE [dbo].[Device_Details](
	[DeviceId] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceName] [nvarchar](50) NOT NULL,
	[RoomNoId] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RadorCoverageArea] [int] NOT NULL,
	[RadorCoverageStatus] [bit] NOT NULL,
	[RadorSensitivityLevel] [int] NOT NULL,
	[RadorSensitivityStatus] [bit] NOT NULL,
	[BeepStatus] [bit] NOT NULL,
	[RadorLEDIndicatorStatus] [bit] NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[SoftwareVersion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Device_Details] PRIMARY KEY CLUSTERED 
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO