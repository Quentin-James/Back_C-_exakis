using Microsoft.EntityFrameworkCore;

namespace EXERCICE_INTEGRATION.DAL
{
    public class AppDbContext : DbContext
    {
        // Constructor to initialize the DbContext with the options
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        // Define a DbSet for the Student model
        public DbSet<Student> Students { get; set; }
    }
    // Define the Student model
}
