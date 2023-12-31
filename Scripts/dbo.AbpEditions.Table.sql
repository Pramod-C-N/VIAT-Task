USE [brady]
GO
/****** Object:  Table [dbo].[AbpEditions]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbpEditions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorUserId] [bigint] NULL,
	[DeleterUserId] [bigint] NULL,
	[DeletionTime] [datetime2](7) NULL,
	[DisplayName] [nvarchar](64) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierUserId] [bigint] NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[AnnualPrice] [decimal](18, 2) NULL,
	[ExpiringEditionId] [int] NULL,
	[MonthlyPrice] [decimal](18, 2) NULL,
	[TrialDayCount] [int] NULL,
	[WaitingDayAfterExpire] [int] NULL,
	[DailyPrice] [decimal](18, 2) NULL,
	[WeeklyPrice] [decimal](18, 2) NULL,
 CONSTRAINT [PK_AbpEditions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbpEditions] ADD  DEFAULT (N'') FOR [Discriminator]
GO
