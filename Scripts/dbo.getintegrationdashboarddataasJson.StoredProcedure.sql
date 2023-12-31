USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getintegrationdashboarddataasJson]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE      PROCEDURE [dbo].[getintegrationdashboarddataasJson]   -- getintegrationdashboarddataasJson '2023-08-22' ,'2023-08-31' ,127,'Sales',null,null,null,null,null,null,null,null,null,null,'brady'           
@fromDate DATETIME,        
@toDate DATETIME,                            
@tenantId INT=null,          
@type nvarchar(max) = 'Sales',  
@invoicereferencenumber nvarchar(max) = null, --done  
@invoicereferncedate nvarchar(max)= null, --done  
@purchaseOrderNo nvarchar(max)= null, --done  
@customername nvarchar(max)= null, --done  
@activestatus nvarchar(max)= null,  
@currency nvarchar(max)= null,  
@payernumber NVARCHAR(MAX)= NULL,  
@salesorderno NVARCHAR(MAX)= NULL, --done  
@shiptono NVARCHAR(MAX)= NULL, --done  
@irnno nvarchar(max)=null,
@createdby nvarchar(max)=null
AS         
BEGIN        
  
declare @salesordernumber nvarchar(max)      
declare @purchaseordernumber nvarchar(max)      
declare @shiptonumber nvarchar(max)      

