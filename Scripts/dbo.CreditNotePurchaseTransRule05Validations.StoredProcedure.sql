USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreditNotePurchaseTransRule05Validations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create         PROCEDURE [dbo].[CreditNotePurchaseTransRule05Validations]  -- exec CreditNotePurchaseTransRule05Validations 657237              
(              
@BatchNo numeric,    
@validstat int    
)          
as   
set nocount on  
begin              
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (537)    
end              
    
begin              
insert into importstandardfiles_errorlists(i.tenantid,i.BatchId,i.uniqueidentifier,i.Status,i.ErrorMessage,i.Errortype,i.isdeleted,i.CreationTime)               
select i.tenantid,@Batchno,i.uniqueidentifier,'0','Credit Note Value is more than Reference Invoice Value',537,0,getdate() from importbatchdata i with(nolock)             
left outer join (select v.BillingReferenceId ,sum(v.lineamountinclusiveVAT) as cntotal,v.VatCategoryCode    
from vi_importstandardfiles_processed v inner join importbatchdata im with(nolock)   
on v.billingreferenceid = im.BillingReferenceId and v.VatCategoryCode = im.VatCategoryCode  where im.invoicetype like 'Credit%'   
and im.batchid = @batchno and v.invoicetype like 'Credit%'  and v.BatchId <> @batchno group by v.BillingReferenceId,v.VatCategoryCode ) vi   
on i.billingreferenceid = vi.billingreferenceid     
left outer join (select billingreferenceid,sum(lineamountinclusiveVAT) as cntotalbatch,VatCategoryCode  from ImportBatchData v  with(nolock)   
where v.invoicetype like 'Credit%' group by billingreferenceid,VatCategoryCode ) vm    
on i.BillingReferenceId = vm.billingreferenceid and i.VatCategoryCode = vm.vatcategorycode   
inner join (select invoicenumber,sum(lineamountinclusiveVAT) as invamt,VatCategoryCode  from vi_importstandardfiles_processed v  with(nolock)  
where v.invoicetype like 'Purchase%' group by invoicenumber,VatCategoryCode ) vs on i.billingreferenceid = vs.invoicenumber and   
i.VatCategoryCode = vs.VatCategoryCode     
where invoicetype like 'Credit%' and  isnull(vi.cntotal,0)+isnull(vm.cntotalbatch,0) > isnull(vs.invamt,0) and batchid = @batchno    
end
GO
