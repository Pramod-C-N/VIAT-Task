USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransRule02Validations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[SalesTransRule02Validations]  -- exec SalesTransRule02Validations 2              
(              
@BatchNo numeric,  
@validstat int  
)              
as              
begin              
 delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 121              
 end              
 begin              
 insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) select tenantid,@batchno,              
 uniqueidentifier,'0','Not valid Exemption Reason for given category',121,0,getdate() from ImportBatchData               
 where invoicetype like 'Sales%' and VatCategoryCode in ('E','Z','O') and VatExemptionReasonCode is null           
 and batchid = @batchno                
 end
GO
