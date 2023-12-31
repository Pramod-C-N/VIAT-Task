USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetTenantBatchData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create                PROCEDURE [dbo].[GetTenantBatchData]  
(  
@fileName nvarchar(max)='' ,
@tenantId int = null
)  
AS  
BEGIN  
  
select BatchId,FileName,TotalRecords,SuccessRecords,FailedRecords,Status from BatchMasterData  where FileName = @fileName and ISNULL(TenantId,0)=ISNULL(@tenantId,0) order by CreationTime desc  
   
END
GO
