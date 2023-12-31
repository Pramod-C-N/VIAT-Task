USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[get_OverrideTransactionMessage]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create     FUNCTION [dbo].[get_OverrideTransactionMessage] (@uuid uniqueidentifier)
RETURNS varchar(max) AS
BEGIN
    DECLARE @combinedString VARCHAR(MAX)
SELECT @combinedString =COALESCE(@combinedString + ';', '') + errormsg 
from
Transactionoverride
where uniqueIdentifier=@uuid 
RETURN @combinedString
END;
GO
