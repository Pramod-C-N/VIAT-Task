USE [vitadb11]
GO
/****** Object:  StoredProcedure [dbo].[SP_WHTDetailReport]    Script Date: 4/13/2023 3:03:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
									-- exec sp_whtdetailreport '2023-01-01','2023-01-31',33
  
ALTER       PROCEDURE [dbo].[SP_WHTDetailReport](
@FromDate datetime,  
@ToDate datetime,
@tenantId int=null)  
as  
begin  
declare @WhtDetailReport as table  
(SLNO int identity(1,1),  
Typeofpayments nvarchar(100),  
NameofPayee nvarchar(100),  
country nvarchar(100),  
paymentdate nvarchar(100),  
totalamountPaid decimal(18,2),  
taxrate decimal(18,2),  
withholdingtaxamount decimal(18,2)  ,
StandardRate decimal(18,2),
DTTrate decimal(18,2),
AffiliationStatus nvarchar(30) 
)  

--select * from VI_ImportMasterFiles_Processed 
--insert into @WhtDetailReport (TypeofPayments,NameofPayee,country,paymentdate,totalamountPaid,taxrate,withholdingtaxamount)   
--values ('TypeofPayment','NameofthePayee','SA',GETDATE(),1000,15,100);  
   
  insert into @WhtDetailReport (TypeofPayments,NameofPayee,country,paymentdate,totalamountPaid,taxrate,withholdingtaxamount,StandardRate,DTTrate,AffiliationStatus)     
  select v.Natureofservices,v.BuyerName,v.BuyerCountryCode,format(v.issuedate,'dd-MM-yyyy'),round(v.LineAmountInclusiveVAT,2),p.effrate,  
  round(v.LineAmountInclusiveVAT*p.effrate/100,2),p.lawrate,p.standardrate,AffiliationStatus    
  from VI_importstandardfiles_Processed v with (nolock)  
  inner join vi_paymentWHTrate p with (nolock) on v.UniqueIdentifier = p.uniqueidentifier 
  where v.TenantId=@tenantId and v.IssueDate >= @fromdate and v.IssueDate <= @todate and v.InvoiceType like 'WHT%'  order by natureofservices,buyername,issuedate  
  
  select * from @WhtDetailReport  

--  select * from VI_PaymentWHTRate 
end