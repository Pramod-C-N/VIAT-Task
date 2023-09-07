USE [brady]
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoLogs]    Script Date: 01-09-2023 17:11:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[InsertIntoLogs]
@text nvarchar(max),
@date nvarchar(max),
@tenantid int=null
as
begin
insert into logs(json,date,batchid)values(@text,@date,@tenantid)
end

GO
