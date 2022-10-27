using Microsoft.EntityFrameworkCore;
using UserMicroservice.Models;

namespace UserMicroservice.DBContexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }
    }
}
