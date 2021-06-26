using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork : System.IDisposable
    {
        IUserRepository UserRepository { get; }
        IActivationCodeRepository ActivationCodeRepository { get; }
        IRoleRepository RoleRepository { get; }
        IForgetPasswordRequestRepository ForgetPasswordRequestRepository { get; }
        IVersionHistoryRepository VersionHistoryRepository { get; }
        ICityRepository CityRepository { get; }
        ICourseDetailRepository CourseDetailRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IQuestionConversationRepository QuestionConversationRepository { get; }
        IBlogCategoryRepository BlogCategoryRepository { get; }
        IBlogRepository BlogRepository { get; }
        ISupportRequestTypeRepository SupportRequestTypeRepository { get; }
        ISupportRequestRepository SupportRequestRepository { get; }
        IZarinpallAuthorityRepository ZarinpallAuthorityRepository { get; }
        IVipPackageRepository VipPackageRepository { get; }
        IUserVipPackageRepository UserVipPackageRepository { get; }
        IVipPackageFeatureRepository VipPackageFeatureRepository { get; }
        ITextRepository TextRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IDiscountCodeRepository DiscountCodeRepository { get; }
        IOrderDiscountRepository OrderDiscountRepository { get; }
        IProductGroupRepository ProductGroupRepository { get; }
        IProductDiscountRepository ProductDiscountRepository { get; }
        IPageRepository PageRepository { get; }
        IPageCountRepository PageCountRepository { get; }
        ISiteBlogRepository SiteBlogRepository { get; }
        ISiteBlogImageRepository SiteBlogImageRepository { get; }
        ISiteBlogCategoryRepository SiteBlogCategoryRepository { get; }
        IEmpClubProductGroupRepository EmpClubProductGroupRepository { get; }
        IEmpClubProductRepository EmpClubProductRepository { get; }
        IEmpClubQuestionRepository EmpClubQuestionRepository { get; }
        IConsultantRequestRepository ConsultantRequestRepository { get; }
        IConsultantRequestFormRepository ConsultantRequestFormRepository { get; }
        IEmpClubVideoGroupRepository EmpClubVideoGroupRepository { get; }
        IFormInstagramLiveRepository FormInstagramLiveRepository { get; }
        ISeminarRepository SeminarRepository { get; }
        ISeminarTeacherRepository SeminarTeacherRepository { get; }
        ISeminarImageRepository SeminarImageRepository { get; }


        void Save();
    }
}
