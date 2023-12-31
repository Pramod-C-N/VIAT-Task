USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransLineAmountInclusiveVATValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[SalesTransLineAmountInclusiveVATValidations]  -- exec SalesTransLineAmountInclusiveVATValidations 657237  
(  
@BatchNo numeric,  
@validstat int,
@tenantid int
)  
as  
begin  
  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (22,187,307)  
end  
  
begin  
  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
select tenantid,@batchno,uniqueidentifier,'0','Invalid Line Amount Inclusive VAT',22,0,getdate() from ImportBatchData   
where invoicetype like 'Sales Invoice%' and LineAmountInclusiveVAT <> (NetPrice * Quantity) + VATLineAmount and batchid = @batchno  and tenantid = @tenantid 
  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
select tenantid,@batchno,uniqueidentifier,'0','Simplified Invoice should be less than 1000 SAR',187,0,getdate() from ImportBatchData   
where invoicetype like 'Sales Invoice%' and LineAmountInclusiveVAT > 1000 
and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like '%SIMPLIFIED%'  
and batchid = @batchno   and tenantid = @tenantid
  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
select tenantid,@batchno,uniqueidentifier,'0','Amount less than 1000SAR, is this a Simplified Invoice',307,0,getdate() from ImportBatchData   
where invoicetype like 'Sales Invoice%' and LineAmountInclusiveVAT < 1000 and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) = 'Standard'  
and batchid = @batchno   
  
end
GO
