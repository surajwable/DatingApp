using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        //this is a Constructor needed by DbContext to initialise a connection string inside a property DbContextOptions
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //each DbSet means a Table in a database, here Users will be a Table using AppUser entity
        public DbSet<AppUser> Users { get; set; }
    }
}