USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetFileMappingByType]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
CREATE     PROCEDURE [dbo].[GetFileMappingByType]  
    @tenantId INT,  
 @type nvarchar(255)  
AS  
BEGIN  
    SELECT mapping 
    FROM FileMappings  
    WHERE @tenantId = @TenantId and transactiontype=@type;  
END;  
GO
