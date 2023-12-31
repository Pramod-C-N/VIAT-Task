USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransRule03Validations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create       procedure [dbo].[PurchaseTransRule03Validations]  -- exec PurchaseTransRule03Validations 657237      
(      
@BatchNo numeric,
@validstat int 
)      
as      
begin      
 delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 516     
 end      
 begin      
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) 
 select tenantid,@batchno,      
 uniqueidentifier,'0','Net Price for nominal cannot be greater than 200.00',516,0,getdate() from ImportBatchData        
where invoicetype like 'Purchase%' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like 'NOMINAL%' and     
upper(buyercountrycode) like 'SA%' and NetPrice > 200.00  
and batchid = @batchno        
 end
GO
