USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getInvoiceSuggestions]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[getInvoiceSuggestions]  --getInvoiceSuggestions null,123,127
(  
@irrno int=null,  
@refNo int=null,
@tenantId int  
)  
as  
begin 
if( @irrno is not null)
begin
select IRNNo as IRNNo,format(IssueDate,'dd-MM-yyyy') as IssueDate from SalesInvoice  
where IRNNo LIKE concat('%',@irrno,'%') and TenantId=@tenantId 
end
else
begin
select BillingReferenceId as InvoiceNumber,format(IssueDate,'dd-MM-yyyy') as IssueDate from SalesInvoice  
where BillingReferenceId LIKE concat('%',@refNo,'%') and TenantId=@tenantId
end
end
GO
