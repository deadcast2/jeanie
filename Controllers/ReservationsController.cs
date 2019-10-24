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
        public ActionResult Edit(Guid id)
        {
            return View();
        }
    }
}
