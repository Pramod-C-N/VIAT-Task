USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PurchaseTransWHTApplicableValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[PurchaseTransWHTApplicableValidations](  
@batchno int,  
@tenantid int,  
@validstat int  
  
)  
as  
begin  
   delete from importstandardfiles_ErrorLists where Batchid=@batchno and ErrorType in (494)  
   insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
   select tenantid,@batchno,                                
 uniqueidentifier,'0','Invalid WHT applicable',494,0,getdate() from importbatchdata  with(nolock)   
 where upper(InvoiceType) like 'PURCHASE%' and upper(InvoiceType) like '%IMPORT%' and (upper(purchasecategory) like 'GOOD%') and WHTApplicable not in (0, 1) 
 and batchid=@batchno and Tenantid=@tenantid
   
end
GO
