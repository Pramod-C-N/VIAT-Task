USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_UpdateWHTStandardRates]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    procedure [dbo].[VI_UpdateWHTStandardRates]   --- exec VI_UpdateWHTStandardRates 535        

(        
@batchno numeric        
)        
as        
begin        
declare @whtrate decimal(5,2)        
  
delete from vi_paymentWHTrate where batchid = @batchno and rateslno in (10,11,12,13,110,111)        
update vi_importstandardfiles_processed set AdvanceRcptRefNo =0 where batchid = @batchno  


exec SP_WHTDTTSpecialRate @batchno
  
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName)         
select v.UniqueIdentifier,w.specialRates ,@batchno,13,w.servicename from VI_importstandardfiles_Processed v inner join whtdttrates w 
on upper(v.NatureofServices) = upper(w.servicename)    
--inner join   vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%'  and left(v.BuyerCountryCode,2)=w.AlphaCode and    
AdvanceRcptRefNo = 13 and v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        
  
  
--- DTT Rates    
    
--delete from vi_paymentWHTrate where batchid = @batchno and rateslno in (10,11,12,13)        
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName)         
select v.UniqueIdentifier,w.DTTRates ,@batchno,12,w.servicename from VI_importstandardfiles_Processed v inner join whtdttrates w 
on upper(v.NatureofServices) = upper(w.servicename) 
--left outer vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%'  
--and p.rateslno not in (13) 
and left(v.BuyerCountryCode,2)=w.AlphaCode and    
v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        
        
--- final rates updates        
        
--delete from vi_paymentWHTrate where batchid = @batchno and rateslno = 11        
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName,lawrate)     
select v.UniqueIdentifier,w.AffiliationRate ,@batchno,11,w.servicename,w.standardrate as lawrate from VI_importstandardfiles_Processed v inner join mst_whtrates w on 
upper(v.NatureofServices) = upper(w.servicename)  --inner join   vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%'  and trim(AffiliationStatus) ='Affiliate' and upper(trim(PlaceofSupply)) = 'INSIDE COUNTRY'  and   -- p.rateslno not in (12,13) and    
v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        
        
--delete from vi_paymentWHTrate where batchid = @batchno and rateslno = 10        
        
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName,lawrate)         
select v.UniqueIdentifier,w.standardrate,@batchno,10,w.servicename,w.standardrate as lawrate from VI_importstandardfiles_Processed v inner join mst_whtrates w on upper(v.NatureofServices) = upper(w.servicename)-- inner join   vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%' and trim(AffiliationStatus) ='Non-affiliate' and upper(trim(PlaceofSupply)) = 'INSIDE COUNTRY' and  -- p.rateslno not in (11,12,13) and    
v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        
        
--delete from vi_paymentWHTrate where batchid = @batchno and rateslno = 111        
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName,lawrate)     
select v.UniqueIdentifier,w.AffiliationRate ,@batchno,111,w.servicename,w.standardrate_ook as lawrate from VI_importstandardfiles_Processed v inner join mst_whtrates w on 
upper(v.NatureofServices) = upper(w.servicename)  --inner join   vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%'  and trim(AffiliationStatus) ='Affiliate' and  upper(trim(PlaceofSupply)) = 'OUTSIDE COUNTRY'  -- p.rateslno not in (12,13) and    
and v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        
        
--delete from vi_paymentWHTrate where batchid = @batchno and rateslno = 10        
        
insert into vi_PaymentWHTRate(uniqueidentifier,standardrate,batchid,rateslno,ServiceName,lawrate)         
select v.UniqueIdentifier,w.standardrate,@batchno,110,w.servicename,w.standardrate_ook as lawrate from VI_importstandardfiles_Processed v inner join mst_whtrates w on upper(v.NatureofServices) = upper(w.servicename)-- inner join   vi_PaymentWHTRate p on p.Batchid=v.BatchId    
where w.status = 1 and v.BatchId=@batchno and v.InvoiceType like 'WHT%' and trim(AffiliationStatus) ='Non-affiliate' and upper(trim(PlaceofSupply)) = 'OUTSIDE COUNTRY' and  -- p.rateslno not in (11,12,13) and    
v.UniqueIdentifier not in (select UniqueIdentifier from vi_paymentwhtrate where batchid = @batchno)        

update vi_PaymentWHTRate set lawrate = (select standardrate from mst_whtrates where mst_whtrates.servicename =  vi_PaymentWHTRate.ServiceName) 
where batchid = @batchno and rateslno in (12,13)

update vi_paymentwhtrate set effrate = case when lawrate < standardrate then lawrate else standardrate end where batchid= @batchno

end        

-- update the lawrate into vi_paymentwhtrate


--select * from mst_whtrates

--select * from vi_paymentwhtrate

--select * from vi_paymentwhtrate

--alter table vi_paymentwhtrate add LawRate numeric(5,2), EffRate numeric(5,2),ServiceName nvarchar(max)

--alter table vi_paymentwhtrate add ServiceName nvarchar(max)
    
--select AffiliationStatus,uniqueidentifier,BuyerName,TotalTaxableAmount ,LineAmountInclusiveVAT  from VI_importstandardfiles_Processed where BatchId=263719 and  UniqueIdentifier ='F4ACDFAF-D5CA-4D8E-99BF-7DF964F71DAC' and trim(buyername) = 'OLIVER MARKET
  
--ING LTD' ItemName='Technical Consultancy'    
--update VI_importstandardfiles_Processed set AffiliationStatus='Affiliate' where UniqueIdentifier ='F4ACDFAF-D5CA-4D8E-99BF-7DF964F71DAC' BuyerName='By Niggi LLC' and BatchId=263719    
    
    
--select *  from VI_importstandardfiles_Processed where BatchId=263719     
    
--select * from vi_paymentwhtrate where UniqueIdentifier ='F4ACDFAF-D5CA-4D8E-99BF-7DF964F71DAC'    
    
--select     
GO
