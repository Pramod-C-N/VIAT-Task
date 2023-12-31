USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getnatureofservice]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getnatureofservice]
as
begin
select distinct Code,Description,concat('SCH - ',code,' - ',Description) as [Value] from NatureofServices where (code <> 0 and Description is not null)
order by code
end
GO
