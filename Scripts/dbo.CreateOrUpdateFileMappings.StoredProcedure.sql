USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CreateOrUpdateFileMappings]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE      PROCEDURE [dbo].[CreateOrUpdateFileMappings]  
    @tenantId INT,  
    @json NVARCHAR(MAX),  
 @type nvarchar(255),  
    @id INT = NULL  ,
	@name nvarchar(255),
	@isActive BIT
AS  
BEGIN  
    -- Check if @id is provided  
    IF @id = -1  
    BEGIN  
        -- Insert new record when @id is not provided  
        INSERT INTO FileMappings (TenantId, Mapping,TransactionType,Name, isActive)  
        VALUES (@tenantId, @json,@type,@name, @isActive); -- Assuming isActive is set to true (1) for new mappings  
    END  
    ELSE  
    BEGIN  
        -- Update existing record when @id is provided  
        UPDATE FileMappings  
        SET TenantId = @tenantId,  
            Mapping = @json,  
   TransactionType=@type  ,
   [Name]=@name,
   isActive = @isActive
        WHERE Id = @id;  
    END  
END;  

GO
