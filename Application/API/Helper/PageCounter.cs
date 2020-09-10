using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Infrastructure;
using Models;

namespace Helper
{
    public class PageCounter : Infrastructure.BaseControllerWithUnitOfWork
    {
        public void Count(string pageName, Guid? entityId)
        {
            DateTime today = DateTime.Today.Date;

            Page page = UnitOfWork.PageRepository.Get(current => current.Name == pageName).FirstOrDefault();

            PageCount pageCount = UnitOfWork.PageCountRepository
                .Get(current => current.PageId == page.Id && current.EntityId == entityId && DbFunctions.TruncateTime(current.VisitDate) == DbFunctions.TruncateTime(today)).FirstOrDefault();

            if (pageCount == null)
            {
                PageCount entity = new PageCount()
                {
                    PageId = page.Id,
                    Count = 1,
                    VisitDate = DateTime.Today.Date,
                    EntityId = entityId
                };

                UnitOfWork.PageCountRepository.Insert(entity);
            }

            else
            {
                pageCount.Count++;
                UnitOfWork.PageCountRepository.Update(pageCount);
            }
            UnitOfWork.Save();

        }
    }
}