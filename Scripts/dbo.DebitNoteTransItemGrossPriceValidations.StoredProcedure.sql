USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNoteTransItemGrossPriceValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create        procedure [dbo].[DebitNoteTransItemGrossPriceValidations]-- exec DebitNoteTransItemGrossPriceValidations 657237              
(              
@BatchNo numeric    ,
@validstat int
)              
as    
set nocount on  
begin              
              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 210             
end              
              
begin              
              
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','Gross Price Cannot be <= Zero',210,0,getdate() from ImportBatchData with(nolock)              
where invoicetype like 'Debit Note%' and len(Grossprice) <=0 and batchid = @batchno               
              
end
GO
