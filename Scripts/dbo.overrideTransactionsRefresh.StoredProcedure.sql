USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[overrideTransactionsRefresh]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE             PROCEDURE [dbo].[overrideTransactionsRefresh]      -- exec overrideTransactionsRefresh null,311  
  
@uuid uniqueidentifier,  
@batchno int  
  
AS         
--delete from Transactionoverride   
declare @Transtype nvarchar(100),  
@tenantid int  
  
        
begin  
  
if @uuid is not null   
   begin  
  
     --insert into logs(batchid,date,json) values(@batchno,getdate(),'uuid')  
     delete from importstandardfiles_Errorlists where uniqueIdentifier= @uuid and errortype in (select code from ErrorType where ErrorGroupId = 3)  
    
     set @Transtype = (select invoicetype from ImportBatchData where UniqueIdentifier = @uuid)  
  
  end  
  
if @batchno is not null and @uuid is null  
   begin  
    
     delete from importstandardfiles_Errorlists where Batchid = @batchno and errortype in (select code from ErrorType where ErrorGroupId = 3)  
  
  set @Transtype = (select top 1 invoicetype from ImportBatchData where batchid = @batchno)  
  end  
   
end  

  
if left(@Transtype,5) = 'Sales'  
begin  
  --insert into logs(batchid,date,json) values(@batchno,getdate(),'Override(Sales)')     
  exec SalesTransValidation @batchno  
end  
else if left(@transtype,6) = 'Credit'  
begin  
  --insert into logs(batchid,date,json) values(@batchno,getdate(),'Override(Credit)')     
  
  exec CreditNoteTransValidation @batchno  
end  
else if left(@transtype,8) = 'Purchase'  
begin  
  --insert into logs(batchid,date,json) values(@batchno,getdate(),'Override(Purchase)')  
  exec PurchaseTransValidation  @batchno  
end  
else if left(@transtype,11) = 'CN Purchase'  
begin  
  exec CreditNotePurchaseTransValidation  @batchno  
end  
else if left(@transtype,11) = 'DN Purchase'  
begin  
  exec DebitNotePurchaseTransValidation  @batchno  
end  
else if left(@transtype,5) = 'Debit'  
begin  
  exec DebitNoteTransValidation   @batchno  
end  
else if left(@transtype,3) = 'WHT'  
begin  
  exec PaymentTransValidation   @batchno  
end
GO
