using jeanie.Lib;
using jeanie.Models;
using System;
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
            else if (reservation.IsComplete)
            {
                TempData["error"] = "Sorry but the reservation has already been completed.";
                return Redirect("/");
            }
            else if (reservation.IsConfirmed)
            {
                TempData["error"] = "Sorry but the reservation has already been confirmed.";
                return Redirect("/");
            }
            else if (reservation.IsCancelled)
            {
                TempData["error"] = "Sorry but the reservation has already been cancelled.";
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
                if (reservation == null || reservation.IsComplete)
                    return Edit(model.Id);

                // So name validation succeeds.
                model.Name = reservation.Name;

                if (model.IsValid(strict: true))
                {
                    context.Reservations.Attach(reservation.Source);

                    reservation.Source.StartDate = model.StartDateFromTimeSlot;
                    reservation.Source.EndDate = model.EndDateFromTimeSlot;
                    reservation.Source.Email = model.Email;
                    reservation.Source.Grade = model.Grade;
                    reservation.Source.Notes = model.Notes;
                    reservation.Source.Status = ReservationStatus.Complete;

                    if (!ReservationHelper.IsValidTimeSlot(ReservationHelper.GetReservationsForDay(model.Date.Value),
                        (reservation.Source.StartDate.Value, reservation.Source.EndDate.Value)))
                    {
                        TempData["error"] = "Sorry but that time slot is no longer available.";
                    }
                    else if (context.SaveChanges() > 0)
                    {
                        var refreshedModel = GetReservation(model.Id);
                        Mailer.SendCompleteAlert(ControllerContext, refreshedModel);
                        TempData["success"] = ViewHelpers.RenderToString(ControllerContext, "_Success",
                            refreshedModel);
                        return Redirect("/");
                    }
                }
            }

            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Confirm(Guid id)
        {
            return UpdateStatus(id, ReservationStatus.Confirmed);
        }

        [HttpGet]
        public ActionResult Cancel(Guid id)
        {
            return UpdateStatus(id, ReservationStatus.Cancelled);
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

        private ActionResult UpdateStatus(Guid id, ReservationStatus status)
        {
            using (var context = new JeanieContext())
            {
                var reservation = GetReservation(id);
                if (reservation == null || reservation.IsConfirmed || reservation.IsCancelled)
                    return Edit(id);

                context.Reservations.Attach(reservation.Source);
                reservation.Source.Status = status;

                if (context.SaveChanges() > 0)
                {
                    if (status == ReservationStatus.Confirmed)
                    {
                        Mailer.SendConfirmationAlert(ControllerContext, reservation);
                        TempData["success"] = "Your reservation has been successfully confirmed!";
                    }
                    else
                    {
                        Mailer.SendCancellationAlert(ControllerContext, reservation);
                        TempData["success"] = "Your reservation has been successfully cancelled!";
                    }
                }

                return Redirect("/");
            }
        }
    }
}
