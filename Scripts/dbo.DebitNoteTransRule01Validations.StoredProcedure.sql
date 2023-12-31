USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransRule01Validations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create        procedure [dbo].[DebitNoteTransRule01Validations]  -- exec DebitNoteTransRule01Validations 2          
(          
@BatchNo numeric,
@validstat int
)          
as          
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype =    62       
end          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,          
uniqueidentifier,'0','Debit Note should contain valid VAT Rate',62,0,getdate() from ImportBatchData           
where invoicetype like 'Debit%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) )))  like '%STANDARD' 
and concat(cast(Vatcategorycode as nvarchar(1)),cast(vatrate as decimal(5,2))) not in           
(select concat(taxcode,cast(rate as decimal(5,2))) from  taxmaster where isactive = 'Y')          
and batchid = @batchno            
end
GO
