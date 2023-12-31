USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_ReconExemptPurchaseReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create            procedure [dbo].[VI_ReconExemptPurchaseReport]    -- exec VI_ReconExemptPurchaseReport '2022-09-01', '2022-09-30'                
(                
@fromdate date,                
@todate date,              
@tenantId int=null              
)                
as                
Begin                
-- (6,'Nominal Supplies',4,4,3)              
               
select 7,'Exempt Purchases',                
      isnull(sum((case when (invoicetype like 'Purchase%') and VatCategoryCode in ('E')                
        then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)                 
        else 0 end))               
 ,0) as inneramount,null,3                
 from VI_importstandardfiles_Processed sales                
 where  Invoicetype like 'Purchase%'   and VatCategoryCode in ('E')               
and issuedate >= @fromdate and issuedate <= @todate               
and TenantId=@tenantId ;                
end
GO
