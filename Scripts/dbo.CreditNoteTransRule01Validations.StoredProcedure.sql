USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransRule01Validations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create        procedure [dbo].[CreditNoteTransRule01Validations]  -- exec CreditNoteTransRule01Validations 2          
(          
@BatchNo numeric,
@validstat int
)          
as          
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 61          
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,          
uniqueidentifier,'0','Credit Note should contain valid VAT Rate',61,0,getdate() from ImportBatchData           
where invoicetype like 'Credit%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) )))  like '%STANDARD' 
and concat(cast(Vatcategorycode as nvarchar(1)),cast(vatrate as decimal(5,2))) not in           
(select concat(taxcode,cast(rate as decimal(5,2))) from  taxmaster where isactive = 'Y')          
and batchid = @batchno            
end
GO
