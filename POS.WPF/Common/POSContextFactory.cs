using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using POS.DAL.Domain;

namespace POS.WPF.Common
{
    public class POSContextFactory : IDesignTimeDbContextFactory<POSContext>
    {
        public POSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();
            optionsBuilder.UseSqlite(@"Data Source=C:\MyCode\POS.db");

            return new POSContext(optionsBuilder.Options);
        }
    }
}
