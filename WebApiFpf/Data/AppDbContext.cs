using Microsoft.EntityFrameworkCore;
using WebApiFpf.Models.Entities;


namespace WebApiFpf.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<CandidateModel> Candidates { get; set; }
        public DbSet<SkillModel> Skills { get; set; }
        public DbSet<CandidateSkillModel> CandidateSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidateModel>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<CandidateSkillModel>()
                .HasKey(cs => new { cs.CandidateId, cs.SkillId });

            
            modelBuilder.Entity<CandidateSkillModel>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSkills)
                .HasForeignKey(cs => cs.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<CandidateSkillModel>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.CandidateSkills)
                .HasForeignKey(cs => cs.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CandidateSkillModel>()
                .Property(cs => cs.Level)
                .HasConversion<string>()
                .HasColumnType("nvarchar(50)");

        }
    }
}
