using System.IO;
using System.Web.Mvc;

namespace jeanie.Lib
{
    public static class ViewHelpers
    {
        public static string RenderToString(ControllerContext context, string viewName, object model)
        {
            context.Controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData,
                    context.Controller.TempData, writer);

                viewResult.View.Render(viewContext, writer);
                viewResult.ViewEngine.ReleaseView(context, viewResult.View);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}