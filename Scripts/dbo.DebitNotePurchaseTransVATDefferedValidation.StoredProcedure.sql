USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DebitNotePurchaseTransVATDefferedValidation]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  procedure [dbo].[DebitNotePurchaseTransVATDefferedValidation]   
(  
 @batchno int,  
 @tenantid int,  
 @validstat int  
)  
as  
begin  
begin                      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in(566)                    
end      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)                       
            
select tenantid,@batchno,uniqueidentifier,'0','Invalid VAT deffered',566,0,getdate() from  ImportBatchData  
where upper(InvoiceType) like 'DN PURCHASE%' AND upper(InvoiceType) like '%IMPORT%' and upper(PurchaseCategory) like 'GOOD%' and upper(BuyerCountryCode) not like 'SA%' 
and VATDeffered=1 and VatCategoryCode not like 'S' and BatchId=@batchno and TenantId=@tenantid
end
GO
