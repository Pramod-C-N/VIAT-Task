USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertStringToDecimal]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   function [dbo].[ConvertStringToDecimal](@input nvarchar(max)) 
returns DECIMAL(18, 2)
as
begin
return PARSE(@input AS DECIMAL(18, 2) USING 'en-US')
end
GO
