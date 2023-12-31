USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransNatureofServicesValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create       procedure [dbo].[DebitNotePurchaseTransNatureofServicesValidations]  -- exec [DebitNotePurchaseTransNatureofServicesValidations] 859256                  
(                  
@BatchNo numeric ,      
@validtstat int      
)                  
as                  
begin                  
                  
                  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (573 ,574)               
                  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                   
select tenantid,@batchno,uniqueidentifier,'0','Invalid Nature of Services',573,0,getdate() from ImportBatchData                   
where Invoicetype like 'DN Purchase%' and whtapplicable like 'Y%'                
and upper(natureofservices) not in (select upper(name) from natureofservices)    and   batchid = @batchno         
        
                  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                   
select tenantid,@batchno,uniqueidentifier,'0','if WHT Applicable ,Nature of Services Required ',574,0,getdate() from ImportBatchData                   
where Invoicetype like 'DN Purchase%' and whtapplicable like 'Y%'                
and (len(trim(natureofservices)) =0 or NatureofServices is null)   and   batchid = @batchno        
        
                  
end
GO
