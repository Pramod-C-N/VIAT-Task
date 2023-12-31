USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CustomerMasterBusinessPurchasesValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create           procedure [dbo].[CustomerMasterBusinessPurchasesValidations]  -- exec CustomerMasterBusinessPurchasesValidations 462453                 
(                 
@tenantid numeric,      
@batchno int,  
@validstat int  
)                 
as                 
begin   
  
if @validstat=1  
begin                 
delete from importmaster_ErrorLists where tenantid=@tenantid and errortype = 297             
end                 
  
if @validstat=1          
begin                 
insert into importmaster_ErrorLists(tenantid,Batchid,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                  
select tenantid,@batchno,uniqueidentifier,'0','Invalid Business Purchases',297,0,getdate() from ImportMasterBatchData             
 where upper(businesspurchase) <>'ALL' and  upper(businesspurchase) not in (select Upper(Purchasetype) from invoiceindicators)     and batchid=@batchno    
end                 
end
GO
