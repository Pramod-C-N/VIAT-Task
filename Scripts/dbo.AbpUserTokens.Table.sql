USE [brady]
GO
/****** Object:  Table [dbo].[AbpUserTokens]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpUserTokens](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginProvider] [nvarchar](128) NULL,
	[Name] [nvarchar](128) NULL,
	[TenantId] [int] NULL,
	[UserId] [bigint] NOT NULL,
	[Value] [nvarchar](512) NULL,
	[ExpireDate] [datetime2](7) NULL,
 CONSTRAINT [PK_AbpUserTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpUserTokens_TenantId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpUserTokens_TenantId_UserId] ON [dbo].[AbpUserTokens]
(
	[TenantId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpUserTokens_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpUserTokens_UserId] ON [dbo].[AbpUserTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AbpUserTokens_AbpUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AbpUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpUserTokens] CHECK CONSTRAINT [FK_AbpUserTokens_AbpUsers_UserId]
GO
