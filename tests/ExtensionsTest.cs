using jeanie.Lib;
using jeanie.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void ItPreviewsStringWithEllipses()
        {
            Assert.AreEqual("This is too long".Preview(6), "This i...");
        }

        [TestMethod]
        public void ItReturnsNullWhenPreviewingNullString()
        {
            string nullstring = null;
            Assert.AreEqual(nullstring.Preview(4), null);
        }

        [TestMethod]
        public void ItCamelCasesTheEnumValue()
        {
            Assert.AreEqual(ReservationStatus.ReminderSent.Text(), "Reminder Sent");
        }

        [TestMethod]
        public void ItReturnsOnlyFirstWordWithCapitalization()
        {
            Assert.AreEqual("biLL Beau".FirstWord(), "Bill");
        }

        [TestMethod]
        public void ItReturnsEmptyFirstWordWhenBlank()
        {
            Assert.AreEqual("".FirstWord(), "");
        }

        [TestMethod]
        public void ItReturnsNullFirstWordWhenNull()
        {
            string nullstring = null;
            Assert.AreEqual(nullstring.FirstWord(), null);
        }
    }
}
