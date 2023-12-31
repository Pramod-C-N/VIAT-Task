USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[VI_SP_GetVATDetailedReport]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[VI_SP_GetVATDetailedReport]
(@FieldNo nvarchar(7),
@FromDate datetime,     
@ToDate datetime ,
@tenantid int=null)


--exec VI_SP_GetVATDetailedReport 'A1','2022-09-01', '2022-09-30',33  
as  
begin  
declare @VatDetailedReport as table  
(SlNo int,  
Issuedate datetime,  
InvoiceNumber int,  
NetAmount decimal (18,2),  
VatAmount decimal (18,2),
TotalAmount decimal (18,2)
)  
If @FieldNo ='A1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedSales15Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (1,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A1.1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedSales5Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (2,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A1.2'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedSalesGovtDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (3,'02-02-2023',0,0,0,0)
end

insert into @VatDetailedReport values (4,0,0,0,0,0)

If @FieldNo ='A3'
 begin 
insert into @VatDetailedReport
exec VI_VATReportZeroRatedSalesDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (5,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A4'
 begin 
insert into @VatDetailedReport
exec VI_VATReportExportsDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (6,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A5'
 begin 
insert into @VatDetailedReport
exec VI_VATReportExemptedSalesDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (7,'02-02-2023',0,0,0,0)
end

--insert into @VatDetailedReport(SlNo,Issuedate,InvoiceNumber,NetAmount,VatAmount,TotalAmount) 
--select 8,'Total Sales',sum(NetAmount) from @VatDetailedReport where SlNo <=7

If @FieldNo ='A5'
 begin 
insert into @VatDetailedReport
exec VI_VATReportExemptedSalesDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (7,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A7'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedPurchases15Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (8,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A7.1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedPurchases5Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (9,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A8'
 begin 
insert into @VatDetailedReport
exec VI_VATReportImportSubjecttoVATPaid15Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (10,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A8.1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportImportSubjecttoVATPaid5Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (11,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A9'
 begin 
insert into @VatDetailedReport
exec VI_VATReportImportSubjecttoRCM15Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (12,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A9.1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportImportSubjecttoRCM5Detailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (13,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A10'
 begin 
insert into @VatDetailedReport
exec VI_VATReportZeroRatedPurchasesDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (14,'02-02-2023',0,0,0,0)
end

If @FieldNo ='A11'
 begin 
insert into @VatDetailedReport
exec VI_VATReportExemptPurchasesDetailed @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (15,'02-02-2023',0,0,0,0)
end

If @FieldNo ='B1'
 begin 
insert into @VatDetailedReport
exec VI_VATReportStandardRatedSales15DetailedAdjustment @Fromdate, @Todate
end 
else 
begin
insert into @VatDetailedReport values (16,'02-02-2023',0,0,0,0)
end



select SlNo,Format(Issuedate,'dd-MM-yyyy')as Issuedate,InvoiceNumber,NetAmount,VatAmount,TotalAmount from @VatDetailedReport
end
GO
