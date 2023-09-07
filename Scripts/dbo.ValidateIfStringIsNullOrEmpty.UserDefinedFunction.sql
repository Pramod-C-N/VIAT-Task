USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ValidateIfStringIsNullOrEmpty]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 

create   function [dbo].[ValidateIfStringIsNullOrEmpty](@input nvarchar(max)) 
returns bit
as
begin
set @input = isnull(@input,'')
if(trim(@input)='')
begin
return 0
end
return 1
end
GO
