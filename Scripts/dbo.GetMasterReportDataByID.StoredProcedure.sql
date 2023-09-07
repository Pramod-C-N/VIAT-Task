USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetMasterReportDataByID]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          PROCEDURE [dbo].[GetMasterReportDataByID]        -- exec GetMasterReportDataByID 730,49     
@batchid int,    
@tenantId int=null,  
@para int=null  
AS   
begin  
if @para is null  
begin  
select Number,Date,Name,errormessage,  
Status,  
uniqueId,  
isOverride           
from (  
select isf.MasterId as Number,format(CreationTime,'dd/MM/yyyy')  as 'date',Name as name, dbo.get_MasterErrorMessage(ise.uniqueIdentifier) as errormessage,  
ise.Status,isf.UniqueIdentifier  as uniqueId,  
dbo.get_MasterOverrideStatus(ise.uniqueIdentifier) as isOverride           
from  (select Distinct uniqueIdentifier,Status ,Batchid,TenantId from ImportMaster_ErrorLists) ise          
inner join ImportMasterBatchData isf on isf.uniqueidentifier = ise.uniqueidentifier and ISNULL(isf.TenantId,0)=ISNULL(ise.TenantId,0)          
where ise.batchid=@batchid  and ise.Status=0        
union All         
select i.MasterId as Number,format(i.CreationTime,'dd/MM/yyyy') as 'Date',Name, case when m.uniqueidentifier is null then ' ' else 'Master Override' end  
 as errormessage,  
 1 as Status      ,i.UniqueIdentifier  as uniqueId,  
 0 as isOverride       
from   ImportMasterBatchData i left outer join (select distinct uniqueidentifier from Masteroverride) m on i.UniqueIdentifier = m.uniqueidentifier           
where i.batchid=@batchid  and ISNULL(i.TenantId,0)=ISNULL(@tenantId,0) and i.UniqueIdentifier not in   
(select Distinct uniqueIdentifier from importMaster_ErrorLists where Batchid=@batchid and Status=0 and  ISNULL(TenantId,0)=ISNULL(@tenantId,0))  
) a order by cast(a.number as int)  
end  
  
else  
begin  
select Number,Date,Name,errormessage,  
Status  
--uniqueId,  
--isOverride           
from (  
select isf.MasterId as Number,format(CreationTime,'dd/MM/yyyy')  as 'date',Name as name, dbo.get_MasterErrorMessage(ise.uniqueIdentifier) as errormessage,  
ise.Status,isf.UniqueIdentifier  as uniqueId,  
dbo.get_MasterOverrideStatus(ise.uniqueIdentifier) as isOverride           
from  (select Distinct uniqueIdentifier,Status ,Batchid,TenantId from ImportMaster_ErrorLists) ise          
inner join ImportMasterBatchData isf on isf.uniqueidentifier = ise.uniqueidentifier and ISNULL(isf.TenantId,0)=ISNULL(ise.TenantId,0)          
where ise.batchid=@batchid  and ise.Status=0        
union All         
select i.MasterId as Number,format(i.CreationTime,'dd/MM/yyyy') as 'Date',Name, case when m.uniqueidentifier is null then ' ' else 'Master Override' end  
 as errormessage,  
 1 as Status      ,i.UniqueIdentifier  as uniqueId,  
 0 as isOverride       
from   ImportMasterBatchData i left outer join (select distinct uniqueidentifier from Masteroverride) m on i.UniqueIdentifier = m.uniqueidentifier           
where i.batchid=@batchid  and ISNULL(i.TenantId,0)=ISNULL(@tenantId,0) and i.UniqueIdentifier not in   
(select Distinct uniqueIdentifier from importMaster_ErrorLists where Batchid=@batchid and Status=0 and  ISNULL(TenantId,0)=ISNULL(@tenantId,0))  
) a order by cast(a.number as int)  
end  
end
GO
