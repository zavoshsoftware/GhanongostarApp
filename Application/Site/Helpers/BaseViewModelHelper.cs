using System.Collections.Generic;
using System.Linq;
using Models;
using ViewModels;


namespace Helpers
{
    public class BaseViewModelHelper:Infrastructure.BaseControllerWithUnitOfWork
    {
        public List<ProductGroup> GetMenuProductGroups()
        {
            List<ProductGroup> productGroups = UnitOfWork.ProductGroupRepository.Get().ToList();

            return productGroups;
        }

        public List<SidebarProductGroupViewModel> GetSidebarProductGroups()
        {
            List<ProductGroup> productGroups = UnitOfWork.ProductGroupRepository.Get().ToList();

            List<SidebarProductGroupViewModel> list = new List<SidebarProductGroupViewModel>();

            foreach (var productGroup in productGroups)
            {
                list.Add(new SidebarProductGroupViewModel()
                {
                    ProductGroup = productGroup,
                    ProductCount = UnitOfWork.ProductRepository.Get(c =>
                        c.ProductGroupId == productGroup.Id && c.IsDeleted == false && c.IsActive).Count()
                });
            }

            return list;
        }
    }
}