DELETE FROM AliasPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%')

DELETE FROM PersonPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%')

DELETE FROM TaxonomyIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%')

DELETE FROM ContainedPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%')

DELETE FROM ContentItemIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%')

DELETE
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like 'l_%'