if(@type = 'Sales')  
BEGIN  
SELECT *  
INTO #invoice FROM  
(  
(SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
sa.PdfUrl as pdf,     
case when s.Errors is null then s.Errors else substring(s.Errors,0,len(s.errors)) end as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name,
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
c.ContactNumber + ' ' +c.Email as Contact_Details, 
s.InvoiceCurrencyCode as Currency,    
case when sa.PdfUrl is null then 'Einvoice not created' else 'Einvoice created' end as [Status],  
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
cast(sa.IRNNo as nvarchar) as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Reference_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number   ,  
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By   ,  
s.CreationTime  
FROM  
  FileUpload_TransactionHeader s  WITH (NOLOCK)  
  inner JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,UniqueIdentifier,TenantId,isOtherCharges                       
  FROM FileUpload_TransactionItem   WITH (NOLOCK)  
  GROUP BY UniqueIdentifier,TenantId,isOtherCharges) i ON s.UniqueIdentifier = i.UniqueIdentifier  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN FileUpload_TransactionParty b WITH (NOLOCK) ON s.UniqueIdentifier = b.UniqueIdentifier   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='388'  
  inner join FileUpload_TransactionContactPerson c with (nolock) on s.UniqueIdentifier = c.UniqueIdentifier   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='388'         
  inner join FileUpload_TransactionParty su with (nolock) on s.UniqueIdentifier = su.UniqueIdentifier   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='388'            
  left join dbo.SalesInvoice sa on sa.UniqueIdentifier = s.UniqueIdentifier                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.TransTypeDescription='388'
  AND s.uniqueidentifier not IN (SELECT uniqueidentifier FROM draft WHERE isSent=0 ))
  --UNION  
  --  (  
  --  SELECT BillingReferenceId as Invoice_Reference_Number  
  --,'' as pdf   
  --, Errors AS Errors  
  --,'' as Issue_date  
  --,'' as Customer_Name  
  --,'' AS Contact_Details  
  --,'Einvoice not created' as Status  
  --,'' as Currency  
  --,'' as Amount  
  --,'' as AdditionalData2  
  --,'' as Invoice_Number  
  --,'' as IRNNo  
  --,cast(format(CreationTime,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Date  
  --,'' as Sales_Order_Number  
  --,'' as Purchase_Order_Number  
  --,'' as Payer_Number  
  --,'' as Ship_To_Number  
  --, CreationTime  
  --FROM dbo.FileUpload_TransactionHeader  WHERE IRNNo=-1  and uniqueidentifier not in (select uniqueidentifier from salesinvoice)
  --AND  uniqueidentifier not IN (SELECT uniqueidentifier FROM draft ) 
  --AND TenantId=@tenantId and CAST(IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(IssueDate AS DATE)<= CAST(@toDate as Date) and TransTypeDescription='388' ) 
  union
  (SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
s.PdfUrl as pdf,     
'' as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name, 
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
c.ContactNumber + ' ' +c.Email as Contact_Details,    
s.InvoiceCurrencyCode as Currency,    
'Draft' as [Status],  
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
cast(s.id as nvarchar) as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Reference_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number   ,  
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By   ,  
s.CreationTime  
FROM  
  draft s  WITH (NOLOCK)  
  inner JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,IRNNo,TenantId,isOtherCharges                       
  FROM [dbo].[DraftItem]   WITH (NOLOCK)  
  GROUP BY IRNNo,TenantId,isOtherCharges) i ON s.id = i.IRNNo  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN [dbo].[DraftParty] b WITH (NOLOCK) ON s.id = b.IRNNo   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.IRNNo=s.id and s.TransTypeDescription='388'  
  inner join [dbo].[DraftContactPerson] c with (nolock) on s.id = c.IRNNo   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.IRNNo=s.id and s.TransTypeDescription='388'         
  inner join [dbo].[DraftParty] su with (nolock) on s.id = su.IRNNo   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.IRNNo=s.id and s.TransTypeDescription='388'            
  --left join dbo.SalesInvoice sa on sa.IRNNo = s.id                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.invoicetypecode='388' and s.isSent=0
  AND   s.uniqueidentifier not in (select uniqueidentifier from salesinvoice)
  and s.source not in ('UI','API'))  
  ) AS t;

  IF(@customername IS NOT NULL)
  BEGIN        
  DELETE FROM #invoice  WHERE Customer_Name not LIKE '%'+ @customername+'%';        
  END        
  
  IF(@invoicereferncedate IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice  WHERE Issue_date not LIKE  cast(format(cast(@invoicereferncedate as Date),'dd-MM-yyyy')as nvarchar);        
  END        
  
  IF(@salesorderno IS NOT NULL)         
  BEGIN        
  DELETE FROM #invoice  WHERE Sales_Order_Number not LIKE '%'+ @salesorderno+'%';        
  END        
  
  IF(@purchaseOrderNo IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice  WHERE Purchase_Order_Number not LIKE '%'+ @purchaseOrderNo+'%';        
  END        
  
  IF(@invoicereferencenumber IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice WHERE Invoice_Number <> @invoicereferencenumber;        
  END        
     IF(@payernumber IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice WHERE Payer_Number <> @payernumber;        
  END    
  IF(@shiptono IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice WHERE Ship_To_Number IS NOT NULL and Ship_To_Number NOT LIKE '%'+@shiptono+'%';        
  END   
  IF(@createdby IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice WHERE (Created_By IS  NULL or Created_By NOT LIKE '%'+@createdby+'%');           
  END  
IF(@irnno IS NOT NULL)        
  BEGIN        
  DELETE FROM #invoice WHERE (IRNNo IS  NULL or IRNNo NOT LIKE '%'+@irnno+'%');
  END 
  
  SELECT Invoice_Reference_Number,pdf,Errors,Invoice_Reference_Date,Customer_Name,Amount,Currency,Status,AdditionalData2,Sales_Order_Number,Purchase_Order_Number,Payer_Number,Ship_To_Number,Invoice_Number,Issue_date,IRNNo,Created_By    
  FROM #invoice  ORDER BY CreationTime desc    
  
  IF OBJECT_ID('tempdb..#invoice') IS NOT NULL DROP TABLE #invoice  
  END  
    
  if(@type = 'Credit')  
begin
select * into #Credit
from
((SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
sa.PdfUrl as pdf,     
case when s.Errors is null then s.Errors else substring(s.Errors,0,len(s.errors)) end as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name, c.ContactNumber + ' ' +c.Email as Contact_Details, 
s.InvoiceCurrencyCode as Currency,    
case when sa.PdfUrl is null then 'Einvoice not created' else 'Einvoice created' end as [Status],  
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
sa.IRNNo as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number ,
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By     

--INTO #Credit        
FROM  
  FileUpload_TransactionHeader s  WITH (NOLOCK)  
  INNER JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,UniqueIdentifier,TenantId,isOtherCharges                       
  FROM FileUpload_TransactionItem   WITH (NOLOCK)  
  GROUP BY UniqueIdentifier,TenantId,isOtherCharges) i ON s.UniqueIdentifier = i.UniqueIdentifier  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN FileUpload_TransactionParty b WITH (NOLOCK) ON s.UniqueIdentifier = b.UniqueIdentifier   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'  
  inner join FileUpload_TransactionContactPerson c with (nolock) on s.UniqueIdentifier = c.UniqueIdentifier   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'         
  inner join FileUpload_TransactionParty su with (nolock) on s.UniqueIdentifier = su.UniqueIdentifier   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'            
  left join dbo.CreditNote sa on sa.UniqueIdentifier = s.UniqueIdentifier                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.TransTypeDescription='381')
  union
  SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
sa.PdfUrl as pdf,     
'' as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name, c.ContactNumber + ' ' +c.Email as Contact_Details,
s.InvoiceCurrencyCode as Currency,    
'Draft' as [Status],  
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
sa.IRNNo as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number ,
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By   

--INTO #Credit        
FROM  
  draft s  WITH (NOLOCK)  
  INNER JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,UniqueIdentifier,TenantId,isOtherCharges                       
  FROM draftitem   WITH (NOLOCK)  
  GROUP BY UniqueIdentifier,TenantId,isOtherCharges) i ON s.UniqueIdentifier = i.UniqueIdentifier  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN draftparty b WITH (NOLOCK) ON s.UniqueIdentifier = b.UniqueIdentifier   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'  
  inner join DraftContactPerson c with (nolock) on s.UniqueIdentifier = c.UniqueIdentifier   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'         
  inner join draftparty su with (nolock) on s.UniqueIdentifier = su.UniqueIdentifier   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='381'            
  left join dbo.CreditNote sa on sa.UniqueIdentifier = s.UniqueIdentifier                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.TransTypeDescription='381'
  and s.source not in ('UI','API')) as C;
  
  IF(@customername IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit  WHERE Customer_Name not LIKE '%'+ @customername+'%';        
  END        
  
  IF(@invoicereferncedate IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit  WHERE Issue_date not LIKE  cast(format(cast(@invoicereferncedate as Date),'dd-MM-yyyy')as nvarchar);        
  END        
  
  IF(@salesorderno IS NOT NULL)         
  BEGIN        
  DELETE FROM #Credit  WHERE Sales_Order_Number not LIKE '%'+ @salesorderno+'%';        
  END        
  
  IF(@purchaseOrderNo IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit  WHERE Purchase_Order_Number not LIKE '%'+ @purchaseOrderNo+'%';        
  END          
  
  IF(@invoicereferencenumber IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit WHERE Invoice_Number <> @invoicereferencenumber;        
  END        
  
  IF(@shiptono IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit WHERE Ship_To_Number IS NOT NULL and Ship_To_Number NOT LIKE '%'+@shiptono+'%';        
  END 
   IF(@createdby IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit WHERE (Created_By IS  NULL or Created_By NOT LIKE '%'+@createdby+'%');           
  END  
  IF(@irnno IS NOT NULL)        
  BEGIN        
  DELETE FROM #Credit WHERE (IRNNo IS  NULL or IRNNo NOT LIKE '%'+@irnno+'%');
  END 
  
  SELECT * FROM #Credit      
  
  IF OBJECT_ID('tempdb..#Credit') IS NOT NULL DROP TABLE #Credit  
  end  
  
if(@type = 'Debit')  
begin 
select * INTO #Debit from
((SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
sa.PdfUrl as pdf,     
case when s.Errors is null then s.Errors else substring(s.Errors,0,len(s.errors)) end as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name, c.ContactNumber + ' ' +c.Email as Contact_Details,    
s.InvoiceCurrencyCode as Currency,    
case when sa.PdfUrl is null then 'Einvoice not created' else 'Einvoice created' end as [Status],  
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
sa.IRNNo as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number ,
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By     

--INTO #Debit        
FROM  
  FileUpload_TransactionHeader s  WITH (NOLOCK)  
  INNER JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,UniqueIdentifier,TenantId,isOtherCharges                       
  FROM FileUpload_TransactionItem   WITH (NOLOCK)  
  GROUP BY UniqueIdentifier,TenantId,isOtherCharges) i ON s.UniqueIdentifier = i.UniqueIdentifier  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN FileUpload_TransactionParty b WITH (NOLOCK) ON s.UniqueIdentifier = b.UniqueIdentifier   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'  
  inner join FileUpload_TransactionContactPerson c with (nolock) on s.UniqueIdentifier = c.UniqueIdentifier   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'         
  inner join FileUpload_TransactionParty su with (nolock) on s.UniqueIdentifier = su.UniqueIdentifier   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'            
  left join dbo.DebitNote sa on sa.UniqueIdentifier = s.UniqueIdentifier                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.TransTypeDescription='383')
  union
  (SELECT  
isnull(s.InvoiceNumber,s.BillingReferenceId) as Invoice_Reference_Number,  
sa.PdfUrl as pdf,     
'' as Errors,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Issue_date,      
b.RegistrationName as Customer_Name, c.ContactNumber + ' ' +c.Email as Contact_Details,
s.InvoiceCurrencyCode as Currency,    
'Draft' as [Status],  
cast(i.LineAmountInclusiveVAT as nvarchar(max)) as Amount,    
ISNULL(s.AdditionalData2,'0') AS AdditionalData2,    
s.InvoiceNumber as Invoice_Number,  
sa.IRNNo as IRNNo,  
cast(format(s.IssueDate,'dd-MM-yyyy') as nvarchar(max)) as Invoice_Date,    
(select salesorderno from openjson(s.additionaldata2) with (salesorderno nvarchar(max) '$.original_order_number')) as Sales_Order_Number,    
(select purchaseorderno from openjson(s.additionaldata2) with (purchaseorderno nvarchar(max) '$.purchase_order_no')) as Purchase_Order_Number,    
c.ContactNumber as Payer_Number,  
(select shiptonumber from openjson(b.additionaldata1) with (shiptonumber nvarchar(max) '$.crNumber')) as Ship_To_Number,
(select createdby from openjson(s.additionaldata4) with (createdby nvarchar(max) '$.created_by')) as Created_By   
--INTO #Debit        
FROM  
  draft s  WITH (NOLOCK)  
  INNER JOIN (SELECT SUM(LineAmountInclusiveVAT) AS LineAmountInclusiveVAT,UniqueIdentifier,TenantId,isOtherCharges                       
  FROM draftitem   WITH (NOLOCK)  
  GROUP BY UniqueIdentifier,TenantId,isOtherCharges) i ON s.UniqueIdentifier = i.UniqueIdentifier  AND ISNULL(i.tenantid,0)=ISNULL(@tenantId,0) AND ISNULL(s.tenantid,0)=ISNULL(@tenantId,0) and i.isOtherCharges=0  
  --INNER JOIN IRNMaster m WITH (NOLOCK) ON s.UniqueIdentifier = m.UniqueIdentifier   AND ISNULL(m.tenantid,0)=ISNULL(@tenantId,0)                        
  INNER JOIN draftparty b WITH (NOLOCK) ON s.UniqueIdentifier = b.UniqueIdentifier   AND ISNULL(b.tenantid,0)=ISNULL(@tenantId,0)                        
  AND b.Type = 'Buyer' and b.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'  
  inner join DraftContactPerson c with (nolock) on s.UniqueIdentifier = c.UniqueIdentifier   and Isnull(c.tenantid,0)=isnull(@tenantId,0)                        
  and c.Type = 'Buyer'  and c.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'         
  inner join draftparty su with (nolock) on s.UniqueIdentifier = su.UniqueIdentifier   and Isnull(su.tenantid,0)=isnull(@tenantId,0)                        
  and su.Type = 'Supplier'  and su.UniqueIdentifier=s.UniqueIdentifier and s.TransTypeDescription='383'            
  left join dbo.DebitNote sa on sa.UniqueIdentifier = s.UniqueIdentifier                  
  where CAST(s.IssueDate AS DATE)>=CAST(@fromDate as Date) and CAST(s.IssueDate AS DATE)<= CAST(@toDate as Date) and s.TransTypeDescription='383'
  and s.source not in ('UI','API'))) as d ;         
  
  IF(@customername IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit  WHERE Customer_Name not LIKE '%'+ @customername+'%';        
  END        
  
  IF(@invoicereferncedate IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit  WHERE Issue_date not LIKE  cast(format(cast(@invoicereferncedate as Date),'dd-MM-yyyy')as nvarchar);        
  END        
  
 IF(@salesorderno IS NOT NULL)         
  BEGIN        
  DELETE FROM #Debit  WHERE Sales_Order_Number not LIKE '%'+ @salesorderno+'%';        
  END        
  
  IF(@purchaseOrderNo IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit  WHERE Purchase_Order_Number not LIKE '%'+ @purchaseOrderNo+'%';        
  END             
  
  IF(@invoicereferencenumber IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit WHERE Invoice_Number <> @invoicereferencenumber;        
  END        
  
  IF(@shiptono IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit WHERE Ship_To_Number IS NOT NULL and Ship_To_Number NOT LIKE '%'+@shiptono+'%';        
  END
   IF(@createdby IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit WHERE (Created_By IS  NULL or Created_By NOT LIKE '%'+@createdby+'%');          
  END  
  IF(@irnno IS NOT NULL)        
  BEGIN        
  DELETE FROM #Debit WHERE (IRNNo IS  NULL or IRNNo NOT LIKE '%'+@irnno+'%');
  END 
  
  SELECT * FROM #Debit
  
  IF OBJECT_ID('tempdb..#Debit') IS NOT NULL DROP TABLE #Debit  
  END  
    
  end
GO
