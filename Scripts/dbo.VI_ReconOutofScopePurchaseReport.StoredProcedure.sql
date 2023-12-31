USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_ReconOutofScopePurchaseReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create     procedure [dbo].[VI_ReconOutofScopePurchaseReport]    -- exec VI_ReconOutofScopePurchaseReport '2022-09-01', '2022-09-30'                  
(                  
@fromdate date,                  
@todate date,                
@tenantId int=null                
)                  
as                  
Begin                  
-- (9,'Out of Scope Supplies',4,4,3)                
                 
select 9,'Out of Scope Purchases',                  
      isnull(sum((case when (invoicetype like 'Purchase%') and VatCategoryCode in ('O') and BuyerCountryCode like 'SA%'                  
        then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)                   
        else 0 end))                 
 ,0) as inneramount,null,3                  
 from VI_importstandardfiles_Processed sales                  
 where  Invoicetype like 'Purchase%'  and VatCategoryCode in ('O') and BuyerCountryCode like 'SA%'              
and effdate >= @fromdate and effdate <= @todate                 
and TenantId=@tenantId ;                  
end
GO
