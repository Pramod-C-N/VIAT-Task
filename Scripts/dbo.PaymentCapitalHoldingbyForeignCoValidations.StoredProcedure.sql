USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PaymentCapitalHoldingbyForeignCoValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create    procedure [dbo].[PaymentCapitalHoldingbyForeignCoValidations]  -- exec PaymentCapitalHoldingbyForeignCoValidations 859256      
(      
@BatchNo numeric      
)      
as      
begin      
      
    
begin      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 117   
end      
begin      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)       
select tenantid,@batchno,uniqueidentifier,'0','Holding by Foreign Co can not be blank if nature of service is dividend.',117,0,getdate() from ImportBatchData       
where Invoicetype like 'WHT%' and batchid = @batchno and trim(upper(NatureofServices))= 'DIVIDEND PAYMENTS' and (PerCapitaHoldingForiegnCo is null or PerCapitaHoldingForiegnCo='')  
end      
      
end
GO
