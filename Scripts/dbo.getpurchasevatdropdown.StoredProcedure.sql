USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getpurchasevatdropdown]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getpurchasevatdropdown]  
as  
begin  
select distinct Invoice_flags as name from invoiceindicators where Purchasetype <> 'NA'  
end
GO
