USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ReplaceHtmlString]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create function [dbo].[ReplaceHtmlString](@html nvarchar(max),@find nvarchar(max),@value nvarchar(max)) 
returns  nvarchar(max)
as
begin
declare @replaced nvarchar(max)= ( SELECT
    REPLACE(@html, @find, ISNULL(@value, '')));
RETURN @replaced
END
GO
