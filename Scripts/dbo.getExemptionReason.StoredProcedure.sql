USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[getExemptionReason]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[getExemptionReason]
@vatcode nvarchar(10)
AS
BEGIN
select Id,Name,Description,Code from ExemptionReason where Code=@vatcode and tenantid is NULL AND IsActive=1
END
GO
