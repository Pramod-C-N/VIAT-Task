USE [brady]
GO
/****** Object:  Table [dbo].[AbpDynamicEntityPropertyValues]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpDynamicEntityPropertyValues](
	[Value] [nvarchar](max) NOT NULL,
	[EntityId] [nvarchar](max) NULL,
	[DynamicEntityPropertyId] [int] NOT NULL,
	[TenantId] [int] NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AbpDynamicEntityPropertyValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId] ON [dbo].[AbpDynamicEntityPropertyValues]
(
	[DynamicEntityPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpDynamicEntityPropertyValues]  WITH CHECK ADD  CONSTRAINT [FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_DynamicEntityPropertyId] FOREIGN KEY([DynamicEntityPropertyId])
REFERENCES [dbo].[AbpDynamicEntityProperties] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpDynamicEntityPropertyValues] CHECK CONSTRAINT [FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_DynamicEntityPropertyId]
GO
