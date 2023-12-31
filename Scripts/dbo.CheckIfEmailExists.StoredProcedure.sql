USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CheckIfEmailExists]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[CheckIfEmailExists]
(
@email nvarchar(200) ,
@checkInAllTenants char(1) = 1
)
as
begin
select count(*) as count from ABpUsers with(nolock) where EmailAddress=@email
end
GO
