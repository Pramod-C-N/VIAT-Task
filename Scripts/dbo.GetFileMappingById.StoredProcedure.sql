USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetFileMappingById]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
    
CREATE       PROCEDURE [dbo].[GetFileMappingById]    
    @tenantId INT,    
  @id INT  = null   
AS    
BEGIN    
DECLARE @mapping NVARCHAR(MAX);  
    SELECT @mapping= mapping   
    FROM FileMappings    
    WHERE @tenantId = @TenantId and id=@id;    
IF(@mapping = '' OR @mapping IS NULL)  
BEGIN  
 SELECT @mapping= mapping   
    FROM FileMappings    
    WHERE @tenantId IS null and id=@id;    
END  
SELECT @mapping  
END;
GO
