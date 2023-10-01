using Hero_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Hero_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}

        public DbSet<Hero> Heroes { get; set; }

    }
}
