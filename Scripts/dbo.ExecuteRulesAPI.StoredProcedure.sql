USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[ExecuteRulesAPI]    Script Date: 01-09-2023 17:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROCEDURE [dbo].[ExecuteRulesAPI]  
(  
@RuleGroupId int=3 ,  
@tenantId int=2,  
@json nvarchar(max)='{
  "Identifier": "Sales",
  "Name": "ert",
  "Description": "retertre",
  "BuyerIdentifier": null,
  "SellerIdentifier": null,
  "StandardIdentifier": null,
  "Quantity": 1.0,
  "UOM": "1",
  "UnitPrice": 1200.0,
  "CostPrice": 0.0,
  "DiscountPercentage": 0.0,
  "DiscountAmount": 0.0,
  "GrossPrice": 0.0,
  "NetPrice": 1200.0,
  "VATRate": 15.0,
  "VATCode": "S",
  "VATAmount": 180.0,
  "LineAmountInclusiveVAT": 1380.0,
  "CurrencyCode": "SAR",
  "TaxSchemeId": null,
  "Notes": null,
  "ExcemptionReasonCode": null,
  "ExcemptionReasonText": null,
  "uuid": "928ffbb4-406c-454b-9a4a-4fa074ff94a2"
}'
  
)  
AS  
BEGIN  
  
declare @query nvarchar(max)=''  
declare @ParentTable nvarchar(max) = (select parentTable from rulegroup where id = @RuleGroupId)  
  
declare @SelectQuery nvarchar(max) = 'select * from  OPENJSON(@json)'  
  
DECLARE @jsonData AS dbo.DataDictionaryType  
INSERT into @jsonData execute sp_executesql @SelectQuery,@Params = N'@json nvarchar(max)', @json =@json  

Declare @uuid as nvarchar(500)= (select [value] from @jsonData where [key]='uuid') 
  
DECLARE @cols AS NVARCHAR(MAX)  
  
select @cols = STUFF((SELECT ',' + QUOTENAME([key])   
                    from @jsonData  
                    group by [key], [value]  
                    order by [key]  
            FOR XML PATH(''), TYPE  
            ).value('.', 'NVARCHAR(MAX)')   
        ,1,1,'')  
  
declare @TransformQuery nvarchar(max) = N'(SELECT ' + @cols + N' from   
             (  
                select [value], [key]  
                from @jsonData  
            ) x  
            pivot   
            (  
                max(value)  
                for [key] in (' + @cols + N')  
            ) p ) as t'  
  
   
  
declare @sql nvarchar(max);  
declare @desc nvarchar(max);  
declare @field nvarchar(max);  
declare @error_code  nvarchar(max);  
declare @stop_condition  int;  
declare @next_rule_on_success int;  
declare @next_rule_on_failure int;  
declare @output int;  
declare @count int=0;  
declare @skip int=0;  
  
declare @v_sql CURSOR;  
set @v_sql=   
CURSOR FOR  
select r.SqlStatement,r.OnSuccessNext,r.OnFailureNext,r.errorCode,r.StopCondition,r.[key] from [Rule] r  
where r.RuleGroupId=@RuleGroupId and r.isActive=1  
order by r.[Order]  
  
declare @batchId int = (select max(batchId) from RulesEngineLog)+1  
   -------------------------
   if(@RuleGroupId=3)
   begin
 set @query = 'INSERT INTO tmp_items([AdditionalData1],[AdditionalData2],[BuyerIdentifier],[CostPrice],[CurrencyCode],[Description],[DiscountAmount],[DiscountPercentage],[ExcemptionReasonCode],[ExcemptionReasonText],[GrossPrice],[Identifier],[isOtherCharges],[Language],[LineAmountInclusiveVAT],[Name],[NetPrice],[Notes],[Quantity],[SellerIdentifier],[StandardIdentifier],[TaxSchemeId],[UnitPrice],[UOM],[uuid],[VATAmount],[VATCode],[VATRate]) SELECT * FROM '+@TransformQuery;
EXECUTE sp_executesql @query,@Params = N'@jsonData dbo.DataDictionaryType READONLY', @jsonData = @jsonData;
end
 -------------------------
set @query  = 'insert into RulesEngineLog(TableName,Field,errorCode,isSuccess,batchId,refBatchId) select '''+@ParentTable+''',@field,@error_code,0,'''+cast(@batchId as nvarchar)+''',null FROM '+@TransformQuery;  
  
--declare @queryToGetField nvarchar(max) = 'select * from @jsonData where [key]'  
  
OPEN @v_sql  
FETCH NEXT   
FROM @v_sql INTO @sql,@next_rule_on_success,@next_rule_on_failure,@error_code,@stop_condition,@field;  
WHILE @@FETCH_STATUS = 0  
BEGIN  
   set @count=@count+1;  
      print @sql
   set @sql = @query + ' WHERE ' + @sql  ;  
  -- print @sql
   EXECUTE sp_executesql @SQL,@Params = N'@jsonData dbo.DataDictionaryType READONLY,@error_code INT,@field nvarchar(max),@items_uuid nvarchar(500)', @jsonData = @jsonData, @error_code=@error_code,@field=@field,@items_uuid=@uuid;  
  
 FETCH NEXT   
 FROM @v_sql INTO @sql,@next_rule_on_success,@next_rule_on_failure,@error_code,@stop_condition,@field;  
  
END  
  
CLOSE @v_sql  
  
  
DEALLOCATE @v_sql  

 if(@RuleGroupId<>3)
   begin
delete from tmp_items where uuid=@uuid;  
  end
-- select @output  
 --select STUFF((SELECT e.ZatcaCode+'--'+e.ZatcaErrorMessage  +';'
 --                   from RulesEngineLog r  
 --    inner join CustomErrorType e  
 --    on r.errorCode = e.Code  
 --    where r.batchId=@batchId and e.IsActive=1  
 --           FOR XML PATH(''), TYPE  
 --           ).value('.', 'NVARCHAR(MAX)')   
 --       ,1,0,'') as error  

 select '' as error
  
  
END
GO
