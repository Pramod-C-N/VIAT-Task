USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransRule01Validations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[SalesTransRule01Validations]  -- exec SalesTransRule01Validations 2          
(          
@BatchNo numeric,
@validstat int
)          
as          
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 120          
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,          
uniqueidentifier,'0','Standard Sales should contain valid VAT Rate',120,0,getdate() from ImportBatchData           
where invoicetype like 'Sales%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) )))  like '%STANDARD' 
and concat(cast(Vatcategorycode as nvarchar(1)),cast(vatrate as decimal(5,2))) not in           
(select concat(taxcode,cast(rate as decimal(5,2))) from  taxmaster where isactive = 'Y')          
and batchid = @batchno            
end
GO
