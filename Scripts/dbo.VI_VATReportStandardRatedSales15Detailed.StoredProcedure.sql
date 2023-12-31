USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_VATReportStandardRatedSales15Detailed]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE      procedure [dbo].[VI_VATReportStandardRatedSales15Detailed]    -- exec VI_VATReportStandardRatedSales15Detailed '2022-09-01', '2022-09-30'    
(

@fromdate date,    
@todate date,  
@tenantId int=null  
)    
as    
Begin    
 Select ROW_NUMBER() OVER(ORDER BY InvoiceNumber ASC) AS SlNo,
 Format(Issuedate,'dd-MM-yyyy'),
 InvoiceNumber,
     
      isnull((case when (invoicetype like 'Sales Invoice%')      
        then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)     
        else 0 end) -     
        (case when (invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')     
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)     
     then isnull(LineNetAmount,0)    
        else 0 end) +    
  (case when (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')    
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)     
  then isnull(LineNetAmount,0)    
  else 0 end),0)    
 as NetAmount,    
    
      isnull((case when (invoicetype like 'Sales Invoice%') then     
               isnull(vatlineamount,0)-isnull(vatonadvancercptamtadjusted,0)     
                  else 0 end) -     
                 (case when invoicetype like 'Credit Note%' and     
                  (Transtype ='Sales' or Transtype = 'AdvanceReceipt') then (isnull(vatlineamount,0))    
                 else 0 end) +    
                (case when invoicetype like 'Debit Note%' and     
                (Transtype ='Sales' or Transtype = 'AdvanceReceipt') then (isnull(vatlineamount,0))    
                else 0 end)   
   ,0) as vatamount  ,
  isnull((case when (invoicetype like 'Sales Invoice%')      
        then isnull(LineAmountInclusiveVAT,0)-isnull(AdvanceRcptAmtAdjusted,0)     
        else 0 end) -     
        (case when (invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')     
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)     
     then isnull(LineAmountInclusiveVAT,0)    
        else 0 end) +    
  (case when (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')    
     and OrignalSupplyDate  >= @fromdate and orignalSupplydate <= @todate)     
  then isnull(LineAmountInclusiveVAT,0)    
  else 0 end),0)    
 as TotalAmount
   

 from VI_importstandardfiles_Processed sales    
 where  ((invoicetype like 'Credit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt')) or     
 (invoicetype like 'Debit Note%' and (Transtype ='Sales' or Transtype = 'AdvanceReceipt'))    
 or (Invoicetype like 'Sales Invoice%'     
)) and vatcategorycode = 'S'    
and OrgType <> 'GOVERNMENT' and left(BuyerCountryCode,2) = 'SA' and VatRate=15      
and effdate >= @fromdate and effdate <= @todate and TenantId=@tenantId ;    
end
GO
