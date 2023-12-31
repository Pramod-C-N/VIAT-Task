USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransRule04Validations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
     
  CREATE      procedure [dbo].[SalesTransRule04Validations]  -- exec SalesTransRule04Validations 23,1,22       
(        
@BatchNo numeric,    
@validstat int,    
@tenantid int    
)        
as        
begin      
declare @tenentVatId nvarchar(20) = null;    
    
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 123      
    
set @tenentVatId = (select top 1  t.VATID from TenantBasicDetails t  where tenantid = @tenantid) -- on i.TenantId=t.tenantid where i.BatchId=@BatchNo)    
    
    
if @tenentVatId is not null       
 begin        
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
select i.TenantId,@batchno,i.UniqueIdentifier,'0','For Internal Supply Invoice Type should be <<Self Billed>> and VAT Catg should be <<O>>',123,0,getdate() from ImportBatchData  i       
where invoicetype like 'Sales%' and buyervatcode <> ''  and BuyerName <> ''     
 and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) )))     
    in (select upper(i.invoice_flags) from invoiceindicators i inner join TenantBusinessSupplies b on i.salestype = b.BusinessSupplies     
 where i.salestype = 'Domestic' )
 --(and i.tenantid=@tenantid)    
 and BuyerVatCode=@tenentVatId and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) <> 'SELF BILLED' and    
 upper(trim(i.Vatcategorycode)) <> 'O'  and batchid = @batchno    
 end        
    
end

--select top 1  t.VATID from TenantBasicDetails t  where tenantid = 43

--select * from importbatchdata where batchid=2590
--select * from invoiceindicators where tenantid=43
GO
