using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class JobsController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> SendReminders()
        {
            await Mailer.SendReminder(ControllerContext, new ReservationViewModel
            {
                Email = "caleb@imap.cc"
            });
            return Content("ok");
        }
    }
}
