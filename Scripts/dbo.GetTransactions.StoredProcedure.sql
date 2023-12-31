USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[GetTransactions]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     procedure [dbo].[GetTransactions]

as
begin
  

	 select 'Sales' as 'Type',
    count(rg.Id) as 'RuleGroupCount',
    5 as 'ExecutionCount'
    from RuleGroup rg
	where rg.transactionType='Sales'

	union

	
	 select 'Credit' as 'Type',
    count(rg.Id) as 'RuleGroupCount',
    10 as 'ExecutionCount'
    from RuleGroup rg
	where rg.transactionType='Credit'

	union

	 select 'Debit' as 'Type',
    count(rg.Id) as 'RuleGroupCount',
    0 as 'ExecutionCount'
    from RuleGroup rg
	where rg.transactionType='Debit'

	

end
GO
