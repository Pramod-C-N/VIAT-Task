USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransRule03Validations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          procedure [dbo].[CreditNoteTransRule03Validations]  -- exec CreditNoteTransRule03Validations 657237            
(            
@BatchNo numeric,      
@validstat int      
)            
as        
      
begin            
 delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 127           
 end            
 begin            
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,            
 uniqueidentifier,'0','Net Price for nominal cannot be greater than 200.00',127,0,getdate() from ImportBatchData  with(nolock)            
where invoicetype like 'Credit%' and transtype = 'Sales' and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) ))) like 'NOMINAL%' and           
upper(trim(Vatcategorycode)) = 'O'   and NetPrice > 200.00        
and batchid = @batchno              
 end
GO
