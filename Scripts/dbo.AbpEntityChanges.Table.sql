USE [brady]
GO
/****** Object:  Table [dbo].[AbpEntityChanges]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpEntityChanges](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ChangeTime] [datetime2](7) NOT NULL,
	[ChangeType] [tinyint] NOT NULL,
	[EntityChangeSetId] [bigint] NOT NULL,
	[EntityId] [nvarchar](48) NULL,
	[EntityTypeFullName] [nvarchar](192) NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_AbpEntityChanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpEntityChanges_EntityChangeSetId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpEntityChanges_EntityChangeSetId] ON [dbo].[AbpEntityChanges]
(
	[EntityChangeSetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpEntityChanges_EntityTypeFullName_EntityId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpEntityChanges_EntityTypeFullName_EntityId] ON [dbo].[AbpEntityChanges]
(
	[EntityTypeFullName] ASC,
	[EntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpEntityChanges]  WITH CHECK ADD  CONSTRAINT [FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId] FOREIGN KEY([EntityChangeSetId])
REFERENCES [dbo].[AbpEntityChangeSets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpEntityChanges] CHECK CONSTRAINT [FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId]
GO
