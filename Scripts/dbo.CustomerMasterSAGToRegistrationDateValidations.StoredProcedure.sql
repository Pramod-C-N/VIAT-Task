USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CustomerMasterSAGToRegistrationDateValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE               procedure [dbo].[CustomerMasterSAGToRegistrationDateValidations]  -- exec CustomerMasterSAGToRegistrationDateValidations 131              
(              
          
@BatchNo numeric          
)              
as              
begin              
delete from importmaster_ErrorLists  where batchid = @BatchNo  and errortype in (352)        
end              
              
begin              
              
insert into importmaster_ErrorLists (tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)               
select tenantid,@batchno,uniqueidentifier,'0','SAG registration date cannot be greater than current date',352,0,getdate() from ImportMasterBatchData               
where  upper(trim(DocumentType)) like '%SAG%' and format(try_cast(RegistrationDate as date),'yyyy-MM-dd') > (format(try_cast(GETDATE() as date),'yyyy-MM-dd'))            
  and batchid = @BatchNo           
           
         
end
GO
