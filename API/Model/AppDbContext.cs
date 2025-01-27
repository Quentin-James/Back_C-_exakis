using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Collections.Generic;

namespace Model.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
    }
}