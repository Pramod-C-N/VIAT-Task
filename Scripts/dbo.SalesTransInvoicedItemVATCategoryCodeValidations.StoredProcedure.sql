USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransInvoicedItemVATCategoryCodeValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
CREATE           procedure [dbo].[SalesTransInvoicedItemVATCategoryCodeValidations]  -- exec SalesTransInvoicedItemVATCategoryCodeValidations 187 657237            
(            
@BatchNo numeric,        
@validStat int        
)            
as     
set nocount on     
begin            
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (16,463,132)            
end            
begin            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,            
uniqueidentifier,'0','Invalid VAT Category Code',16,0,getdate() from ImportBatchData with(nolock)            
where invoicetype like 'Sales%' and (upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like '%EXPORT'         
--and VatCategoryCode not in     (select code from  taxcategory)
and VatCategoryCode not like 'Z%' or 
upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) not like '%EXPORT'         
and VatCategoryCode not in     (select code from  taxcategory))

and batchid = @batchno              
end            
            
begin            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,            
uniqueidentifier,'0','For invoice type Nominal value cannot be greater than 200. Value given is ' +  cast(NetPrice as varchar),463,0,getdate() from ImportBatchData with(nolock)             
where invoicetype like 'Sales%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like '%NOMINAL' and             
NetPrice > 200             
and batchid = @batchno              
            
end        
        
begin            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,            
uniqueidentifier,'0','Invalid VAT Category Code for '+upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))),132,0,getdate()         
from ImportBatchData with(nolock)  where invoicetype like 'Sales%' and         
upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) not like '%NOMINAL' and             
Vatcategorycode = 'O'             
and batchid = @batchno              
            
end
GO
