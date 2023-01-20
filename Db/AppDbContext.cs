using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Db.Entity;


// dotnet ef migrations add Initial -- "Server=localhost; Database = NNToApiDoDb; User Id=sa; Password=HardT0Gue$$Pa$$word; Trusted_Connection=True; Encrypt=False;"
// dotnet ef database update -- "Server=localhost; Database = NNToApiDoDb; User Id=sa; Password=HardT0Gue$$Pa$$word; Trusted_Connection=True;integrated security=False; Encrypt=False;"

namespace TodoApp.Api.Db
{
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        //public AppDbContext()
        //{
        //}

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // builder.Entity<UserEntity>().ToTable("Users");
            // builder.Entity<RoleEntity>().ToTable("Roles");
            // builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            // builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            // builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            // builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            // builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }

        //public DbSet<User> Users { get; set; }
        //public DbSet<ToDoEntity> ToDos { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //            "Server = localhost; " +
        //            "Database = ToDoNewDb; " +
        //            "User Id = sa ; " +
        //            "Password = HardT0Gue$$Pa$$word; " +
        //            "Encrypt=False"
        //        );
        //}
    }
}

