USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ReplaceHtmlDecimal]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReplaceHtmlDecimal](@html nvarchar(max),@find nvarchar(max),@value decimal(15,2)) 
returns  nvarchar(max)
as
begin
declare @replaced nvarchar(max)= ( SELECT
    REPLACE(@html, @find, FORMAT(ISNULL(@value, 0), '#,0.00')));
RETURN @replaced
END
GO
