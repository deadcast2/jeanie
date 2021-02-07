using jeanie.Areas.Admin.Models;
using jeanie.Lib;
using System;
using System.Linq;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        [HttpGet]
        public ActionResult Show(Guid id)
        {
            using (var context = new JeanieContext())
            {
                var setting = context.Settings.FirstOrDefault();
                var reservation = context.Reservations.Find(id);

                return Json(new EmailViewModel(setting, reservation), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(EmailViewModel model)
        {
            Mailer.SendReservation(model);

            TempData["success"] = $"Reservation successfully emailed to {model.To}!";

            return RedirectToAction("Index", "Reservations");
        }
    }
}