USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertStringToDateTime]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 

create   function [dbo].[ConvertStringToDateTime](@input nvarchar(max)) 
returns datetime
as
begin
return Parse(@input as datetime)
end
GO
