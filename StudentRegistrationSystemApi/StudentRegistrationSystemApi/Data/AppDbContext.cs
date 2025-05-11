using Microsoft.EntityFrameworkCore;
using StudentRegistrationSystemApi.Model.Entity;

namespace StudentRegistrationSystemApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<StudentInformation> StudentInformations { get; set; }
    }
}
