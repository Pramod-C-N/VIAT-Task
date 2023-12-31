USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetLatestBatchId]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     procedure [dbo].[GetLatestBatchId](
@tenantId int =127
)    
as     
begin    
select isnull(max(batchId),0) as BatchId from BatchData where TenantId=@tenantId     
end
GO
