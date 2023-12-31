USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetMasterBatchData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        procedure [dbo].[GetMasterBatchData]  
(  
@fromdate datetime,  
@todate datetime,
@tenantId int=null
)  
AS  
SET NOCOUNT ON
BEGIN  
select BatchId,FileName,TotalRecords,SuccessRecords,FailedRecords,Status,CreationTime as CreatedDate from BatchMasterData with (nolock)  
where CAST(CreationTime AS DATE)  BETWEEN cast(@fromdate as date) AND cast(@todate as date) 
and ISNULL(TenantId,0)=ISNULL(@tenantId,0) order by CreationTime desc  
END
GO
