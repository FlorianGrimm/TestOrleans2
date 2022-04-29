DELETE FROM [history].[ToDoHistory]
GO
DELETE FROM [history].[ProjectHistory]
GO
DELETE FROM [dbo].[ToDo]
GO
DELETE FROM [dbo].[Project]
GO

DELETE FROM dbo.Operation
FROM dbo.Operation 
LEFT OUTER JOIN (
	SELECT [OperationId]
	  FROM [dbo].[Project]
	UNION
	SELECT [OperationId]
	  FROM [dbo].[ToDo]
) as x
	ON Operation.OperationId = x.OperationId
LEFT OUTER JOIN 
	[history].[UserHistory] as xx
ON Operation.OperationId = xx.OperationId
WHERE (x.OperationId IS NULL) AND (xx.OperationId IS NULL) 
GO

--exec [dbo].[UserSelectByUserName] @UserName=N'GRIMMBART\flori'
--go
--exec [dbo].[ProjectSelectPK] @ProjectId='A4452A4F-E07D-4A85-A6BD-5F2F9DA0F993'
--go
--exec [dbo].[ProjectSelectPK] @ProjectId='A4452A4F-E07D-4A85-A6BD-5F2F9DA0F993'
--go
--exec [dbo].[OperationInsert] @OperationId='D279B5A6-6153-4053-B70D-635289D5E5C1',@Title=N'ProjectController.Po',@EntityType=N'Project',@EntityId=N'a4452a4f-e07d-4a85-a6bd-5f2f9da0f993',@Data=N'{"Username":"GRIMMBART\\flori","Method":"POST","Path":"/api/Project","Form":null,"ArgumentType":"Replacement.Contracts.API.Project","Argument":{"ProjectId":"a4452a4f-e07d-4a85-a6bd-5f2f9da0f993","Title":"a4452a4f-e07d-4a85-a6bd-5f2f9da0f993","OperationId":"00000000-0000-0000-0000-000000000000","CreatedAt":"0001-01-01T00:00:00+00:00","CreatedBy":null,"ModifiedAt":"0001-01-01T00:00:00+00:00","ModifiedBy":null,"SerialVersion":0}}',@CreatedAt='2022-04-28 16:22:24.9381072 +02:00',@UserId='D80EB031-6FAE-4CAB-9267-165C93AF499B'
--go
--exec [dbo].[ProjectUpsert] @ProjectId='A4452A4F-E07D-4A85-A6BD-5F2F9DA0F991',@Title=N'a4452a4f-e07d-4a85-a6bd-5f2f9da0f991',@OperationId='D279B5A6-6153-4053-B70D-635289D5E5C1',@CreatedAt='2022-04-28 16:22:24.9381072 +02:00',@CreatedBy='D80EB031-6FAE-4CAB-9267-165C93AF499B',@ModifiedAt='2022-04-28 16:22:24.9381072 +02:00',@ModifiedBy='D80EB031-6FAE-4CAB-9267-165C93AF499B',@SerialVersion=0
--go
--exec [dbo].[OperationInsert] @OperationId='09FE6307-F9A7-400A-89B9-3B7B9ADBB3EA',@Title=N'ProjectController.Po',@EntityType=N'Project',@EntityId=N'a4452a4f-e07d-4a85-a6bd-5f2f9da0f993',@Data=N'{"Username":"GRIMMBART\\flori","Method":"POST","Path":"/api/Project","Form":null,"ArgumentType":"Replacement.Contracts.API.Project","Argument":{"ProjectId":"a4452a4f-e07d-4a85-a6bd-5f2f9da0f993","Title":"28.04.2022 16:22:31","OperationId":"d279b5a6-6153-4053-b70d-635289d5e5c0","CreatedAt":"2022-04-28T16:22:24.9381072+02:00","CreatedBy":"d80eb031-6fae-4cab-9267-165c93af499b","ModifiedAt":"2022-04-28T16:22:24.9381072+02:00","ModifiedBy":"d80eb031-6fae-4cab-9267-165c93af499b","SerialVersion":79100}}',@CreatedAt='2022-04-28 16:22:32.0614353 +02:00',@UserId='D80EB031-6FAE-4CAB-9267-165C93AF499B'
--go
--exec [dbo].[ProjectUpsert] @ProjectId='A4452A4F-E07D-4A85-A6BD-5F2F9DA0F993',@Title=N'28.04.2022 16:22:31',@OperationId='09FE6307-F9A7-400A-89B9-3B7B9ADBB3EA',@CreatedAt='2022-04-28 16:22:24.9381072 +02:00',@CreatedBy='D80EB031-6FAE-4CAB-9267-165C93AF499B',@ModifiedAt='2022-04-28 16:22:32.0614353 +02:00',@ModifiedBy='D80EB031-6FAE-4CAB-9267-165C93AF499B',@SerialVersion=79100
--go
