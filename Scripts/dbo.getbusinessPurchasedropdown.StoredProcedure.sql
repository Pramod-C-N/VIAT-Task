USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getbusinessPurchasedropdown]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getbusinessPurchasedropdown]  
as  
begin  
select distinct Purchasetype from invoiceindicators  
end
GO
