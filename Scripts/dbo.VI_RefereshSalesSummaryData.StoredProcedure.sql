USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_RefereshSalesSummaryData]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create       procedure [dbo].[VI_RefereshSalesSummaryData]    -- exec VI_RefereshSalesSummaryData ,50  
(  
@tenantId int=null
)  
as  
Begin  
--  select * from apportionmentbasedata
declare @fromdate as datetime,
@todate as datetime

declare @col01 float=0.00,
@col02 float=0.00,
@col03 float=0.00,
@col04 float=0.00,
@col05 float=0.00,
@col06 float=0.00,
@col07 float=0.00,
@col08 float=0.00,
@col09 float=0.00,
@col10 float=0.00,
@col11 float=0.00,
@col12 float=0.00,
@amount float=0.00

set @fromdate = (select top 1 EffectiveFromDate from  FinancialYear with (nolock) where isactive = 1 and tenantid = @tenantid) --in (select tenantid from ImportBatchData where BatchId=@batchno))                  
                  
set @todate = (select top 1 EffectiveTillEndDate from  FinancialYear with (nolock) where isactive = 1 and tenantid = @tenantid)  --in (select tenantid from ImportBatchData where BatchId=@batchno))                  

--select * from apportionmentbasedata 

--select * from VI_importstandardfiles_Processed 

select  isnull(sum((case when (invoicetype like 'Sales Invoice%')    
        then isnull(LineAmountInclusiveVAT,0)-isnull(AdvanceRcptAmtAdjusted,0)   
        else 0 end) -   
        (case when (invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')   
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)   
     then isnull(LineAmountInclusiveVAT,0)  
        else 0 end) +  
  (case when (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')  
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)   
  then isnull(LineAmountInclusiveVAT,0)  
  else 0 end)),0)  
 as amount,  
 effdate,
 'Taxable'  
 from VI_importstandardfiles_Processed   
 where  ((invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')) or   
 (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt'))  
 or (Invoicetype like 'Sales Invoice%'   
)) and vatcategorycode = 'S'  
and left(BuyerCountryCode,2) = 'SA' and VatRate=15    
and effdate >= @fromdate and effdate <= @todate and TenantId=@tenantId
group by effdate


--select CASE WHEN CAST(effdate AS DATETIME) BETWEEN DATEADD(mm, -1, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then sum(case when invoicetype like 'Credit Note%' then 
--	  isnull(taxablesupply,0) else 0 end as @col1
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -2, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col2
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -3, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col3
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -4, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col4
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -5, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col5
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -6, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col6
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -7, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col7
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -8, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col8
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -9, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col9
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -10, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col10
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -11, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col11
--,      CASE WHEN [date]<>'Total' and CAST('01-'+[date] AS DATETIME) BETWEEN DATEADD(mm, -12, @fromdate) AND DATEADD(mm, 0, @fromdate)
--      then isnull(taxablesupply,0) else 0 end as col12
--,     CASE WHEN [date]='Total'
--      then isnull(taxablesupply,0) else 0 end as Amount
--, 0 as style
-- from VI_importstandardfiles_Processed   
-- where  ((invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')) or   
-- (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt'))  
-- or (Invoicetype like 'Sales Invoice%'   
--)) and vatcategorycode = 'S'  
--and left(BuyerCountryCode,2) = 'SA' and VatRate=15    
--and effdate >= @fromdate and effdate <= @todate and TenantId=@tenantId ;  
end

GO
