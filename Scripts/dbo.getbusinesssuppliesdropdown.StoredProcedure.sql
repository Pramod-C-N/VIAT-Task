USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getbusinesssuppliesdropdown]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getbusinesssuppliesdropdown]  
as  
begin  
select distinct salestype from invoiceindicators  
end
GO
