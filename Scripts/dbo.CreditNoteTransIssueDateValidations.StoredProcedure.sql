USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransIssueDateValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[CreditNoteTransIssueDateValidations]  -- exec CreditNoteTransIssueDateValidations 776,'2022-05-01','2022-05-31',0,33                 
(                
@BatchNo numeric,        
@fmdate date,        
@todate date,        
@validstat int,        
@tenantid int        
)                
as           
      
begin                
                
declare @finstartdate date,                
@finenddate date         
, @BusinessStartDate date        
                
                
begin                
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (31,32,253,145)                
end                
         
 if @validstat=1        
 begin              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (33,398)              
end        
         
begin                
set @finstartdate = (select top 1 cast(EffectiveFromDate as datetime2) from  FinancialYear with(nolock) where isactive = 1 and tenantid in (select tenantid from ImportBatchData with(nolock) where BatchId=@batchno))                
                
set @finenddate = (select top 1 cast(EffectiveTillEndDate as datetime2) from  FinancialYear with(nolock) where isactive = 1 and tenantid in (select tenantid from ImportBatchData with(nolock) where BatchId=@batchno))                
                
set @BusinessStartDate = (select top 1 RegistrationDate  from TenantDocuments with(nolock) where upper(trim(DocumentType)) = 'CRN' and TenantId = @tenantid)        
        
 if @BusinessStartDate is null          
    begin        
    set @BusinessStartDate = (select top 1 RegistrationDate  from TenantDocuments with(nolock) where upper(trim(DocumentType)) = 'VAT' and TenantId = @tenantid)        
    end        
-- end          
end                
                
begin         
        
set @BusinessStartDate = isnull(@businessstartdate,@finstartdate)        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)             
select tenantid,@batchno,uniqueidentifier,'0','Credit Note Date cannot be greater than current date',31,0,getdate() from ImportBatchData with(nolock)                
where invoicetype like 'Credit%' and transtype like 'Sales%' and (issuedate > getdate())  and batchid = @batchno                  
 
  insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                 
select tenantid,@batchno,uniqueidentifier,'0','Incorrect Issue Date Format',145,0,getdate() from ImportBatchData   with (nolock) 
where invoicetype like 'Credit%' and (issuedate is null or len(issuedate)=0)  and batchid = @batchno and tenantid=@tenantid
end                
            
            
begin                
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)             
select i.tenantid,@batchno,i.uniqueidentifier,'0','Credit Note Date should be within active financial year',32,0,getdate() from ImportBatchData i with(nolock)           
where i.invoicetype like 'Credit%' and (i.issuedate < @finstartdate  or i.IssueDate > @finenddate)  and i.batchid = @batchno                  
         
  end            
  if @validstat = 1            
begin          
          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select i.tenantid,@batchno,i.uniqueidentifier,'0','Credit Note Date should be greater than last Quarter/Month Filing Date',33,0,getdate() from ImportBatchData i with(nolock)         
inner join tenantbasicdetails t on i.tenantid = t.tenantid          
where i.invoicetype like 'Credit%' and (t.LastReturnFiled is not null and i.issuedate <= t.LastReturnFiled)          
and i.batchid = @batchno           
        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select i.tenantid,@batchno,i.uniqueidentifier,'0','Invoice Date cannot be prior to business start date',398,0,getdate() from ImportBatchData i  with(nolock)        
where i.invoicetype like 'Credit%' and (i.issuedate < @BusinessStartDate)  and i.batchid = @batchno         
         
          
end          
        
begin                
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
select tenantid,@batchno,uniqueidentifier,'0','Credit Note Date is outside Upload period',253,0,getdate() from ImportBatchData    with(nolock)             
where invoicetype like 'Credit%' and transtype like 'Sales%' and (issuedate > @todate and issuedate < @fmdate)  and batchid = @batchno           
        
end        
             
           
end
GO
