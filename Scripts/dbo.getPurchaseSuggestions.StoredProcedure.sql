USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getPurchaseSuggestions]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getPurchaseSuggestions]
(
@irrno int,
@tenantId int
)
as
begin
select Id,IssueDate from PurchaseEntry
where id LIKE concat('%',@irrno,'%') and TenantId=@tenantId
end
GO
