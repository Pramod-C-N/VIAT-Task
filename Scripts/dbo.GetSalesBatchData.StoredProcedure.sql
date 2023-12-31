USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetSalesBatchData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create              PROCEDURE [dbo].[GetSalesBatchData]  
(  
@fileName nvarchar(max)='' ,
@tenantId int = null
)  
AS  
BEGIN  
  
select BatchId,FileName,TotalRecords,SuccessRecords,FailedRecords,Status from BatchData  where FileName = @fileName and ISNULL(TenantId,0)=ISNULL(@tenantId,0) order by CreationTime desc  
   
END
GO
