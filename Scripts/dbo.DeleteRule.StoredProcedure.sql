USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[DeleteRule]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     procedure [dbo].[DeleteRule]
    (
        @Id int 
    )
    as
    begin
        delete from RuleDetails where RuleId = @Id
        delete from [Rule] where Id = @Id
    end
GO
