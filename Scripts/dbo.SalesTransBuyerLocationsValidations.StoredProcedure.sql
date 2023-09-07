USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransBuyerLocationsValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         procedure [dbo].[SalesTransBuyerLocationsValidations]  -- exec SalesTransBuyerLocationValidations 2        
(        
@BatchNo numeric,    
@validstat int    
)        
as        
begin        
        
        
begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in( 184,90)        
end        
begin         
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','For Exports country code cannot be '+ buyercountrycode,184,0,getdate() from ImportBatchData         
where InvoiceType  like 'Sales%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) = 'EXPORT'    
and 
--concat(tenantid,
cast(left(BuyerCountryCode,2) as nvarchar) in (select Alphacode2 from currencymapping) and batchid = @batchno          
    
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','For Domestic Sales Country Code cannot be '+buyercountrycode,90,0,getdate() from ImportBatchData         
where InvoiceType  like 'Sales%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) <> 'EXPORT'    
and cast(left(BuyerCountryCode,2) as nvarchar) not in (select Alphacode2 from currencymapping) and batchid = @batchno          
    
end        
        
end
GO
