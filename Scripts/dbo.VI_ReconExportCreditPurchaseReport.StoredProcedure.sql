USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_ReconExportCreditPurchaseReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create    procedure [dbo].[VI_ReconExportCreditPurchaseReport]    -- exec VI_ReconExportCreditPurchaseReport '2022-09-01', '2022-09-30'                        
(                        
@fromdate date,                        
@todate date,                      
@tenantId int=null                      
)                        
as                        
Begin                        
-- (7,'Export Purchase Credit Notes',8,8,5)                      
                       
select 7,'Import Purchase Credit Notes',                       
      isnull(sum((case when (invoicetype like 'CN%') and invoicetype like '%Import%'                         
        then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)                         
        else 0 end))                       
 ,0) as inneramount,null,5                       
 from VI_importstandardfiles_Processed sales                        
 where  Invoicetype like 'CN%'   and invoicetype like '%Import%'                      
and effdate >= @fromdate and effdate <= @todate                       
and TenantId=@tenantId ;              
end
GO
