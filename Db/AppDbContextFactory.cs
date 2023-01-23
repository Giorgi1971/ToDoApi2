using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApp.Api.Db;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(args[0]);

        return new AppDbContext(optionsBuilder.Options);
    }
}
// dotnet ef migrations add Initial -- "Server=localhost; Database = NNToApiDoDb; User Id=sa; Password=HardT0Gue\$\$Pa\$\$word; Trusted_Connection=True; Encrypt=False;"
// dotnet ef database update -- "Server=localhost; Database = NNToApiDoDb; User Id=sa; Password=HardT0Gue\$\$Pa\$\$word; Trusted_Connection=True;integrated security=False; Encrypt=False;"