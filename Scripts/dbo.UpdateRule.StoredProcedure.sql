USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[UpdateRule]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE     procedure [dbo].[UpdateRule]
    (
        @Id int,
         @errorCode nvarchar(max),
        @OnSuccessNext int,
        @OnFailureNext int,
        @StopCondition int,
        @Sql nvarchar(max),
        @Order int=null,
        @RuleType nvarchar(100),
        @RuleValue nvarchar(max)
      )
 as
 begin
        declare @RuleId int
        declare @RuleDetailId int
        declare @RuleDetailType int
        declare @RuleDetailValue nvarchar(max)
    
        -- update the rule
        update [Rule] set SqlStatement = @Sql, errorCode = @errorCode, OnSuccessNext = @OnSuccessNext, OnFailureNext = @OnFailureNext, StopCondition = @StopCondition, [Order] = @Order where Id = @Id
        -- update the rule details
        update RuleDetails set RuleType = @RuleType, RuleValue = @RuleValue where RuleId = @Id
    
    end
GO
