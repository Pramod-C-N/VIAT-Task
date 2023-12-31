USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[PaymentPlaceofServiceValidations]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create     procedure [dbo].[PaymentPlaceofServiceValidations]  -- exec PaymentPlaceofServiceValidations 859256      
(      
@BatchNo numeric ,
@validStat int
)      
as      
begin      
      
if @validStat=1    
begin      
delete from importstandardfiles_errorlists where batchid = @BatchNo and errortype = 115      
end 
if @validStat=1
begin      
insert into importstandardfiles_errorlists(tenantid,BatchId,uniqueidentifier,Status,ErrorMessage,Errortype,isdeleted,CreationTime)       
select tenantid,@batchno,uniqueidentifier,'0','Invalid Place of performance of Service',115,0,getdate() from ImportBatchData       
where Invoicetype like 'WHT%' and batchid = @batchno       
and ((PlaceofSupply  is null or PlaceofSupply  ='') or (trim(upper(PlaceofSupply)) not in (select trim(upper(description))  from PlaceOfPerformance )))         
end      
      
end
GO
