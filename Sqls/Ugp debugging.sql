/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [glaspoduzetnikah_clanovi_db].[glaspoduzetnikah_idea].[members] where oib_subject='01591544523'

  DELETE glaspoduzetnikah_clanovi_db.glaspoduzetnikah_idea.members where oib_subject='01591544523'


  SELECT count(*), oib_subject from members where oib_subject is not null group by oib_subject having count(*)>1

  UPDATE [glaspoduzetnikah_clanovi_db].[glaspoduzetnikah_idea].[members] SET oib_subject=RIGHT('0000'+oib_subject,11) where len(oib_subject)<11