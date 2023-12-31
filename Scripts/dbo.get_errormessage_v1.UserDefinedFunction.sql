USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[get_errormessage_v1]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[get_errormessage_v1] (@uuid uniqueidentifier)  
RETURNS varchar(max) AS  
BEGIN  
    DECLARE @combinedString VARCHAR(MAX)  
SELECT @combinedString =COALESCE(@combinedString + ';', '') + cast(ErrorType as varchar)+ '-'+ ErrorMessage   
from  
importstandardfiles_ErrorLists  with(nolock)
where uniqueIdentifier=@uuid and Status =0  
RETURN @combinedString  
END;
GO
