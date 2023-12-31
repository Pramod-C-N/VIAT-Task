USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetReportDataByID]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE         PROCEDURE [dbo].[GetReportDataByID]        -- exec GetReportDataByID 1060,24  ,1   
              
@batchid int,        
@tenantId int=null,    
@para int=null    
    
AS     
begin    
  
declare @importstandardfiles_ErrorList as table   
(  
TenantId int,  
Batchid int,  
uniqueIdentifier uniqueidentifier,  
Status bit  
)  
  
insert into @importstandardfiles_ErrorList (uniqueIdentifier,Status,Batchid,TenantId)  
select Distinct uniqueIdentifier,Status ,Batchid,TenantId from importstandardfiles_ErrorLists where batchid = @batchid   
and TenantId=@tenantId  
  
if @para is null    
begin   
select a.Number as Number, format(a.date,'dd-MM-yyyy') as 'Date', a.Name as Name, a.errormessage as errormessage, a.status as Status, uniqueId ,      
isoverride as isOverride from (      
select isf.InvoiceNumber as Number,IssueDate as 'Date',BuyerName as Name,   
dbo.get_errormessage_v2(ise.uniqueIdentifier,@tenantId) as errormessage,  
--dbo.get_errormessage(ise.uniqueIdentifier) as errormessage,  
ise.Status,      
isf.UniqueIdentifier  as uniqueId, dbo.get_TransactionOverrideStatus_v1(ise.uniqueIdentifier,@tenantId) as isOverride               
from  (select * from @importstandardfiles_ErrorList) ise              
inner join ImportBatchData isf on isf.uniqueidentifier = ise.uniqueidentifier                
where ise.batchid=@batchid  and ise.Status=0 and isf.TenantId=ise.TenantId          
union All             
select i.InvoiceNumber,i.IssueDate,i.BuyerName,  case when m.uniqueidentifier is null then ' ' else 'Transaction Override' end as errormessage,       
1 as Status      ,i.UniqueIdentifier  as uniqueId,0 as isOverride from   ImportBatchData i       
left outer join (select distinct uniqueidentifier from Transactionoverride where batchid = @batchid) m on i.UniqueIdentifier = m.uniqueidentifier      
where i.batchid=@batchid  and ISNULL(i.TenantId,0)=ISNULL(@tenantId,0) and       
i.UniqueIdentifier not in (select uniqueIdentifier from @importstandardfiles_ErrorList where Status=0 ) ) a order by a.Number  asc  
end    
    
else    
begin    
select a.Number as Number, a.date as 'Date', a.Name as Name, a.errormessage as errormessage, a.status as Status    
--, uniqueId ,      
--isoverride as isOverride     
from (      
select isf.InvoiceNumber as Number,IssueDate as 'Date',BuyerName as Name, dbo.get_errormessage_v2(ise.uniqueIdentifier,@tenantId) as errormessage,ise.Status,      
isf.UniqueIdentifier  as uniqueId, dbo.get_TransactionOverrideStatus_v1(ise.uniqueIdentifier,@TenantId) as isOverride               
from  (select * from @importstandardfiles_ErrorList) ise              
inner join ImportBatchData isf on isf.uniqueidentifier = ise.uniqueidentifier                
where ise.batchid=@batchid  and ise.Status=0 and isf.TenantId=ise.TenantId          
union All             
select i.InvoiceNumber,i.IssueDate,i.BuyerName,  case when m.uniqueidentifier is null then ' ' else 'Transaction Override' end as errormessage,       
1 as Status      ,i.UniqueIdentifier  as uniqueId,0 as isOverride from   ImportBatchData i       
left outer join (select distinct uniqueidentifier from Transactionoverride where batchid = @batchid) m on i.UniqueIdentifier = m.uniqueidentifier      
where i.batchid=@batchid  and ISNULL(i.TenantId,0)=ISNULL(@tenantId,0) and       
i.UniqueIdentifier not in (select uniqueIdentifier from @importstandardfiles_ErrorList where   Status=0) ) a order by a.Number asc    
    
end    
    
end
GO
