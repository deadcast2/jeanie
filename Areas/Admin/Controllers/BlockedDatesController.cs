using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Controllers
{
    [Authorize]
    public class BlockedDatesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dates()
        {
            using (var context = new JeanieContext())
            {
                return Json(context.BlockedDates.ToList().Select(e => e.Date.ToShortDateString()),
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Update(DateTime date)
        {
            using (var context = new JeanieContext())
            {
                var blockedDate = context.BlockedDates.FirstOrDefault(e => e.Date == date);
                if (blockedDate != null)
                {
                    context.Entry(blockedDate).State = EntityState.Deleted;
                }
                else
                {
                    context.BlockedDates.Add(new BlockedDate { Date = date });
                }
                context.SaveChanges();

                return Json(new { enabled = blockedDate == null });
            }
        }
    }
}
