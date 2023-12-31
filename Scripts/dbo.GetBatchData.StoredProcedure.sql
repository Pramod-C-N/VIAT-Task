USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetBatchData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       procedure [dbo].[GetBatchData]    
(    
@fromdate datetime,    
@todate datetime,  
@tenantId int=null  
)    

AS    
SET NOCOUNT ON  
  
BEGIN    
select BatchId,FileName,TotalRecords,SuccessRecords,FailedRecords,Status,format(CreationTime,'dd-MM-yyyy') as CreatedDate from BatchData with (nolock)     
where CAST(CreationTime AS DATE)  BETWEEN CAST(@fromdate AS date) AND CAST(@todate AS date)   
and ISNULL(TenantId,0)=ISNULL(@tenantId,0) order by CreationTime desc    
END
GO
