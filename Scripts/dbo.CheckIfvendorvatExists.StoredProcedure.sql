USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CheckIfvendorvatExists]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create    procedure [dbo].[CheckIfvendorvatExists]
(
@vat nvarchar(200) ,
@checkInAllVendor char(1) = 1
)
as
begin
select count(*) as count from VendorDocuments with(nolock) where DocumentTypeCode='VAT' and DocumentNumber=@vat
end
GO
