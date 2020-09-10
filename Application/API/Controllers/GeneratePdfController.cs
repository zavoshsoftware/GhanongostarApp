using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace API.Controllers
{
    public class GeneratePdfController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        // GET: GeneratePdf
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrintDetails(Guid id)
        {
            OrderDetail detail = db.OrderDetails.Find(id);
            PdfGeneratorViewModel pdfGenerator = new PdfGeneratorViewModel();
            pdfGenerator.FullName = detail.Fullname;
            pdfGenerator.Title = detail.Product.Title;
            pdfGenerator.Type = detail.Product.ProductType.Title;

            return View(detail);
        }
        public ActionResult GeneratePDF(Guid id)
        {
            OrderDetail detail = db.OrderDetails.Find(id);
            Order order = db.Orders.Find(detail.OrderId);
            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("/Uploads/Order/"), Path.GetFileName(id + ".pdf"));

            var pdf = new Rotativa.ActionAsPdf("PrintDetails", new { id = id }) { FileName = order.Code + ".pdf", SaveOnServerPath = path };
            detail.OrderFile = "/Uploads/Order/" + id + ".pdf";
            db.SaveChanges();
            return pdf;
        }
    }
}