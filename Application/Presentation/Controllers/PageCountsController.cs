using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class PageCountsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            DateTime today = DateTime.Today.Date;

            PageCounterViewModel page = new PageCounterViewModel
            {
                PageCounts = GetPageCounter(today),
                Date = DateTime.Today
            };

            return View(page);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PageCounterViewModel page)
        {
            DateTime today = page.Date;
         
            page.PageCounts = GetPageCounter(today);

            return View(page);
        }

        public List<PageCountItem> GetPageCounter(DateTime date)
        {

            List<PageCount> pageCounts = db.PageCounts.Where(current =>
                current.IsDeleted == false &&
                DbFunctions.TruncateTime(current.VisitDate) == DbFunctions.TruncateTime(date)).OrderBy(current => current.CreationDate).ToList();

            List<PageCountItem> countItems = new List<PageCountItem>();

            foreach (PageCount pageCount in pageCounts)
            {
                if (pageCount.EntityId == null)
                {
                    countItems.Add(new PageCountItem()
                    {
                        Title = pageCount.Page.Title,
                        SubTitle = pageCount.Page.Title,
                        Count = pageCount.Count
                    });
                }
                else
                {
                    if (pageCount.Page.Name == "workshoplist" || pageCount.Page.Name == "eventslist" ||
                        pageCount.Page.Name == "productpackagelist" || pageCount.Page.Name == "formlist" ||
                        pageCount.Page.Name == "videogrouplist")
                    {
                        countItems.Add(new PageCountItem()
                        {
                            Title = pageCount.Page.Title,
                            SubTitle = pageCount.Page.Title,
                            Count = pageCount.Count
                        });
                    }
                    else if (pageCount.Page.Name == "workshopdetail" || pageCount.Page.Name == "eventdetail" ||
                             pageCount.Page.Name == "productpackagedetail" || pageCount.Page.Name == "newsdetail" ||
                             pageCount.Page.Name == "formdetaile" || pageCount.Page.Name == "videodetail")
                    {
                        if (pageCount.EntityId != null)
                        {
                            Product product = db.Products.FirstOrDefault(current => current.Id == pageCount.EntityId);

                            if (product != null)
                                countItems.Add(new PageCountItem()
                                {
                                    Title = product.Title,
                                    SubTitle = pageCount.Page.Title,
                                    Count = pageCount.Count
                                });
                        }
                    }
                    else if (pageCount.Page.Name == "videolistbygroup")
                    {
                        ProductGroup productGroup = db.ProductGroups.FirstOrDefault(current => current.Id == pageCount.EntityId);

                        if (productGroup != null)
                            countItems.Add(new PageCountItem()
                            {
                                Title = productGroup.Title,
                                SubTitle = pageCount.Page.Title,
                                Count = pageCount.Count
                            });
                    }
                }
            }
            return countItems;
        }
    }
}