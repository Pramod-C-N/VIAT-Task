USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getsalesvatdropdown]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getsalesvatdropdown]  
as  
begin  
select distinct Invoice_flags as name from invoiceindicators where Salestype <> 'NA'  

end
GO
