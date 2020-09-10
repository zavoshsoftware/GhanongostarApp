using System.Data;
using System.Linq;
using System.Threading;
using System.Web.Http.Controllers;
using Helper;

namespace Infrastructure
{
    public class BaseApiController : System.Web.Http.ApiController
    {
        public BaseApiController()
        {
        }
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //System.Globalization.CultureInfo oCultureInfo =
            //    new System.Globalization.CultureInfo("fa-IR");
            //System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
            var persianCulture = new PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;
            base.Initialize(controllerContext);
        }
    }
}