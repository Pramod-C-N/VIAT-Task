USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransAdvanceReceiptValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    procedure [dbo].[SalesTransAdvanceReceiptValidations]  -- exec SalesTransAdvanceReceiptValidations 126755  
(  
@BatchNo numeric,  
@validstat int  
)  
as  
begin  
  
  
begin  
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 23  
end  
begin  
  
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)   
select tenantid,@batchno,uniqueidentifier,'0','Advance Amount should <= Invoice Amount',23,0,getdate() from ImportBatchData   
where AdvanceRcptAmtAdjusted > 0 and AdvanceRcptAmtAdjusted > linenetamount and batchid = @batchno    
  
end  
  
end
GO
