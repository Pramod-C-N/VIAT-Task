USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SP_WHTDetailReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        PROCEDURE [dbo].[SP_WHTDetailReport](     --exec SP_WHTDetailReport '2023-01-01','2023-02-28',33  
@FromDate datetime,    
@ToDate datetime,  
@tenantId int=null,
@code nvarchar(3)='0')    
as    
begin    
declare @WhtDetailReport as table    
(SLNO int identity(1,1),    
Typeofpayments nvarchar(100),    
NameofPayee nvarchar(100),    
country nvarchar(100),    
paymentdate datetime,    
totalamountPaid decimal(18,2),    
taxrate decimal(18,2),    
withholdingtaxamount decimal(18,2)  ,  
StandardRate decimal(18,2),  
DTTrate decimal(18,2),  
AffiliationStatus nvarchar(30) ,  
Obtainedrequireddocuments nvarchar(30)  

 

)    

 

--select * from VI_ImportMasterFiles_Processed   
--insert into @WhtDetailReport (TypeofPayments,NameofPayee,country,paymentdate,totalamountPaid,taxrate,withholdingtaxamount)     
--values ('TypeofPayment','NameofthePayee','SA',GETDATE(),1000,15,100);    

 
 if @code = '0'
  begin
	  insert into @WhtDetailReport (TypeofPayments,NameofPayee,country,paymentdate,totalamountPaid,taxrate,withholdingtaxamount,StandardRate,DTTrate,AffiliationStatus,Obtainedrequireddocuments)       
	  select v.Natureofservices,v.BuyerName,v.BuyerCountryCode,v.issuedate,round(v.LineAmountInclusiveVAT,2),p.effrate,    
	  round(v.LineAmountInclusiveVAT*p.effrate/100,2),p.lawrate,p.standardrate,AffiliationStatus ,VATDeffered     
	  from VI_importstandardfiles_Processed v     
	  inner join vi_paymentWHTrate p on v.UniqueIdentifier = p.uniqueidentifier   
	  where v.TenantId=@tenantId and v.IssueDate >= @fromdate and v.IssueDate<= @todate and v.InvoiceType like 'WHT%'  order by natureofservices,buyername,issuedate    
   end
else
   begin
   	  insert into @WhtDetailReport (TypeofPayments,NameofPayee,country,paymentdate,totalamountPaid,taxrate,withholdingtaxamount,StandardRate,DTTrate,AffiliationStatus,Obtainedrequireddocuments)       
	  select v.Natureofservices,v.BuyerName,v.BuyerCountryCode,v.issuedate,round(v.LineAmountInclusiveVAT,2),p.effrate,    
	  round(v.LineAmountInclusiveVAT*p.effrate/100,2),p.lawrate,p.standardrate,AffiliationStatus ,VATDeffered     
	  from VI_importstandardfiles_Processed v     
	  inner join vi_paymentWHTrate p on v.UniqueIdentifier = p.uniqueidentifier   
	  inner join NatureofServices n on v.NatureofServices = n.name
	  where v.TenantId=@tenantId and v.IssueDate >= @fromdate and v.IssueDate<= @todate and v.InvoiceType like 'WHT%'  and n.code = @code order by natureofservices,buyername,issuedate    
    end
 

  select SLNO,Typeofpayments,NameofPayee,country,FORMAT(paymentdate,'dd-MM-yyyy') as paymentdate,totalamountPaid,  
  taxrate,withholdingtaxamount,StandardRate,DTTrate,AffiliationStatus,case when (Obtainedrequireddocuments =1) then 'Yes' else 'No' end as Obtainedrequireddocuments from @WhtDetailReport    

 

--  select * from VI_PaymentWHTRate   
end
GO
