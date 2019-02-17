using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grundloven.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(params string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-grundloven-4;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new ApplicationDbContext(builder.Options);
        }
    }
}
