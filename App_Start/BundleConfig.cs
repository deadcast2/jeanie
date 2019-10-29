using System.Web.Optimization;

namespace jeanie
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jsrender.min.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/clipboard/clipboard.min.js")
                .Include("~/Scripts/pickadate/picker.js")
                .Include("~/Scripts/pickadate/picker.date.js")
                .Include("~/Scripts/pickadate/picker.time.js")
                .Include("~/Scripts/fullcalendar/core/main.min.js")
                .Include("~/Scripts/fullcalendar/daygrid/main.min.js")
                .Include("~/Scripts/fullcalendar/interaction/main.min.js")
                .Include("~/Scripts/app/app.js")
                .Include("~/Scripts/app/blockeddate.js")
                .Include("~/Scripts/app/reservation.js")
            );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/pickadate/default.css",
                      "~/Content/pickadate/default.date.css",
                      "~/Content/pickadate/default.time.css",
                      "~/Content/fullcalendar/core/main.min.css",
                      "~/Content/fullcalendar/daygrid/main.min.css"));

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
