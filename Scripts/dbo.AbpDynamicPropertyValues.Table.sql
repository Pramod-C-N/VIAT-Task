USE [brady]
GO
/****** Object:  Table [dbo].[AbpDynamicPropertyValues]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpDynamicPropertyValues](
	[Value] [nvarchar](max) NOT NULL,
	[TenantId] [int] NULL,
	[DynamicPropertyId] [int] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_AbpDynamicPropertyValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpDynamicPropertyValues_DynamicPropertyId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpDynamicPropertyValues_DynamicPropertyId] ON [dbo].[AbpDynamicPropertyValues]
(
	[DynamicPropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpDynamicPropertyValues]  WITH CHECK ADD  CONSTRAINT [FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropertyId] FOREIGN KEY([DynamicPropertyId])
REFERENCES [dbo].[AbpDynamicProperties] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpDynamicPropertyValues] CHECK CONSTRAINT [FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropertyId]
GO
