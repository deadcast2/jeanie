using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var context = new JeanieContext())
            {
                return View((new ReservationViewModel(), GetReservations()));
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            using (var context = new JeanieContext())
            {
                var reservation = context.Reservations.Find(id);
                if (reservation != null)
                {
                    return View(new ReservationViewModel(reservation));
                }
                TempData["error"] = "Reservation not found.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ReservationViewModel model)
        {
            if (model.IsValid())
            {
                using (var context = new JeanieContext())
                {
                    context.Reservations.Add(new Reservation
                    {
                        Name = model.Name,
                        CreatedAt = DateTime.Now
                    });

                    if (context.SaveChanges() > 0)
                    {
                        TempData["success"] = "New reservation generated!";
                        return RedirectToAction("Index");
                    }
                }
            }

            return View("Index", (model, GetReservations()));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Update(ReservationViewModel model)
        {
            if (model.IsValid())
            {
                using (var context = new JeanieContext())
                {
                    var reservation = context.Reservations.Find(model.Id);
                    if (reservation != null)
                    {
                        reservation.Name = model.Name;
                        reservation.Email = model.Email;
                        reservation.Grade = model.Grade;
                        reservation.Notes = model.Notes;
                        reservation.StartDate = model.StartDate;
                        reservation.EndDate = model.EndDate;
                        reservation.UpdatedAt = DateTime.Now;

                        if (context.SaveChanges() > 0)
                        {
                            TempData["success"] = "Reservation successfully updated!";
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            return View("Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Destroy(Guid id)
        {
            using (var context = new JeanieContext())
            {
                var reservation = context.Reservations.Find(id);
                if (reservation != null)
                {
                    context.Entry(reservation).State = EntityState.Deleted;
                    if (context.SaveChanges() > 0)
                    {
                        TempData["success"] = "Reservation successfully deleted!";
                    }
                }

                return RedirectToAction("Index");
            }
        }

        private List<ReservationViewModel> GetReservations()
        {
            using (var context = new JeanieContext())
            {
                return context.Reservations.OrderByDescending(e => e.CreatedAt).ToList()
                    .Select(e => new ReservationViewModel(e)).ToList();
            }
        }
    }
}
