USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertBatchUploadVendor]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE           PROCEDURE [dbo].[InsertBatchUploadVendor]             
( @json NVARCHAR(max),      
 @tenantId INT = NULL,      
 @fileName nvarchar(max),      
 @fromdate DateTime=null,        
 @todate datetime=null      
 )            
AS            
  BEGIN          
        
  Declare @MaxBatchId int       
      
Select         
  @MaxBatchId = isnull(max(batchId) , 0)  
from         
  BatchMasterData;        
Declare @batchno int = @MaxBatchId + 1;        
INSERT INTO [dbo].[BatchMasterData]       
(        
  [TenantId],        
  [BatchId],      
  [FileName],         
  [TotalRecords],      
  [Status],      
  [Type],       
  [fromDate],      
  [toDate],        
  [CreationTime],      
  [IsDeleted]        
)         
VALUES         
  (        
    @tenantId,         
    @batchno,         
    @fileName,         
    0,         
    'Unprocessed',         
    'VendorData',       
    @fromdate,        
    @todate,      
    GETDATE(),         
    0        
  )       
          
  --set @tenantId=(SELECT top 1 TenantId FROM ImportMasterBatchdata ORDER  BY id desc)        
  Declare @id int;            
  Select @id= SCOPE_IDENTITY()    
    
  Declare @totalRecords int =(select count(*) from OPENJSON(@json) );  
        
Insert into dbo.logs         
values         
  (        
    @json,      
    getdate(),         
    @batchno      
  )      
  INSERT INTO [dbo].[ImportMasterBatchdata]      
       (
	    [TenantId]            
       ,[UniqueIdentifier]
       ,[TenantType]            
       ,[ConstitutionType]        
       ,[Name]
	   ,[LegalName]
	   ,[ContactPerson]            
       ,[ContactNumber]
	   ,[EmailID]            
       ,[Nationality]
	   ,[DocumentLineIdentifier]
	   ,[DocumentType]        
       ,[DocumentNumber]
	   ,[RegistrationDate]
	   ,[ParentEntityName]        
       ,[LegalRepresentative]
	   ,[Designation]
	   ,[ParentEntityCountryCode]
	   ,[BusinessCategory]
	   ,[OperationalModel]
	   ,[BusinessPurchase]
	   ,[PurchaseVATCategory]
	   ,[InvoiceType]
	   ,[OrgType]
	   ,[AffiliationStatus]
	   ,[MasterType]      
       ,[MasterId]      
       ,[CreationTime]            
       ,[CreatorUserId]            
       ,[IsDeleted]      
       ,[batchid]    
	 )     
      
     Select  
	    @tenantId    
       ,NEWID()            
       ,[TenantType]            
       ,[ConstitutionType]
	   ,[Name]
	   ,[LegalName]
       ,[ContactPerson]
	   ,[ContactNumber]
	   ,[EmailID]
	   ,[Nationality]
	   ,CAST([DocumentLineIdentifier] AS INT)
	   ,[DocumentType]
       ,[DocumentNumber]
	   ,dbo.ISNULLOREMPTYFORDATE([RegistrationDate]) as RegistrationDate
	   ,[ParentEntityName]
	   ,[LegalRepresentative]
	   ,[Designation]
	   ,[ParentEntityCountryCode]
	   ,[BusinessCategory]          
       ,[OperationalModel]
	   ,case when [BusinessPurchase] is null or [BusinessPurchase] = '' then 'Domestic'    
        else [BusinessPurchase] end
	   ,[PurchaseVATCategory]
	   ,[InvoiceType]
	   ,[OrgType]
	   ,[AffiliationStatus]
	   ,'Vendor' as MasterType      
       ,cast([MasterId] as nvarchar) as MasterId      
       ,getdate()            
       ,1            
       ,0      
       ,@batchno     
    from            
    OPENJSON(@json)             
        with (            
		[MasterId] nvarchar(max) '$."VendorID"',      
        [TenantType] nvarchar(max) '$."VendorType"',             
        [ConstitutionType] nvarchar(max) '$."VendorConstitution"',  
		[Name] nvarchar(max) '$."VendorName"',
		[LegalName] nvarchar(max) '$."Legal/CommercialName"',
		[ContactPerson] nvarchar(max) '$."ContactPerson"',
		[ContactNumber] nvarchar(max) '$."ContactNumber"',
		[EmailID] nvarchar(max) '$."EmailID"',
		[Nationality] nvarchar(max) '$."VendorCountryCode"',
		[DocumentLineIdentifier] nvarchar(max) '$."DocumentLineIdentifier"',
		[DocumentType] nvarchar(max) '$."DocumentType"',
		[DocumentNumber] nvarchar(max) '$."DocumentNumber"',
		[RegistrationDate] nvarchar(max) '$."RegistrationDate"',
		[ParentEntityName] nvarchar(max) '$."ForeignEntityName"',
		[LegalRepresentative] nvarchar(max) '$."NameofLegalRep"',
		[Designation] nvarchar(max) '$."Designation"' ,
		[ParentEntityCountryCode] nvarchar(max) '$."ParentEntityCountryCode"' ,
		[BusinessCategory] nvarchar(max) '$."BusinessCategory"',          
        [OperationalModel] nvarchar(max) '$."OperatingModel"',
		[BusinessPurchase] nvarchar(max) '$."BusinessPurchases"',
		[PurchaseVATCategory] nvarchar(max) '$."PurchaseVATCategory"',
		[InvoiceType] nvarchar(max) '$."InvoiceType"',
		[OrgType] nvarchar(max) '$."VendorSubType"',
		[AffiliationStatus] nvarchar(max) '$."AffiliationStatus"'
            
  )      
update    BatchMasterData  set    TotalRecords = @totalRecords,    SuccessRecords = @totalRecords,    FailedRecords = 0,    status = 'Processed',   batchid= @batchno where    FileName = @fileName    and Status = 'Unprocessed';    
       
   end       
    
   begin      
   exec VendorTransValidation @batchno,@tenantid      
   end
GO
