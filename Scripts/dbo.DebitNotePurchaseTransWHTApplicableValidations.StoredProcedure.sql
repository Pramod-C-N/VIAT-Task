USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransWHTApplicableValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  procedure [dbo].[DebitNotePurchaseTransWHTApplicableValidations](    
@batchno int,    
@tenantid int,    
@validstat int    
    
)    
as    
begin    
   delete from importstandardfiles_ErrorLists where Batchid=@batchno and ErrorType in (570)    
   insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)     
   select tenantid,@batchno,                                  
 uniqueidentifier,'0','Invalid WHT applicable',570,0,getdate() from importbatchdata  with(nolock)     
 where upper(InvoiceType) like 'DN PURCHASE%' and upper(InvoiceType) like '%IMPORT%' and (upper(purchasecategory) like 'GOOD%') and WHTApplicable not in (0, 1)   
 and batchid=@batchno and Tenantid=@tenantid  
     
end
GO
