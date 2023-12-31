USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertBatchUploadCustomer]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE             PROCEDURE [dbo].[InsertBatchUploadCustomer]                 
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
    'CustomerData',           
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
           ([TenantId]                
           ,[UniqueIdentifier]                
           ,[TenantType]                
           ,[ConstitutionType]            
     , [BusinessCategory]            
     , [OperationalModel]            
           ,[ContactPerson]                
           ,[ContactNumber]                
           ,[EmailID]                
           ,[Nationality]                
           ,[Designation]            
      ,[ParentEntityName]            
   ,[name]    
   ,[LegalName]     
     ,[LegalRepresentative]            
    , [ParentEntityCountryCode]            
        ,[DocumentType]            
           ,[DocumentNumber]          
     ,[DocumentLineIdentifier]          
     ,[RegistrationDate]     
,[SalesVATCategory]         
        ,[BusinessSupplies]    
,[InvoiceType]          
     ,[MasterType]          
     ,[MasterId]          
      , [CreationTime]                
           ,[CreatorUserId]                
           ,[IsDeleted],          
     [batchid],
	 [vatid])         
          
     Select      
  @tenantId,    
     NEWID(),                
    --[TenantType]
	case when [TenantType] is null or [TenantType] = '' then 'Individual'        
    else [TenantType] end
           ,[ConstitutionType]              
     , [BusinessCategory]            
     , [OperationalModel]            
           ,[ContactPerson]                
           ,[ContactNumber]                
           ,[EmailID]                
           ,[Nationality]                
           ,[Designation]             
     ,[ParentEntityName]       
  ,[CustomerName]    
  ,[LegalName]    
     ,[LegalRepresentative]            
    , [ParentEntityCountryCode]            
         ,[DocumentType]            
            ,[DocumentNumber]          
      ,CAST([DocumentLineIdentifier] AS INT)          
      ,dbo.ISNULLOREMPTYFORDATE([RegistrationDate]) as RegistrationDate          
 ,SalesVATCategory as SalesVATCategory    
 ,case when [BusinessSupplies] is null or [BusinessSupplies] = '' then 'Domestic'        
 else [BusinessSupplies] end        
 ,case when [InvoiceType] is null or [InvoiceType] = '' then 'Standard'        
 else [InvoiceType] end    
 ,'Customer' as MasterType          
      , cast([MasterId] as nvarchar) as MasterId          
     ,getdate()                
           ,1                
           ,0          
     ,@batchno 
	 ,case when [BusinessSupplies] = 'Domestic' then [DocumentNumber]        
     else '' end 
     from                
     OPENJSON(@json)                 
        with (                
    [MasterId] nvarchar(max) '$."Customer ID"',          
        [TenantType] nvarchar(max) '$."Customer Type"',                 
        [ConstitutionType] nvarchar(max) '$."Constitution Type"',    
  [CustomerName] nvarchar(max) '$."Customer Name"',    
  [LegalName] nvarchar(max) '$."Legal/Commercial Name"',    
        [BusinessCategory] nvarchar(max) '$."Business Category"',              
        [OperationalModel] nvarchar(max) '$."Operational Model"',                 
         [ContactPerson] nvarchar(max) '$."Contact Person"',                 
        [ContactNumber] nvarchar(max) '$."Contact Number"',                 
   [EmailID] nvarchar(max) '$."Email ID"',                 
        [Nationality] nvarchar(max) '$."Country Code"',                 
        [Designation] nvarchar(max) '$."Designation"' ,            
   [ParentEntityName] nvarchar(max) '$."Foreign Entity Name"',            
  [LegalRepresentative] nvarchar(max) '$."Name of Legal Rep"',            
   [ParentEntityCountryCode] nvarchar(max) '$."Parent Entity Country Code"' ,            
    [DocumentType] nvarchar(max) '$."Document Type"',          
    [DocumentNumber] nvarchar(max) '$."Registration Number"',          
 [DocumentLineIdentifier] nvarchar(max) '$."Document Line Identifier"',          
 [RegistrationDate] nvarchar(max) '$."Registration Date"',          
 [SalesVATCategory] nvarchar(max) '$."Sales VAT Category"',          
 [BusinessSupplies] nvarchar(max) '$."Business Supplies"',          
 [InvoiceType] nvarchar(max) '$."Invoice Type"'                
  )          
update    BatchMasterData  set    TotalRecords = @totalRecords,    SuccessRecords = @totalRecords,    FailedRecords = 0,    status = 'Processed',   batchid= @batchno where    FileName = @fileName    and Status = 'Unprocessed';        
           
   end           
        
   begin          
   exec CustomerMasterValidation @batchno,@tenantid          
   end
GO
