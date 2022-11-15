using Microsoft.EntityFrameworkCore;
using JobsAPI.Models;

namespace JobsAPI.DBControl
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}
