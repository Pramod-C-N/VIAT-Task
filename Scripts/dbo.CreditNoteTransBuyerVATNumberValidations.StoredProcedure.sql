USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNoteTransBuyerVATNumberValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[CreditNoteTransBuyerVATNumberValidations]  -- exec CreditNoteTransBuyerVATNumberValidations 2    
(      
@BatchNo numeric,    
@validstat int,    
@tenantid int    
)      
as      
   
begin      
      
declare @Validformat nvarchar(100) = null      
      
begin      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (54,182)      
end      
      
begin       
set @Validformat = (select validformat from documentmaster where code='VAT' and tenantid = @tenantid)      
      
end      
      
      
if @Validformat is not null      
   begin      
      
 --insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,creationtime)       
 -- select tenantid,@batchno,uniqueidentifier,'0','Invalid VAT Number',54,0,getdate()      
 --from ImportBatchData where invoicetype like 'Credit Note%' and transtype ='Sales' and buyervatcode <> ''       
 --and  BuyerVatCode NOT in       
 --(select DocumentNumber from CustomerDocuments where DocumentTypeCode='VAT' )      
 -- and batchid = @batchno       
      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,creationtime)       
 select tenantid,@batchno,uniqueidentifier,'0','Same VAT Number for Multiple Buyer Name',182,0,getdate()      
 from ImportBatchData  with(nolock) where invoicetype like 'Credit Note%' and transtype ='Sales' and buyervatcode <> ''  and BuyerName <> ''     
 and upper(trim(RIGHT(invoicetype , LEN(invoicetype) - CHARINDEX('-', invoicetype) )))     
    in (select upper(i.invoice_flags) from invoiceindicators i with(nolock) inner join TenantBusinessSupplies b with(nolock) on i.salestype = b.BusinessSupplies     
 where i.salestype = 'Domestic')     
 and  BuyerVatCode in       
 (select v.buyervatcode from (select buyername,BuyerVatCode from ImportBatchData where batchid = @batchno and   
  (buyervatcode = '' or buyervatcode is null) group by BuyerVatCode,BuyerName ) v group by v.buyervatcode having count(*)>1)  
  and batchid=@BatchNo and TenantId=@tenantid      
    
      
end      
      
      
end
GO
