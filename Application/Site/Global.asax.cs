using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GSD.Globalization;
using Models;

namespace Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            url = HttpUtility.UrlDecode(url);

            DatabaseContext db = new DatabaseContext();

            List<Redirect> redirects = db.Redirects.Where(c => c.IsActive).ToList();

            foreach (Redirect redirect in redirects)
            {
                if(url==redirect.OldUrl)
                    Response.RedirectPermanent(redirect.NewUrl);
            }

            if(url== "/legal-contract-form/")
                Response.RedirectPermanent("/forms");
 else if(url== "/specialty-package/")
                Response.RedirectPermanent("/products");

            else if(url== "/درباره-ما/"||url== "contact")
                Response.Redirect("/");


            var persianCulture = new PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;

        }
    }
}
