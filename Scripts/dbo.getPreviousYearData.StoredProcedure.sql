USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getPreviousYearData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[getPreviousYearData]   --exec getPreviousYearData 127
(@tenantId int = null)
as
begin
Select FinYear,ApportionmentSupplies from ApportionmentBaseData where [Type]='Previous' and [Date] = 'Total' and TenantId=@tenantId
end
GO
