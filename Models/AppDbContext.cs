using Microsoft.EntityFrameworkCore;

namespace Job_Candidate_API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Candidate> Candidates { get; set; } = null!;
    }
}
