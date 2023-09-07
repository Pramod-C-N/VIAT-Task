USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetMasterTypeFromBatchId]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetMasterTypeFromBatchId]( @batchId int)
as
begin
select top 1 ISNULL(Type,'') as masterType from BatchMasterData where BatchId=@batchId
end
GO
