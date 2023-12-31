USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SP_WHTreturnReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE    PROCEDURE [dbo].[SP_WHTreturnReport](@FromDate datetime,    --exec SP_WHTreturnReport '2023-01-01','2023-06-01',52
@ToDate datetime,
@tenantId int=null)  
as  
begin  
declare @WhtReturnReport1 as table  
(slno1 int identity(1,1),  
Typeofpayments1 nvarchar(100),  
NameofPayee1 nvarchar(100),  
paymentdate1 datetime,  
totalamountPaid1 decimal(18,2),  
taxrate1 decimal(18,2),  
taxDue1 decimal(18,2)  
)  

declare @WhtReturnReport as table  
(slno int identity(1,1),  
Typeofpayments nvarchar(100),  
NameofPayee nvarchar(100),  
paymentdate datetime,  
totalamountPaid decimal(18,2),  
taxrate decimal(18,2),  
taxDue decimal(18,2),
ordcode nvarchar(3),
style int
)  

--insert into @WhtReturnReport (TypeofPayment,NameofthePayee,paymentdate,totalamount,taxrate,taxdue)   
--values ('TypeofPayment','NameofthePayee',GETDATE(),1000,15,100);  
   
   
 insert into @WhtReturnReport1 (Typeofpayments1,totalamountPaid1,taxDue1)   
 select v.Natureofservices,sum(v.LineAmountInclusiveVAT) as totalamount,  
  sum(round(v.LineAmountInclusiveVAT*p.standardrate/100,2)) as taxdue    
  from VI_importstandardfiles_Processed v   
  inner join vi_paymentWHTrate p on v.UniqueIdentifier = p.uniqueidentifier  
  where v.TenantId=@tenantId and v.IssueDate >= @fromdate and v.IssueDate <= @todate and v.InvoiceType like 'WHT%'    
  group by v.NatureofServices   
  order by v.NatureofServices  
   

 insert into @WhtReturnReport (Typeofpayments,NameofPayee,totalamountPaid,taxDue,ordcode,style )   
 select n.description,'SCH-'+n.Code,v.totalamountpaid1,  
  v.taxdue1,n.code,0    
  from NatureofServices n   
  inner join @whtreturnreport1 v on upper(trim(v.typeofpayments1)) = upper(trim(n.name))  
  where n.code <> '0'  and n.TenantId is null order by n.code
    
 insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue,ordcode,style)   
 select n.description,' ',null,0,0,0,n.code ,0  
  from natureofservices n
  where n.tenantid is null and n.code <> '0' and n.code not in (select ordcode from @whtreturnreport) order by n.code
 
insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue,ordcode,style)   
 select 'Monthly Total',' ',null,sum(totalamountpaid),0,sum(taxdue),'96',0   
  from @WhtReturnReport 
  where ordcode in ('01','02','03','04','05','06','07','08','09','10','11','12','13')
 
insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue,ordcode,style)   
 values ('Delay Fint at 1% for each 30 days of delay',' ',null,0,0,0,'97',0) 

insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue,ordcode,style)   
 values ('Evasion Fine',' ',null,0,0,0,'98',0) 

 insert into @WhtReturnReport (Typeofpayments,NameofPayee,paymentdate,totalamountPaid,taxrate,taxDue,ordcode,style)   
 select 'Total tax and fine due (14 + 15 + 16)',' ',null,sum(totalamountpaid),0,sum(taxdue),'99',1    
  from @WhtReturnReport 
  where ordcode in ('96','97','98')

  select row_number() over (order by ordcode) as slno,Typeofpayments,NameofPayee,FORMAT(paymentdate,'dd-MM-yyyy') as paymentdate,totalamountPaid,taxrate,taxDue,style from @WhtReturnReport order by ordcode   
  
end
GO
