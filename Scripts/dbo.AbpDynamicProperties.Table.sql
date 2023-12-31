USE [brady]
GO
/****** Object:  Table [dbo].[AbpDynamicProperties]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpDynamicProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PropertyName] [nvarchar](256) NULL,
	[InputType] [nvarchar](max) NULL,
	[Permission] [nvarchar](max) NULL,
	[TenantId] [int] NULL,
	[DisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_AbpDynamicProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpDynamicProperties_PropertyName_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AbpDynamicProperties_PropertyName_TenantId] ON [dbo].[AbpDynamicProperties]
(
	[PropertyName] ASC,
	[TenantId] ASC
)
WHERE ([PropertyName] IS NOT NULL AND [TenantId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
