using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
         System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ActivationCode> ActivationCodes { get; set; }
        public DbSet<ForgetPasswordRequest> ForgetPasswordRequests { get; set; }
        public DbSet<VersionHistory> VersionHistories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CourseDetail> CourseDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<SupportRequestType> SupportRequestTypes { get; set; }
        public DbSet<ZarinpallAuthority> ZarinpallAuthorities { get; set; }

        public DbSet<VipPackage> VipPackages { get; set; }
        public DbSet<UserVipPackage> UserVipPackages { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<VipPackageFeature> VipPackageFeatures { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<Models.QuestionConversation> QuestionConversations { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageCount> PageCounts { get; set; }
        public DbSet<SiteBlogCategory> SiteBlogCategories { get; set; }
        public DbSet<SiteBlog> SiteBlogs { get; set; }
        public DbSet<SiteBlogImage> SiteBlogImages { get; set; }
        public DbSet<Redirect> Redirects { get; set; }
        public DbSet<EmpClubProductGroup> EmpClubProductGroups { get; set; }
        public DbSet<EmpClubProduct> EmpClubProducts { get; set; }
        public DbSet<EmpClubQuestion> EmpClubQuestions { get; set; }
        public DbSet<ConsultantRequest> ConsultantRequests { get; set; }
        public DbSet<ConsultantRequestForm> ConxConsultantRequestForms { get; set; }
        public DbSet<EmpClubVideoGroup> EmpClubVideoGroups { get; set; }
    }
}
