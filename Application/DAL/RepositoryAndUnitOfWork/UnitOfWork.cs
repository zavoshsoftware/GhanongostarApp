using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class UnitOfWork : System.Object, IUnitOfWork
    {
        public UnitOfWork()
        {
            IsDisposed = false;
        }
        protected bool IsDisposed { get; set; }

        private Models.DatabaseContext _databaseContext;
        protected virtual Models.DatabaseContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext =
                        new Models.DatabaseContext();
                }
                return (_databaseContext);
            }
        }
        public void Save()
        {
            try
            {
                DatabaseContext.SaveChanges();
            }
            //catch (System.Exception ex)
            catch
            {
                //Hmx.LogHandler.Report(GetType(), null, ex);
                throw;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _databaseContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Inserting custom Respositories
        //private IRepository _Repository;
        //public IRepository Repository
        //{
        //	get
        //	{
        //		if (_Repository == null)
        //		{
        //			_Repository =
        //				new Repository(DatabaseContext);
        //		}
        //		return (_Repository);
        //	}
        //}

        private IVersionHistoryRepository _versionHistoryRepository;
        public IVersionHistoryRepository VersionHistoryRepository
        {
            get
            {
                if (_versionHistoryRepository == null)
                {
                    _versionHistoryRepository =
                        new VersionHistoryRepository(DatabaseContext);
                }
                return (_versionHistoryRepository);
            }
        }

        private IVipPackageRepository _vipPackageRepository;
        public IVipPackageRepository VipPackageRepository
        {
            get
            {
                if (_vipPackageRepository == null)
                {
                    _vipPackageRepository =
                        new VipPackageRepository(DatabaseContext);
                }
                return (_vipPackageRepository);
            }
        }

        private IUserVipPackageRepository _userVipPackageRepository;
        public IUserVipPackageRepository UserVipPackageRepository
        {
            get
            {
                if (_userVipPackageRepository == null)
                {
                    _userVipPackageRepository =
                        new UserVipPackageRepository(DatabaseContext);
                }
                return (_userVipPackageRepository);
            }
        }

        private ITextRepository _textRepository;
        public ITextRepository TextRepository
        {
            get
            {
                if (_textRepository == null)
                {
                    _textRepository =
                        new TextRepository(DatabaseContext);
                }
                return (_textRepository);
            }
        }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository =
                        new UserRepository(DatabaseContext);
                }
                return (_userRepository);
            }
        }

        private IActivationCodeRepository _activationCodeRepository;
        public IActivationCodeRepository ActivationCodeRepository
        {
            get
            {
                if (_activationCodeRepository == null)
                {
                    _activationCodeRepository =
                        new ActivationCodeRepository(DatabaseContext);
                }
                return (_activationCodeRepository);
            }
        }
        private ISupportRequestTypeRepository _supportRequestTypeRepository;
        public ISupportRequestTypeRepository SupportRequestTypeRepository
        {
            get
            {
                if (_supportRequestTypeRepository == null)
                {
                    _supportRequestTypeRepository =
                        new SupportRequestTypeRepository(DatabaseContext);
                }
                return (_supportRequestTypeRepository);
            }
        }

        private ISupportRequestRepository _supportRequestRepository;
        public ISupportRequestRepository SupportRequestRepository
        {
            get
            {
                if (_supportRequestRepository == null)
                {
                    _supportRequestRepository =
                        new SupportRequestRepository(DatabaseContext);
                }
                return (_supportRequestRepository);
            }
        }

        //private ICarBrandRepository _carBrandRepository;
        //public ICarBrandRepository CarBrandRepository
        //{
        //    get
        //    {
        //        if (_carBrandRepository == null)
        //        {
        //            _carBrandRepository =
        //                new CarBrandRepository(DatabaseContext);
        //        }
        //        return (_carBrandRepository);
        //    }
        //}
        //private ICarModelRepository _carModelRepository;
        //public ICarModelRepository CarModelRepository
        //{
        //    get
        //    {
        //        if (_carModelRepository == null)
        //        {
        //            _carModelRepository =
        //                new CarModelRepository(DatabaseContext);
        //        }
        //        return (_carModelRepository);
        //    }
        //}

        //private ICarModelTypeRepository _carModelTypeRepository;
        //public ICarModelTypeRepository CarModelTypeRepository
        //{
        //    get
        //    {
        //        if (_carModelTypeRepository == null)
        //        {
        //            _carModelTypeRepository =
        //                new CarModelTypeRepository(DatabaseContext);
        //        }
        //        return (_carModelTypeRepository);
        //    }
        //}


        private IRoleRepository _roleRepository;
        public IRoleRepository  RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository =
                        new RoleRepository(DatabaseContext);
                }
                return (_roleRepository);
            }
        }


    
        private IForgetPasswordRequestRepository _forgetPasswordRequestRepository;
        public IForgetPasswordRequestRepository ForgetPasswordRequestRepository
        {
            get
            {
                if (_forgetPasswordRequestRepository == null)
                {
                    _forgetPasswordRequestRepository =
                        new ForgetPasswordRequestRepository(DatabaseContext);
                }
                return (_forgetPasswordRequestRepository);
            }
        }


        private ICityRepository _cityRepository;
        public ICityRepository CityRepository
        {
            get
            {
                if (_cityRepository == null)
                {
                    _cityRepository =
                        new CityRepository(DatabaseContext);
                }
                return (_cityRepository);
            }
        }

        private ICourseDetailRepository _courseDetailRepository;
        public ICourseDetailRepository CourseDetailRepository
        {
            get
            {
                if (_courseDetailRepository == null)
                {
                    _courseDetailRepository =
                        new CourseDetailRepository(DatabaseContext);
                }
                return (_courseDetailRepository);
            }
        }


        private IOrderRepository _orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository =
                        new OrderRepository(DatabaseContext);
                }
                return (_orderRepository);
            }
        }



        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository =
                        new ProductRepository(DatabaseContext);
                }
                return (_productRepository);
            }
        }

        private IProductTypeRepository _productTypeRepository;
        public IProductTypeRepository ProductTypeRepository
        {
            get
            {
                if (_productTypeRepository == null)
                {
                    _productTypeRepository =
                        new ProductTypeRepository(DatabaseContext);
                }
                return (_productTypeRepository);
            }
        }
        private IProvinceRepository _provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (_provinceRepository == null)
                {
                    _provinceRepository =
                        new ProvinceRepository(DatabaseContext);
                }
                return (_provinceRepository);
            }
        }
        private IQuestionConversationRepository _questionConversationRepository;
        public IQuestionConversationRepository QuestionConversationRepository
        {
            get
            {
                if (_questionConversationRepository == null)
                {
                    _questionConversationRepository =
                        new QuestionConversationRepository(DatabaseContext);
                }
                return (_questionConversationRepository);
            }
        }


        private IBlogCategoryRepository _blogCategoryRepository;
        public IBlogCategoryRepository BlogCategoryRepository
        {
            get
            {
                if (_blogCategoryRepository == null)
                {
                    _blogCategoryRepository =
                        new BlogCategoryRepository(DatabaseContext);
                }
                return (_blogCategoryRepository);
            }
        }


        private IBlogRepository _blogRepository;
        public IBlogRepository BlogRepository
        {
            get
            {
                if (_blogRepository == null)
                {
                    _blogRepository =
                        new BlogRepository(DatabaseContext);
                }
                return (_blogRepository);
            }
        }


        private IZarinpallAuthorityRepository _zarinpallAuthorityRepository;
        public IZarinpallAuthorityRepository ZarinpallAuthorityRepository
        {
            get
            {
                if (_zarinpallAuthorityRepository == null)
                {
                    _zarinpallAuthorityRepository =
                        new ZarinpallAuthorityRepository(DatabaseContext);
                }
                return (_zarinpallAuthorityRepository);
            }
        }

        private IVipPackageFeatureRepository _vipPackageFeatureRepository;
        public IVipPackageFeatureRepository VipPackageFeatureRepository
        {
            get
            {
                if (_vipPackageFeatureRepository == null)
                {
                    _vipPackageFeatureRepository =
                        new VipPackageFeatureRepository(DatabaseContext);
                }
                return (_vipPackageFeatureRepository);
            }
        }

        private IOrderDetailRepository _orderDetailRepository;
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository =
                        new OrderDetailRepository(DatabaseContext);
                }
                return (_orderDetailRepository);
            }
        }

        private IDiscountCodeRepository _discountCodeRepository;
        public IDiscountCodeRepository DiscountCodeRepository
        {
            get
            {
                if (_discountCodeRepository == null)
                {
                    _discountCodeRepository =
                        new DiscountCodeRepository(DatabaseContext);
                }
                return (_discountCodeRepository);
            }
        }

        private IOrderDiscountRepository _orderDiscountRepository;
        public IOrderDiscountRepository OrderDiscountRepository
        {
            get
            {
                if (_orderDiscountRepository == null)
                {
                    _orderDiscountRepository =
                        new OrderDiscountRepository(DatabaseContext);
                }
                return (_orderDiscountRepository);
            }
        }

        private IProductGroupRepository _productGroupRepository;
        public IProductGroupRepository ProductGroupRepository
        {
            get
            {
                if (_productGroupRepository == null)
                {
                    _productGroupRepository =
                        new ProductGroupRepository(DatabaseContext);
                }
                return (_productGroupRepository);
            }
        }


        private IProductDiscountRepository _productDiscountRepository;
        public IProductDiscountRepository ProductDiscountRepository
        {
            get
            {
                if (_productDiscountRepository == null)
                {
                    _productDiscountRepository =
                        new ProductDiscountRepository(DatabaseContext);
                }
                return (_productDiscountRepository);
            }
        }

        private IPageRepository _pageRepository;
        public IPageRepository PageRepository
        {
            get
            {
                if (_pageRepository == null)
                {
                    _pageRepository =
                        new PageRepository(DatabaseContext);
                }
                return (_pageRepository);
            }
        }

        private IPageCountRepository _pageCountRepository;
        public IPageCountRepository PageCountRepository
        {
            get
            {
                if (_pageCountRepository == null)
                {
                    _pageCountRepository =
                        new PageCountRepository(DatabaseContext);
                }
                return (_pageCountRepository);
            }
        }
        private ISiteBlogRepository _siteBlogRepository;
        public ISiteBlogRepository SiteBlogRepository
        {
            get
            {
                if (_siteBlogRepository == null)
                {
                    _siteBlogRepository =
                        new SiteBlogRepository(DatabaseContext);
                }
                return (_siteBlogRepository);
            }
        }
        private ISiteBlogImageRepository _siteBlogImageRepository;
        public ISiteBlogImageRepository SiteBlogImageRepository
        {
            get
            {
                if (_siteBlogImageRepository == null)
                {
                    _siteBlogImageRepository =
                        new SiteBlogImageRepository(DatabaseContext);
                }
                return (_siteBlogImageRepository);
            }
        }
        private ISiteBlogCategoryRepository _siteBlogCategoryRepository;
        public ISiteBlogCategoryRepository SiteBlogCategoryRepository
        {
            get
            {
                if (_siteBlogCategoryRepository == null)
                {
                    _siteBlogCategoryRepository =
                        new SiteBlogCategoryRepository(DatabaseContext);
                }
                return (_siteBlogCategoryRepository);
            }
        }
        #endregion Inserting custom Respositories

    }
}
