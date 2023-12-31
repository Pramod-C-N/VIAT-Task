USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransOriginalIssueDateValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
        
        
CREATE         procedure [dbo].[CreditNoteTransOriginalIssueDateValidations]  -- exec CreditNoteTransOriginalIssueDateValidations 766,'2023-01-29','2023-01-29'                                      
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
@finenddate date  ,        
@BusinessStartDate date        
                              
                              
                             
                              
begin                              
set @finstartdate = (select top 1 EffectiveFromDate from  financialyear with(nolock) where isactive = 1 and tenantid in (select tenantid from ImportBatchData with(nolock) where BatchId=@batchno))                              
                              
set @finenddate = (select top 1 EffectiveTillEndDate from  financialyear with(nolock) where isactive = 1 and tenantid in (select tenantid from ImportBatchData with(nolock) where BatchId=@batchno))                              
         
 set @BusinessStartDate = (select top 1 RegistrationDate  from TenantDocuments with(nolock) where upper(trim(DocumentType)) = 'CRN' and TenantId = @tenantid)        
        
    if @BusinessStartDate is null          
 begin        
  set @BusinessStartDate = (select top 1 RegistrationDate  from TenantDocuments with(nolock) where upper(trim(DocumentType)) = 'VAT' and TenantId = @tenantid)        
 end        
        
end           
        
begin                              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (37,125,250,400,401,403,404,456,622)                              
end         
        
if @validstat = 1         
begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (402)            
        
end        
                              
begin                              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                             
select tenantid,@batchno,uniqueidentifier,'0','Original Supply Date cannot be greater than current date',250,0,getdate() from ImportBatchData with(nolock)                              
where (OrignalSupplyDate > getdate()) and invoicetype like 'Credit%' and OrignalSupplyDate is not null and batchid = @batchno     and tenantid=@tenantid                           
     
--   insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                     
--select tenantid,@batchno,uniqueidentifier,'0','Incorrect Issue Date Format',456,0,getdate() from ImportBatchData   with (nolock)     
--where invoicetype like 'Credit%' and (IssueDate is null or len(IssueDate)=0)  and batchid = @batchno and tenantid=@tenantid    
end     
            
                              
begin                              
insert into importstandardfiles_errorlists                            
(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                             
select tenantid,@batchno,uniqueidentifier,'0','Incorrect Original Supply Date Format',37,0,getdate()                             
from ImportBatchData  with(nolock)                             
where invoicetype like 'Credit%' and (OrignalSupplyDate  is null  or len(orignalsupplydate) = 0 ) and batchid = @batchno   and tenantid=@tenantid                             
                            
end          
begin            
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select i.tenantid,@batchno,i.uniqueidentifier,'0','Invoice Issue Date cannot be less than Supply Date',400,0,getdate() from ImportBatchData i with(nolock)        
where i.invoicetype like 'Credit%' and (i.issuedate < i.SupplyDate)  and i.batchid = @batchno   and tenantid=@tenantid           
            
end          
        
begin              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select tenantid,@batchno,uniqueidentifier,'0','Supply Date is outside Upload period',401,0,getdate() from ImportBatchData with(nolock)              
where invoicetype like 'Credit%' and (Supplydate > @todate and Supplydate < @fmdate )  and batchid = @batchno and tenantid=@tenantid              
              
end          
                    
begin                              
insert into importstandardfiles_errorlists                            
(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                             
select tenantid,@batchno,uniqueidentifier,'0','Original Supply Date cannot be greater than credit note date',125,0,getdate()                             
from ImportBatchData with(nolock)                              
where invoicetype like 'Credit%' and OrignalSupplyDate > issuedate   and batchid = @batchno  and tenantid=@tenantid                              
                              
end                              
            
 if @validstat = 1            
begin          
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)           
select i.tenantid,@batchno,i.uniqueidentifier,'0','Supply Date cannot be prior to business start date',402,0,getdate() from ImportBatchData i with(nolock)         
where i.invoicetype like 'Credit%' and (i.supplydate < @BusinessStartDate)  and i.batchid = @batchno   and tenantid=@tenantid             
end        
        
begin        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select i.tenantid,@batchno,i.uniqueidentifier,'0','Supply date is prior to last Quarter/Month Filing Date, File this value by Ammending the Return',403,0,getdate() from ImportBatchData i with(nolock)        
inner join tenantbasicdetails t with(nolock) on i.tenantid = t.tenantid        
where i.invoicetype like 'Credit%' and (t.LastReturnFiled is not null and i.SupplyDate <= t.LastReturnFiled) and         
VATLineAmount > 5000        
and i.batchid = @batchno              
        
end        
        
begin        
        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)         
select i.tenantid,@batchno,i.uniqueidentifier,'0','Supply date is prior to last Quarter/Month Filing Date, use (Box 14 of VAT Returns - Adjustment from previous period)',404,0,getdate()         
from ImportBatchData i with(nolock) inner join tenantbasicdetails t with(nolock) on i.tenantid = t.tenantid        
where i.invoicetype like 'Credit%' and (t.LastReturnFiled is not null and i.SupplyDate <= t.LastReturnFiled) and         
VATLineAmount <= 5000 and i.batchid = @batchno             
        
end        
        
        
        
begin                              
insert into importstandardfiles_errorlists                            
(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                             
select tenantid,@batchno,uniqueidentifier,'0','Original Supply Date cannot be prior to 5 years',126,0,getdate()                             
from ImportBatchData                               
where invoicetype like 'Credit%' and (orignalsupplydate > '1900-01-01 00:00:00.0000000' and datediff(YEAR,OrignalSupplyDate, issuedate) > 5)   and batchid = @batchno                                
   and tenantid=@tenantid     

if @validstat = 1 
begin
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                     
select tenantid,@batchno,uniqueidentifier,'0','Invalid Original Supply Date Reference',622,0,getdate() from importbatchdata                     
--select concat(BillingReferenceId,cast(OrignalSupplyDate as nvarchar)),tenantid,uniqueidentifier,'0','Invalid Original Supply Date',230,0,getdate() from importbatchdata                     
where invoicetype like 'Credit%' and  BillingReferenceId is not null and orignalsupplydate is not null and       
concat(BillingReferenceId,cast(format(OrignalSupplyDate,'dd-MM-yyyy') as nvarchar)) not in                
  (select concat(InvoiceNumber,cast(format(SupplyDate,'dd-MM-yyyy') as nvarchar)) from VI_importstandardfiles_Processed where invoicetype like 'Sales%'       
  and InvoiceNumber in (select InvoiceNumber from VI_importstandardfiles_Processed where invoicetype like 'Sales%' and tenantid = @tenantid) )         
and batchid = @batchno  and tenantid = @tenantid 
end                              
end                    
                    
                    
end
GO
