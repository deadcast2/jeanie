using jeanie.Lib;
using jeanie.Models;
using System.Linq;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        [HttpGet]
        public ActionResult Edit()
        {
            using (var context = new JeanieContext())
            {
                var setting = context.Settings.FirstOrDefault();
                if (setting != null)
                {
                    return View(new SettingViewModel(setting));
                }
                TempData["error"] = "Settings have not yet been configured.";
                return RedirectToAction("Index", "Reservations");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Update(SettingViewModel model)
        {
            if (model.IsValid())
            {
                using (var context = new JeanieContext())
                {
                    var trackedSetting = context.Settings.FirstOrDefault();
                    if (trackedSetting != null)
                    {
                        trackedSetting.DailyReservationLimit = model.DailyReservationLimit;
                        if (context.SaveChanges() > 0)
                        {
                            TempData["success"] = "Changes saved!";
                        }
                    }

                    return RedirectToAction("Index", "Reservations");
                }
            }

            return View("Edit", model);
        }
    }
}
