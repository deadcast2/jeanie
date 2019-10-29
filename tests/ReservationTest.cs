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
            (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12)),
            (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
            (DateTime.Today.AddHours(11), DateTime.Today.AddHours(14)),
            (DateTime.Today.AddHours(12), DateTime.Today.AddHours(15)),
            (DateTime.Today.AddHours(13), DateTime.Today.AddHours(16)),
            (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17)),
            (DateTime.Today.AddHours(15), DateTime.Today.AddHours(18)),
            (DateTime.Today.AddHours(16), DateTime.Today.AddHours(19)),
            (DateTime.Today.AddHours(17), DateTime.Today.AddHours(20)),
            (DateTime.Today.AddHours(18), DateTime.Today.AddHours(21))
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
        public void TimeSlotsFilteredPartially()
        {
            var timeSlots = ReservationHelper.GetTimeSlots(DateTime.Now);

            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12)),
                (DateTime.Today.AddHours(13), DateTime.Today.AddHours(16))
            };

            var openTimeSlots = ReservationHelper.FilterTimeSlots(bookedTimeSlots, timeSlots);

            Assert.AreEqual(openTimeSlots.Count, 2);
            Assert.AreEqual(openTimeSlots[0].start, DateTime.Today.AddHours(17));
            Assert.AreEqual(openTimeSlots[0].end, DateTime.Today.AddHours(20));
            Assert.AreEqual(openTimeSlots[1].start, DateTime.Today.AddHours(18));
            Assert.AreEqual(openTimeSlots[1].end, DateTime.Today.AddHours(21));
        }

        [TestMethod]
        public void TimeSlotsFilteredFully()
        {
            var timeSlots = ReservationHelper.GetTimeSlots(DateTime.Now);

            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12)),
                (DateTime.Today.AddHours(13), DateTime.Today.AddHours(16)),
                (DateTime.Today.AddHours(17), DateTime.Today.AddHours(20))
            };

            var openTimeSlots = ReservationHelper.FilterTimeSlots(bookedTimeSlots, timeSlots);

            Assert.AreEqual(openTimeSlots.Count, 0);
        }

        [TestMethod]
        public void ReturnsFalseWhenDayNotFullyBooked()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
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
                (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
            };

            var newTimeSlot = (DateTime.Today.AddHours(18), DateTime.Today.AddHours(21));

            Assert.IsTrue(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotStartOverlaps()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
            };

            var newTimeSlot = (DateTime.Today.AddHours(11), DateTime.Today.AddHours(14));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotEndOverlaps()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
            };

            var newTimeSlot = (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotOutOfBounds()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(10), DateTime.Today.AddHours(13)),
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
            };

            var newTimeSlot = (DateTime.Today.AddHours(21), DateTime.Today.AddHours(24));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotBackToBackAfter()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12))
            };

            var newTimeSlot = (DateTime.Today.AddHours(12), DateTime.Today.AddHours(15));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ReturnsFalseWhenTimeSlotBackToBackBefore()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(12), DateTime.Today.AddHours(15))
            };

            var newTimeSlot = (DateTime.Today.AddHours(9), DateTime.Today.AddHours(12));

            Assert.IsFalse(ReservationHelper.IsValidTimeSlot(bookedTimeSlots, newTimeSlot));
        }

        [TestMethod]
        public void ChecksAvailabilityForReservation()
        {
            var bookedTimeSlots = new List<(DateTime start, DateTime end)>
            {
                (DateTime.Today.AddHours(14), DateTime.Today.AddHours(17))
            };

            var openTimeSlots = ReservationHelper.AvailableTimeSlots(DateTime.Now, bookedTimeSlots);

            Assert.AreEqual(openTimeSlots.Count, 3);
        }

        [TestMethod]
        public void ReturnsTrueWhenDate72HoursInAdvance()
        {
            var result = ReservationHelper.IsValidDate(DateTime.Today.AddDays(3));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsFalseWhenDateNot72HoursInAdvance()
        {
            var result = ReservationHelper.IsValidDate(DateTime.Today.AddDays(1));

            Assert.IsFalse(result);
        }
    }
}
