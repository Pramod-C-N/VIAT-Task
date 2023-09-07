USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CheckIfvatExists]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create      procedure [dbo].[CheckIfvatExists]  
(  
@vat nvarchar(200) ,  
@checkInAllTenants char(1) = 1  
)  
as  
begin  
select count(*) as count from TenantBasicDetails with(nolock) where VATID=@vat  
end
GO
