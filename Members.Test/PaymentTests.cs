using Members.Payments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
