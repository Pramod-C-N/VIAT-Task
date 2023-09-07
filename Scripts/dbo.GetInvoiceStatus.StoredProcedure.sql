USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetInvoiceStatus]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

      
          
create         procedure [dbo].[GetInvoiceStatus]  -- UpdateInvoiceStatus 'I',4, 770,'207504',''          
(          
@TenantId int=null,             
@irnno nvarchar(100)=null    
)          
as          
begin          
select inputData from InvoiceStatus where TenantId=@TenantId and irnno = @irnno
end
GO
