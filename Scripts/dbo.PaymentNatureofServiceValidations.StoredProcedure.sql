USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PaymentNatureofServiceValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
        
CREATE       procedure [dbo].[PaymentNatureofServiceValidations]  -- exec [PaymentNatureofServiceValidations] 155123        
(        
@BatchNo numeric,    
@validStat int,  
@tenantid int  
)        
as        
begin        
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 114        
end        
        
begin        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,        
uniqueidentifier,'0','Invalid Nature of Service',114,0,getdate() from ImportBatchData         
where invoicetype like 'WHT%' and (NatureofServices  is null or NatureofServices  ='')  and batchid = @batchno          
       
end        
  
begin        
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,        
uniqueidentifier,'0','Nature of Service not present. Please change the Nature of Service.',82,0,getdate() from ImportBatchData         
where invoicetype like 'WHT%' and (NatureofServices  is not null) and upper(NatureofServices) not in (select upper(name) from  NatureofServices)  
and batchid = @batchno          
       
end    



--select * from ValidationStatus

--update ValidationStatus set ValidStat=0
GO
