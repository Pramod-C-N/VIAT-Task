USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CheckIfRefNumExists]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CheckIfRefNumExists]  
(  
@refnum nvarchar(200) ,  
@tenantid int  
)  
as  
begin  
select count(*) as count from SalesInvoice with(nolock) where BillingReferenceId=@refnum and tenantid=@tenantid  
end
GO
