USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteInvoiceIssueDateValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    procedure [dbo].[CreditNoteInvoiceIssueDateValidations]  -- exec CNInvoiceIssueDateValidations 657237          
(          
@BatchNo numeric          
)          
as          
begin          
          
declare @finstartdate date,          
@finenddate date          
          
          
begin          
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (31,32,33)          
end          
          
begin          
set @finstartdate = (select EffectiveTillEndDate from  financialyear where isactive = 1 and tenantid in (select tenantid from ImportBatchData where BatchId=@batchno))          
          
set @finenddate = (select EffectiveTillEndDate from  financialyear where isactive = 1 and tenantid in (select tenantid from ImportBatchData where BatchId=@batchno))          
          
end          
          
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','Invoice Date cannot be greater than current date',32,0,getdate() from ImportBatchData           
where (issuedate > getdate())  and batchid = @batchno            
          
end          
          
begin          
insert into importstandardfiles_errorlists        
(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select tenantid,@batchno,uniqueidentifier,'0','Invoice Date should be within active financial year',33,0,getdate()         
from ImportBatchData           
where invoicetype like 'Credit%' and (issuedate < @finstartdate  or IssueDate > @finenddate)  and batchid = @batchno            
          
end          
          
end
GO
