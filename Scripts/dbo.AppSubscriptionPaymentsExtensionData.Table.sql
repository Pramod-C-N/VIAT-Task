USE [brady]
GO
/****** Object:  Table [dbo].[AppSubscriptionPaymentsExtensionData]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSubscriptionPaymentsExtensionData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SubscriptionPaymentId] [bigint] NOT NULL,
	[Key] [nvarchar](450) NULL,
	[Value] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_AppSubscriptionPaymentsExtensionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppSubscriptionPaymentsExtensionData_SubscriptionPaymentId_Key_IsDeleted]    Script Date: 01-09-2023 17:11:24 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppSubscriptionPaymentsExtensionData_SubscriptionPaymentId_Key_IsDeleted] ON [dbo].[AppSubscriptionPaymentsExtensionData]
(
	[SubscriptionPaymentId] ASC,
	[Key] ASC,
	[IsDeleted] ASC
)
WHERE ([IsDeleted]=(0))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
