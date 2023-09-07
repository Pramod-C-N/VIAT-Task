USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[SalesTransADvanceVATAmtValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[SalesTransADvanceVATAmtValidations]  -- exec SalesTransADvanceVATAmtValidations 126755
(
@BatchNo numeric,
@validstat int 
)
as
begin


begin
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype in (24,510)		
end
begin

insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) 
select tenantid,@batchno,uniqueidentifier,'0','Advance VAT Amount should < Invoice VAT Amount',24,0,getdate() from ImportBatchData 
where isnull(VatOnAdvanceRcptAmtAdjusted,0)  > 0 and isnull(VatOnAdvanceRcptAmtAdjusted,0)  > isnull(VATLineAmount,0)  and batchid = @batchno  

insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime) 
select tenantid,@batchno,uniqueidentifier,'0','Advance VAT Amount should not be blank',510,0,getdate() from ImportBatchData 
where   (VatOnAdvanceRcptAmtAdjusted is null or len(VatOnAdvanceRcptAmtAdjusted)=0) and AdvanceRcptAmtAdjusted is not null  and batchid = @batchno 


end

end
GO
