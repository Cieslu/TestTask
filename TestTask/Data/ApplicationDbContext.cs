using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDo> Tasks { get; set; }//This is needed to create table called "Tasks" in the database

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(string connectionString, ServerVersion version) : base(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseMySql(connectionString, version)
        .Options)
        {
        }
    }
}
