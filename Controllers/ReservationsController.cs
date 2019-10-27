using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
                var reservation = GetReservation(model.Id);
                if (reservation == null || reservation.IsBooked) return Edit(model.Id);

                // So name validation succeeds.
                model.Name = reservation.Name;

                if (model.IsValid(strict: true))
                {
                    context.Reservations.Attach(reservation.Source);

                    reservation.Source.StartDate = model.StartDateFromTimeSlot;
                    reservation.Source.EndDate = model.EndDateFromTimeSlot;
                    reservation.Source.Grade = model.Grade;
                    reservation.Source.Notes = model.Notes;

                    if (!ReservationHelper.IsValidTimeSlot(ReservationHelper.GetReservationsForDay(model.Date.Value),
                        (reservation.Source.StartDate.Value, reservation.Source.EndDate.Value)))
                    {
                        TempData["error"] = "Sorry but that time slot is no longer available.";
                    }
                    else if (context.SaveChanges() > 0)
                    {
                        var refreshedModel = GetReservation(model.Id);
                        Task.Run(() => Mailer.SendReservationConfirmation(ControllerContext, refreshedModel));
                        TempData["success"] = ViewHelpers.RenderToString(ControllerContext, "_Success",
                            refreshedModel);
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
