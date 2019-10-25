using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class ReservationsController : Controller
    {
        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            var reservation = GetReservation(id);

            if (reservation == null)
            {
                TempData["error"] = "Sorry but the reservation could not be found.";
                return Redirect("/");
            }
            else if (reservation.IsBooked)
            {
                TempData["error"] = "Sorry but the reservation has already been booked.";
                return Redirect("/");
            }

            return View(reservation);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Update(ReservationViewModel model)
        {
            using (var context = new JeanieContext())
            {
                var reservation = context.Reservations.Find(model.Id);
                if (reservation == null) return Edit(model.Id);

                // So name validation succeeds.
                model.Name = reservation.Name;

                var hourRange = (model.TimeSlot ?? "").Split('-');
                if (model.IsValid(strict: true) && hourRange.Length == 2)
                {
                    int.TryParse(hourRange[0], out int startHour);
                    reservation.StartDate = model.Date?.AddHours(startHour);
                    int.TryParse(hourRange[1], out int endHour);
                    reservation.EndDate = model.Date?.AddHours(endHour);

                    reservation.Grade = model.Grade;
                    reservation.Notes = model.Notes;

                    if (!ReservationHelper.IsValidTimeSlot(ReservationHelper.GetReservations(model.Date.Value),
                        (reservation.StartDate.Value, reservation.EndDate.Value)))
                    {
                        TempData["error"] = "Sorry but that time slot is no longer available.";
                    }
                    else if (context.SaveChanges() > 0)
                    {
                        TempData["success"] = ViewHelpers.RenderToString(ControllerContext, 
                            "_Success", GetReservation(model.Id));
                        return Redirect("/");
                    }
                }
            }

            return View("Edit", model);
        }

        private ReservationViewModel GetReservation(Guid? id)
        {
            using (var context = new JeanieContext())
            {
                var reservation = context.Reservations.Find(id);
                if (reservation == null)
                {
                    return null;
                }

                return new ReservationViewModel(reservation);
            }
        }
    }
}
