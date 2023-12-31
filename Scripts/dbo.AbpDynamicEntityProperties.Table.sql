USE [brady]
GO
/****** Object:  Table [dbo].[AbpDynamicEntityProperties]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpDynamicEntityProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityFullName] [nvarchar](256) NULL,
	[DynamicPropertyId] [int] NOT NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_AbpDynamicEntityProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpDynamicEntityProperties_DynamicPropertyId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpDynamicEntityProperties_DynamicPropertyId] ON [dbo].[AbpDynamicEntityProperties]
(
	[DynamicPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_TenantId] ON [dbo].[AbpDynamicEntityProperties]
(
	[EntityFullName] ASC,
	[DynamicPropertyId] ASC,
	[TenantId] ASC
)
WHERE ([EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpDynamicEntityProperties]  WITH CHECK ADD  CONSTRAINT [FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPropertyId] FOREIGN KEY([DynamicPropertyId])
REFERENCES [dbo].[AbpDynamicProperties] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpDynamicEntityProperties] CHECK CONSTRAINT [FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPropertyId]
GO
