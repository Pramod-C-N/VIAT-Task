USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetFileMappings]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE    PROCEDURE [dbo].[GetFileMappings]  
    @tenantId INT  
AS  
BEGIN  
    SELECT *  
    FROM FileMappings  
    WHERE tenantId = @tenantId;  
END;  
GO
