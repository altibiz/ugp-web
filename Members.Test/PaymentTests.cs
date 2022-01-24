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

            Assert.AreEqual("Dont tell me", bs.Data[0].Partner.Name);
            Assert.AreEqual("Uplata", bs.Data[0].Type);

            Assert.AreEqual("Drugi", bs.Data[1].Partner.Name);
            Assert.AreEqual("Isplata", bs.Data[1].Type);

            bs = BankStatPartService.ParseStmt(Constants.jsonbnkstatement);
            Assert.IsNotNull(bs.Date);

            Assert.AreEqual("Something someone", bs.Data[0].Partner.Name);
            Assert.AreEqual("Uplata", bs.Data[0].Type);
        }
    }
}
