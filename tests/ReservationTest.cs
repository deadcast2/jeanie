using System;
using System.Collections.Generic;
using jeanie.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class ReservationTest
    {
        private List<(DateTime start, DateTime end)> FullDay = new List<(DateTime start, DateTime end)>
        {
            (DateTime.Now.Date.AddHours(9), DateTime.Now.Date.AddHours(12)),
            (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
            (DateTime.Now.Date.AddHours(11), DateTime.Now.Date.AddHours(14)),
            (DateTime.Now.Date.AddHours(12), DateTime.Now.Date.AddHours(15)),
            (DateTime.Now.Date.AddHours(13), DateTime.Now.Date.AddHours(16)),
            (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17)),
            (DateTime.Now.Date.AddHours(15), DateTime.Now.Date.AddHours(18)),
            (DateTime.Now.Date.AddHours(16), DateTime.Now.Date.AddHours(19)),
            (DateTime.Now.Date.AddHours(17), DateTime.Now.Date.AddHours(20)),
            (DateTime.Now.Date.AddHours(18), DateTime.Now.Date.AddHours(21))
        };

        [TestMethod]
        public void CorrectAmountOfTimeSlotsGenerated()
        {
            var timeSlots = ReservationHelper.GetTimeSlots(DateTime.Now);

            Assert.AreEqual(FullDay.Count, timeSlots.Count);
        }

        [TestMethod]
        public void CorrectTimeSlotsGenerated()
        {
            var timeSlots = ReservationHelper.GetTimeSlots(DateTime.Now);

            for (int i = 0; i < timeSlots.Count; i++)
            {
                Assert.AreEqual(timeSlots[i].start, FullDay[i].start);
                Assert.AreEqual(timeSlots[i].end, FullDay[i].end);
            }
        }

        [TestMethod]
        public void TimeSlotsFiltered()
        {
            var timeSlots = ReservationHelper.GetTimeSlots(DateTime.Now);

            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            var openTimeSlots = ReservationHelper.FilterTimeSlots(bookedTimeSlots, timeSlots);

            Assert.AreEqual(timeSlots.Count - bookedTimeSlots.Count, openTimeSlots.Count);
        }

        [TestMethod]
        public void ReturnsFalseWhenDayNotFullyBooked()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            Assert.IsFalse(ReservationHelper.IsDayFullyBooked(DateTime.Now, bookedTimeSlots));
        }

        [TestMethod]
        public void ReturnsTrueWhenDayFullyBooked()
        {
            Assert.IsTrue(ReservationHelper.IsDayFullyBooked(DateTime.Now, FullDay));
        }

        [TestMethod]
        public void ReturnsTrueWhenTimeSlotFits()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            var newTimeSlot = (DateTime.Now.Date.AddHours(17), DateTime.Now.Date.AddHours(20));

            Assert.IsTrue(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotStartOverlaps()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            var newTimeSlot = (DateTime.Now.Date.AddHours(11), DateTime.Now.Date.AddHours(14));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotEndOverlaps()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            var newTimeSlot = (DateTime.Now.Date.AddHours(9), DateTime.Now.Date.AddHours(12));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotOutOfBounds()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Now.Date.AddHours(10), DateTime.Now.Date.AddHours(13)),
                (DateTime.Now.Date.AddHours(14), DateTime.Now.Date.AddHours(17))
            };

            var newTimeSlot = (DateTime.Now.Date.AddHours(21), DateTime.Now.Date.AddHours(24));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }
    }
}
