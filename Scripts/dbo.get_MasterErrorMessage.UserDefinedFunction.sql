USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[get_MasterErrorMessage]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   FUNCTION [dbo].[get_MasterErrorMessage] (@uuid uniqueidentifier)
RETURNS varchar(max) AS
BEGIN
    DECLARE @combinedString VARCHAR(MAX)
SELECT @combinedString =COALESCE(@combinedString + ';', '') + cast(ErrorType as varchar)+ '-'+ ErrorMessage 
from
importMaster_ErrorLists
where uniqueIdentifier=@uuid and Status =0
RETURN @combinedString
END;
GO
