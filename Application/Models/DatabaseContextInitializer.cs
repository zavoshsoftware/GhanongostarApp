using System;

namespace Models
{
    internal static class DatabaseContextInitializer
    {
        static DatabaseContextInitializer()
        {

        }

        internal static void Seed(DatabaseContext databaseContext)
        {
            //InitialRoles(databaseContext);
            InitialProductType(databaseContext);
        }

        #region Role
        public static void InitialRoles(DatabaseContext databaseContext)
        {
            InsertRole("f1dcedb2-a865-4c73-bc51-1afd28118d39", "SuperAdministrator", "راهبر ویژه", databaseContext);
            InsertRole("f53d469b-4172-42a9-8355-20032367c627", "Administrator", "راهبر", databaseContext);
            InsertRole("b999eb27-7330-4062-b81f-62b3d1935885", "Employer", "کارفرما", databaseContext);
            InsertRole("6d352c2f-6e64-4762-aae4-00f49979d7f1", "Employee", "کارمند", databaseContext);
        }

        public static void InsertRole(string roleId, string roleName, string roleTitle, DatabaseContext databaseContext)
        {
            Guid id = new Guid(roleId);
            Role role = new Role();
            role.Id = id;
            role.Title = roleTitle;
            role.Name = roleName;
            role.CreationDate = DateTime.Now;
            role.IsActive = true;
            role.IsDeleted = false;

            databaseContext.Roles.Add(role);
            databaseContext.SaveChanges();
        }
        #endregion

        #region ProductType
        public static void InitialProductType(DatabaseContext databaseContext)
        {
            InsertProductType("latest", "تازه ها", databaseContext);
            InsertProductType("question", "پرسش و پاسخ", databaseContext);
            InsertProductType("forms", "فرم ها و قراردادها", databaseContext);
            InsertProductType("course", "دوره های آموزشی آنلاین", databaseContext);
            InsertProductType("physicalproduct", "سفارش محصولات آموزشی", databaseContext);
            InsertProductType("workshop", "کارگاه ها", databaseContext);
            InsertProductType("event", "رویداد ها", databaseContext);
        }

        public static void InsertProductType(string name, string title, DatabaseContext databaseContext)
        {
            ProductType pt = new ProductType()
            {
                Name = name,
                Title = title,
                CreationDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                Id = Guid.NewGuid()
            };

            databaseContext.ProductTypes.Add(pt);
            databaseContext.SaveChanges();
        }
        #endregion


    }
}
