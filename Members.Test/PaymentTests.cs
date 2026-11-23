using Members.Payments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Members.Test
{
    [TestClass]
    public class PaymentTests
    {
        [TestMethod]
        public void ConvertBsXmlToJson()
        {
            var bs = BankStatPartService.ParseStmt(Constants.xmlbnkstatmnt);
            Assert.IsNotNull(bs.Date);
            Assert.AreEqual("CAMT053/0000000000/000/2025/00/001", bs.StatementId);
            Assert.AreEqual("00", bs.LglSeqNbr);
            Assert.AreEqual("DUMMY CREDITOR", bs.Data[0].Partner.Name);
            Assert.AreEqual("Isplata", bs.Data[0].Type);
            Assert.AreEqual("XX00", bs.Data[0].RRN.Model);

            Assert.AreEqual("DUMMY COMPANY", bs.Data[1].Partner.Name);
            Assert.AreEqual("Uplata", bs.Data[1].Type);
            Assert.AreEqual("00000000000000000001", bs.Data[0].Number);

            bs = BankStatPartService.ParseStmt(Constants.jsonbnkstatement);
            Assert.IsNotNull(bs.Date);

            Assert.AreEqual("Something someone", bs.Data[0].Partner.Name);
            Assert.AreEqual("Uplata", bs.Data[0].Type);
            Assert.AreEqual("2020-00000000-0000000000", bs.Data[0].Number);
        }

        [TestMethod]
        public void ParseXmlBankMultipleStatements()
        {
            var bs = BankStatPartService.ParseStmt(Constants.xmlBankMultipleStatetments);

            // Check correct number of entries (5 from first statement + 4 from second statement)
            Assert.AreEqual(9, bs.Data.Count, "Should have 9 total entries from both statements");

            // Check that Date (start date) and EndDate are different
            Assert.IsNotNull(bs.Date, "Date should not be null");
            Assert.IsNotNull(bs.EndDate, "EndDate should not be null");
            Assert.AreNotEqual(bs.Date, bs.EndDate, "Date and EndDate should be different");

            // Check that Date is the first statement's start date (2025-12-22)
            Assert.AreEqual(new DateTime(2025, 12, 22), bs.Date.Value.Date, "Date should be 2025-12-22 (first statement start)");

            // Check that EndDate is the last statement's start date (2025-12-24)
            Assert.AreEqual(new DateTime(2025, 12, 24), bs.EndDate.Value.Date, "EndDate should be 2025-12-24 (last statement start)");

            // Check that not all entry dates are the same
            var distinctDates = bs.Data.Select(d => d.Date?.Date).Distinct().ToList();
            Assert.IsTrue(distinctDates.Count > 1, "Entry dates should not all be the same");

            // Verify the first 5 entries have date 2025-12-22 (from first statement)
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(new DateTime(2025, 12, 22), bs.Data[i].Date.Value.Date, 
                    $"Entry {i} should have date 2025-12-22");
            }

            // Verify the last 4 entries have date 2025-12-24 (from second statement)
            for (int i = 5; i < 9; i++)
            {
                Assert.AreEqual(new DateTime(2025, 12, 24), bs.Data[i].Date.Value.Date, 
                    $"Entry {i} should have date 2025-12-24");
            }

            // Verify statement metadata
            Assert.AreEqual("157", bs.LglSeqNbr, "LglSeqNbr should be from the last statement");
            Assert.AreEqual("CAMT053/0000000000/000/2025/02/001", bs.StatementId, 
                "StatementId should be from the last statement");
        }


        [TestMethod]
        public void ParseHtmlBankStatement()
        {
            var bs = BankStatPartService.ParseStmt(Constants.htmlBankStatements);

            Assert.IsNotNull(bs.Date);
            Assert.AreEqual(new DateTime(2026, 1, 6), bs.Date.Value.Date);
            Assert.AreEqual("005", bs.StatementId);
            Assert.AreEqual("005", bs.LglSeqNbr);

            Assert.AreEqual(1, bs.Data.Count);
            Assert.AreEqual("DUMMY COMPANY D.O.O. ZAGREB", bs.Data[0].Partner.Name);
            Assert.AreEqual("Uplata", bs.Data[0].Type);
            Assert.AreEqual(30.00m, bs.Data[0].Amount);
            Assert.AreEqual("HR99", bs.Data[0].RRN.Model);
            Assert.AreEqual("00-00000000000", bs.Data[0].RRN.Number);
            Assert.AreEqual("2026-0000000-00000000000", bs.Data[0].Number);
            Assert.AreEqual(new DateTime(2026, 1, 6), bs.Data[0].Date.Value.Date);
        }

        private const string xmlNoReferenceEntry = @"
            <Ntry>
              <AcctSvcrRef>{0}</AcctSvcrRef>
              <BkTxCd><Domn><Cd>PMNT</Cd><Fmly><Cd>{1}</Cd><SubFmlyCd>NTAV</SubFmlyCd></Fmly></Domn></BkTxCd>
              <NtryDtls><TxDtls>
                <Refs><AcctSvcrRef>{0}</AcctSvcrRef><EndToEndId>HR99</EndToEndId></Refs>
                <AmtDtls><TxAmt><Amt Ccy=""EUR"">{2}</Amt></TxAmt></AmtDtls>
                <RltdPties><{3}><Pty><Nm>DUMMY PARTY</Nm></Pty></{3}></RltdPties>
                <RmtInf>
                  <Ustrd>{4}</Ustrd>
                  <Strd><CdtrRefInf><Tp><CdOrPrtry><Cd>SCOR</Cd></CdOrPrtry></Tp></CdtrRefInf></Strd>
                </RmtInf>
              </TxDtls></NtryDtls>
            </Ntry>";

        [TestMethod]
        public void ParseXmlEntriesWithoutReference()
        {
            // HR99 payments have no Strd/CdtrRefInf/Ref and no Strd/AddtlRmtInf — only RmtInf/Ustrd
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.08"">
  <BkToCstmrStmt>
    <Stmt>
      <Id>CAMT053/TEST</Id>
      <LglSeqNb>1</LglSeqNb>
      <FrToDt><FrDtTm>2026-09-28T00:00:00</FrDtTm></FrToDt>"
                + string.Format(xmlNoReferenceEntry, "2026-0000001-00000000001", "RCDT", "50.00", "Dbtr", "HR00 00000000001 - Donacija")
                + string.Format(xmlNoReferenceEntry, "2026-0000002-00000000002", "ICDT", "242.00", "Cdtr", "Membership fee")
                + @"
    </Stmt>
  </BkToCstmrStmt>
</Document>";

            var bs = BankStatPartService.ParseStmt(xml);

            Assert.AreEqual(2, bs.Data.Count);

            Assert.AreEqual("Uplata", bs.Data[0].Type);
            Assert.AreEqual(50.00m, bs.Data[0].Amount);
            Assert.AreEqual("HR99", bs.Data[0].RRN.Model);
            Assert.AreEqual("00000000001", bs.Data[0].RRN.Number, "Reference typed into the description should be picked up");
            Assert.AreEqual("HR00 00000000001 - Donacija", bs.Data[0].PaymentDescription);

            Assert.AreEqual("Isplata", bs.Data[1].Type);
            Assert.AreEqual("", bs.Data[1].RRN.Number);
            Assert.AreEqual("Membership fee", bs.Data[1].PaymentDescription);
            Assert.AreEqual("DUMMY PARTY", bs.Data[1].Partner.Name);
        }

    }
}
