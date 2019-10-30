using jeanie.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace tests
{
    [TestClass]
    public class CalendarTest
    {
        [TestMethod]
        public void ReturnsSpecialFormattedDate()
        {
            var startDate = new DateTime(2019, 10, 31, 9, 0, 0);
            Assert.AreEqual(Calendar.FormatDate(startDate), "20191031T090000");
        }
    }
}
