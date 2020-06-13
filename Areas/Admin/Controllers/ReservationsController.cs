using jeanie.Lib;
using jeanie.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
                return View(new ReservationViewModel());
            }
        }

        [HttpPost]
        public ActionResult Read(DataTable dataTable)
        {
            using (var context = new JeanieContext())
            {
                var reservations = context.Reservations.AsQueryable();

                foreach (var order in dataTable.order)
                {
                    var colName = dataTable.columns[order.column].name;
                    var property = reservations.ElementType.GetProperty(colName);
                    var parameter = Expression.Parameter(reservations.ElementType, "r");
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    MethodCallExpression orderBy = Expression.Call(
                        typeof(Queryable),
                        order.dir == "asc" ? "OrderBy" : "OrderByDescending",
                        new Type[] { reservations.ElementType, property.PropertyType },
                        reservations.Expression,
                        orderByExp);

                    reservations = reservations.Provider.CreateQuery<Reservation>(orderBy);
                }

                reservations = reservations.Skip(dataTable.start).Take(dataTable.length);

                var results = reservations.ToList()
                    .Select(r => new ReservationViewModel(r)).ToList();

                return Json(new
                {
                    dataTable.draw,
                    recordsTotal = context.Reservations.Count(),
                    recordsFiltered = context.Reservations.Count(),
                    data = results.Select(r => new object[6]
                    {
                        r.Name,
                        r.Grade.Preview(20),
                        r.FormattedTimeSlot,
                        r.StatusText,
                        r.CreatedAt.ToShortDateString(),
                        ViewHelpers.RenderToString(ControllerContext, "_Actions", r)
                    })
                });
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

            return View("Index", model);
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
                        reservation.PhoneNumber = model.FormattedPhoneNumber;
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
    }
}
