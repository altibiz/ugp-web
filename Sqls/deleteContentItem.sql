BEGIN TRANSACTION;
BEGIN TRY
	declare @deletedIds table ( id int );
	DELETE ci output deleted.documentId into @deletedIds FROM ContentItemIndex ci where contentType='Pledge' and id>80649
	DELETE from TaxonomyIndex WHERE DocumentId in (Select id from @deletedIds)
	DELETE FROM LocalizedContentItemIndex where DocumentId in (Select id from @deletedIds)
	DELETE FROM AliasPartIndex where DocumentId in (Select id from @deletedIds)
	DELETE FROM ContainedPartIndex where DocumentId in (Select id from @deletedIds)
	DELETE from Document where id in (Select id from @deletedIds)
	COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
	PRINT 'Transaction failed: ' + ERROR_MESSAGE();
END CATCH;