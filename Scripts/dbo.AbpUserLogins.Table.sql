USE [brady]
GO
/****** Object:  Table [dbo].[AbpUserLogins]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpUserLogins](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](256) NOT NULL,
	[TenantId] [int] NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_AbpUserLogins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpUserLogins_ProviderKey_TenantId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AbpUserLogins_ProviderKey_TenantId] ON [dbo].[AbpUserLogins]
(
	[ProviderKey] ASC,
	[TenantId] ASC
)
WHERE ([TenantId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey] ON [dbo].[AbpUserLogins]
(
	[TenantId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpUserLogins_TenantId_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpUserLogins_TenantId_UserId] ON [dbo].[AbpUserLogins]
(
	[TenantId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AbpUserLogins_UserId]    Script Date: 01-09-2023 17:11:24 ******/
CREATE NONCLUSTERED INDEX [IX_AbpUserLogins_UserId] ON [dbo].[AbpUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AbpUserLogins_AbpUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AbpUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AbpUserLogins] CHECK CONSTRAINT [FK_AbpUserLogins_AbpUsers_UserId]
GO
