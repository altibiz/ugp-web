declare @idPattern nvarchar(50)
SET @idPattern='offer_%'
DELETE FROM AliasPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM PersonPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM TaxonomyIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM ContainedPartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM ContentItemIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM PaymentIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM OfferIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE FROM AutoroutePartIndex WHERE documentId IN
(SELECT Id
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern)

DELETE
  FROM [dbo].[Document]
  WHERE JSON_VALUE(Content,'$.ContentItemId') Like @idPattern