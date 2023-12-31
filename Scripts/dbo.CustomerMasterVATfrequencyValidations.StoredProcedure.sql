USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CustomerMasterVATfrequencyValidations]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CustomerMasterVATfrequencyValidations]  -- exec CustomerMasterVATfrequencyValidations 131          
(          
      
@BatchNo numeric      
)          
as          
begin
DELETE FROM importmaster_ErrorLists
WHERE batchid = @BatchNo
  AND errortype IN (333)
END

BEGIN

INSERT INTO importmaster_ErrorLists (tenantid, BatchId, uniqueidentifier, Status, ErrorMessage, Errortype, isdeleted, CreationTime)
  SELECT
    tenantid
   ,@batchno
   ,uniqueidentifier
   ,'0'
   ,'For Foreign Branch Nationality cannot be SA'
   ,333
   ,0
   ,GETDATE()
  FROM ImportMasterBatchData
  WHERE UPPER(TRIM(ConstitutionType)) LIKE '%FOREIGN BRANCH%'
  AND UPPER(Nationality) LIKE '%SA%'
  AND batchid = @BatchNo


END
GO
