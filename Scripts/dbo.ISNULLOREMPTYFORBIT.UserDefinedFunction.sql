USE [brady]
GO
/****** Object:  UserDefinedFunction [dbo].[ISNULLOREMPTYFORBIT]    Script Date: 01-09-2023 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ISNULLOREMPTYFORBIT]  
(     
    @value NVARCHAR(max)  
)  
RETURNS NVARCHAR(MAX)  
AS  
BEGIN   
  
IF (@value IS NULL)  
BEGIN  
    RETURN 0  
END  
ELSE  
BEGIN  
    IF (LEN(LTRIM(@value)) = 0)  
    BEGIN   
        RETURN 0  
    END   
END  
  
RETURN 1;  
END
GO
