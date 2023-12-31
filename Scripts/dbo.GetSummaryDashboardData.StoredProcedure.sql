USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetSummaryDashboardData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create         PROCEDURE [dbo].[GetSummaryDashboardData]  --GetSummaryDashboardData '2023-01-01','2023-01-01','19','Yearly'
(@fromDate datetime,     
@toDate datetime,  
@tenantId int=null,  
@type varchar(10)  
)      
                
as  
begin

SET DATEFIRST 7
if @type=('Monthly')
begin
select CONVERT(char(10), IssueDate,126) as invoiceDate,isnull(sum(LineNetAmount),0.00) as salesAmount,isnull(sum(VATLineAmount),0.00) as vatAmount 
from VI_importstandardfiles_Processed    
where TenantId=@tenantId and   
IssueDate>=@fromDate and IssueDate<=@toDate
and InvoiceType like 'Sales%' group by CONVERT(char(10), IssueDate,126)

end

else if @type=('Weekly')
begin
select CONVERT(char(10), IssueDate,126) as invoiceDate,isnull(sum(LineNetAmount),0.00) as salesAmount,isnull(sum(VATLineAmount),0.00) as vatAmount

from VI_importstandardfiles_Processed    
where TenantId=@tenantId and   
IssueDate>=@fromDate and IssueDate<=@toDate  
and InvoiceType like 'Sales%' group by CONVERT(char(10), IssueDate,126)

end

else
begin
select CONVERT(char(10), IssueDate,126) as invoiceDate,isnull(sum(LineNetAmount),0.00) as salesAmount,isnull(sum(VATLineAmount),0.00) as vatAmount  
from VI_importstandardfiles_Processed    
where TenantId=@tenantId and   
IssueDate>=@fromDate and IssueDate<=@toDate
and InvoiceType like 'Sales%' group by CONVERT(char(10), IssueDate,126)
end
 

end
GO
