USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransBuyerMasterCodeValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[DebitNoteTransBuyerMasterCodeValidations]  -- exec DebitNoteTransBuyerMasterCodeValidations 657237            
(            
@BatchNo numeric    ,  
@validstat int  
)            
as  
set nocount on 
begin    
--if @validstat=1  
  
begin            
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 154            
end            
begin            
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)             
select tenantid,@batchno,uniqueidentifier,'0','Invalid Buyer Master Code',154,0,getdate() from importbatchdata  with(nolock)           
where invoicetype like 'Debit%' and BuyerMasterCode is not null and BuyerMasterCode<> '' and             
buyermastercode not in (select id from  CompanyProfiles with(nolock)  ) and batchid = @batchno              
end            
end
GO
