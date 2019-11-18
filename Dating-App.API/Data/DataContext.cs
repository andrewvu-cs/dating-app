using Dating_App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating_App.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        // Values represents the table name when we scaffold our db
        public DbSet<Value> Values { get; set; }
    }
}