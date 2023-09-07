USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[CheckIfcustomervatExists]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create    procedure [dbo].[CheckIfcustomervatExists]
(
@vat nvarchar(200) ,
@checkInAllCustomer char(1) = 1
)
as
begin
select count(*) as count from CustomerDocuments with(nolock) where DocumentTypeCode='VAT' and DocumentNumber=@vat
end
GO
