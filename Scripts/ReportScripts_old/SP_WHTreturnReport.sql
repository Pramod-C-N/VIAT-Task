USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[SP_WHTreturnReport]    Script Date: 4/13/2023 2:53:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
ALTER    PROCEDURE [dbo].[SP_WHTreturnReport](@FromDate datetime,  
@ToDate datetime,
@tenantId int=null)  
as  
begin  
declare @WhtReturnReport as table  
(slno int identity(1,1),  
Typeofpayments nvarchar(100),  
NameofPayee nvarchar(100),  
paymentdate nvarchar(max),  
totalamountPaid decimal(18,2),  
taxrate decimal(18,2),  
taxDue decimal(18,2)  
)  
--insert into @WhtReturnReport (TypeofPayment,NameofthePayee,paymentdate,totalamount,taxrate,taxdue)   
--values ('TypeofPayment','NameofthePayee',GETDATE(),1000,15,100);  
   
   
 insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue)   
 select v.Natureofservices,v.BuyerName,format(v.issuedate,'dd-MM-yyyy') as issuedate,sum(v.LineAmountInclusiveVAT) as totalamount,p.standardrate,  
  sum(round(v.LineAmountInclusiveVAT*p.standardrate/100,2)) as taxdue    
  from VI_importstandardfiles_Processed v with (nolock)  
  inner join vi_paymentWHTrate p with (nolock) on v.UniqueIdentifier = p.uniqueidentifier  
  where v.TenantId=@tenantId and v.IssueDate >= @fromdate and v.IssueDate <= @todate and v.InvoiceType like 'WHT%'    
  group by v.NatureofServices,v.BuyerName,v.IssueDate,p.StandardRate   
  order by v.NatureofServices,v.BuyerName,v.IssueDate  
   
    
  select * from @WhtReturnReport order by Typeofpayments,NameofPayee,paymentdate   
  
end