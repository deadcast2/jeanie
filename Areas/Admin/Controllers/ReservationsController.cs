using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [HttpPost]
        public ActionResult Create(ReservationViewModel model)
        {
            if (model.IsValid)
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
                        TempData["success"] = "New reservation generated";
                        return RedirectToAction("Index");
                    }
                }
            }

            return View("Index", (model, GetReservations()));
        }

        private List<Reservation> GetReservations()
        {
            using (var context = new JeanieContext())
            {
                return context.Reservations.OrderByDescending(e => e.CreatedAt).ToList();
            }
        }
    }
}
