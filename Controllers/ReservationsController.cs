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

        [HttpPost]
        public ActionResult Update(ReservationViewModel model)
        {
            var reservation = GetReservation(model.Id);

            if (reservation == null)
            {
                return Edit(model.Id);
            }

            TempData["success"] = "Your reservation has been successfully booked!";
            return Redirect("/");
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
