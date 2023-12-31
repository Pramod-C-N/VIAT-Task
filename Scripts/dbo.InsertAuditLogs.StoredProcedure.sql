USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertAuditLogs]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[InsertAuditLogs](  
@json nvarchar(max)=null,  
@invoiceNumber nvarchar(500),  
@servicename nvarchar(500),  
@message nvarchar(500)=null,  
@tenantId int  
)  
as  
begin  
INSERT INTO [dbo].[AbpAuditLogs]  
           (  
   [ClientName]  
           ,[CustomData]  
           ,[ServiceName]  
           ,[TenantId]  
           ,[ReturnValue],  
     [ExecutionDuration],  
     [ExecutionTime])  
     VALUES  
           (  
     @invoicenumber  
           ,@message  
           ,@serviceName  
           ,@tenantId  
           ,@json,  
     1,  
     GETDATE())  
  
end
GO
