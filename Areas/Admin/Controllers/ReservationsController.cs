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
        public ActionResult Index()
        {
            return View();
        }
    }
}