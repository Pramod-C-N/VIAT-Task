USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransValidations_TobeDeleted]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   procedure [dbo].[SalesTransValidations_TobeDeleted]  -- exec SalesTransValidations 657237
(
@BatchNo numeric
)
as
begin

-- Invalid Invoice Type
--insert into  

--update ImportStandardFiles set InvoiceType = 'Sales Invoice - Standard' where batchid = @batchno and Invoicetype = 'Sales Invoice - '

begin
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 1
end
begin
insert into importstandardfiles_ErrorLists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) 
select tenantid, @batchno,uniqueidentifier,'0','Invalid Invoice Type',1,0,GETDATE() from ImportBatchData 
where trim(substring(InvoiceType,16,len(InvoiceType)-15)) not in (select invoice_flags from mst_invoiceindicators) and batchid = @batchno  
end

end
GO
