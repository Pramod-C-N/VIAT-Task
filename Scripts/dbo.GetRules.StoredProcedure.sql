USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetRules]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  CREATE     procedure [dbo].[GetRules]
  (
    @RuleGroupId int
  )
    as
    begin
    select r.Id, r.RuleGroupId, rd.RuleType, r.errorCode, r.OnSuccessNext, r.OnFailureNext, r.StopCondition, r.SqlStatement as 'sql', r.[Order], rd.RuleValue from [Rule] r inner join RuleDetails rd on r.Id = rd.RuleId where r.RuleGroupId = @RuleGroupId
    end
GO
