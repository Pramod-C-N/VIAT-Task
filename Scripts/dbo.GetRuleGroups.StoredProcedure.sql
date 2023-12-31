USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetRuleGroups]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     procedure [dbo].[GetRuleGroups]
(
    @TransactionType varchar(100) 
)
as
begin
    select rg.id, rg.name, rg.description, rg.transactiontype,rg.[type],rg.parentTable, count(r.id) as numberOfRules
    from rulegroup rg
    left join [Rule] r on rg.id = r.rulegroupid
    where rg.transactiontype = @TransactionType
    group by rg.id, rg.name, rg.description, rg.transactiontype, rg.[Type], rg.parentTable
end
GO
