USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetReportType]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetReportType] @code varchar(max)
as
Begin
select ReportName,Code from ReportCode where Code like '%'+@code+'%' and Active=1

End
GO
