USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_VATReportImportSubjecttoRCM5Detailed]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     procedure [dbo].[VI_VATReportImportSubjecttoRCM5Detailed]     
(    
@fromdate date,    
@todate date,  
@tenantId int=null  
)    
as    
Begin    
    
 Select ROW_NUMBER() OVER(ORDER BY InvoiceNumber ASC) AS SlNo,
 Format(Issuedate,'dd-MM-yyyy'),
 InvoiceNumber
 ,isnull((case when (invoicetype like 'Purchase Entry%')     
      then isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0)+isnull(customSPAID,0)+isnull(excisetaxpaid,0)+isnull(otherchargespaid,0)     
 else 0 end) -     
      (case when (invoicetype like 'CN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment'))     
   and OrignalSupplyDate  >= @fromdate and OrignalSupplyDate <= @todate     
   then 0-(isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0))     
 else 0 end) +     
      (case when (invoicetype like 'DN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment'))     
   and OrignalSupplyDate >= @fromdate and OrignalSupplyDate <= @todate     
   then 0-(isnull(LineNetAmount,0)-isnull(AdvanceRcptAmtAdjusted,0))     
 else 0 end),0) as NetAmount,    
          
      0 as vatamount   ,
	  isnull((case when (invoicetype like 'Purchase Entry%')     
      then isnull(LineAmountInclusiveVAT,0)-isnull(AdvanceRcptAmtAdjusted,0)+isnull(customSPAID,0)+isnull(excisetaxpaid,0)+isnull(otherchargespaid,0)     
 else 0 end) -     
      (case when (invoicetype like 'CN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment'))     
   and OrignalSupplyDate  >= @fromdate and OrignalSupplyDate <= @todate     
   then 0-(isnull(LineAmountInclusiveVAT,0)-isnull(AdvanceRcptAmtAdjusted,0))     
 else 0 end) +     
      (case when (invoicetype like 'DN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment'))     
   and OrignalSupplyDate >= @fromdate and OrignalSupplyDate <= @todate     
   then 0-(isnull(LineAmountInclusiveVAT,0)-isnull(AdvanceRcptAmtAdjusted,0))     
 else 0 end),0) as NetAmount
 from VI_importstandardfiles_Processed sales    
 where  ((invoicetype like 'CN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment')) or     
          (invoicetype like 'DN Purchase%' and (Transtype = 'Purchases' or Transtype = 'AdvancePayment')) or Invoicetype like 'Purchase Entry%')     
and  left(BuyerCountryCode,2) <> 'SA' and VatRate=5  and (VATDeffered =1 or RCMApplicable =1)    
and effdate >= @fromdate and effdate <= @todate and TenantId=@tenantId ;    
end
GO
