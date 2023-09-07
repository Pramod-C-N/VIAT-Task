USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_ReconExportSalesReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[VI_ReconExportSalesReport]    -- exec VI_ReconExportSalesReport '2022-09-01', '2022-09-30'            
(            
@fromdate date,            
@todate date,          
@tenantId int=null          
)            
as            
Begin            
-- (10,'Export Supplies',8,8,5)          
           
select 10,'Export Supplies',    
      isnull(sum((case when (invoicetype like 'Sales Invoice%') and invoicetype like '%Export%'             
        then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)             
        else 0 end))           
 ,0) as inneramount,null,5            
 from VI_importstandardfiles_Processed sales            
 where  Invoicetype like 'Sales Invoice%'   and invoicetype like '%Export%'          
and effdate >= @fromdate and effdate <= @todate           
and TenantId=@tenantId ;            
end
GO
