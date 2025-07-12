using AlumniSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlumniSystem.Infrastructure.Data
{
	public class AlumniDbContext : IdentityDbContext<IdentityUser>
	{
		public AlumniDbContext(DbContextOptions<AlumniDbContext> potions)
			:base(potions) 
		{
		}

		public DbSet<Alumni> Alumnis { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<JobPosting> JobPostings { get; set; }
		public DbSet<Community> Communities { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Alumni>(entity =>
			{
				entity.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
				entity.Property(a => a.Surname).IsRequired().HasMaxLength(50);
				entity.Property(a => a.LastName).IsRequired().HasMaxLength(50);
				entity.Property(a => a.FacultyNumber).IsRequired().HasMaxLength(50);
				entity.Property(a => a.Email).IsRequired();
			});

			modelBuilder.Entity<Community>()
				.HasMany(c => c.Members)
				.WithMany(a => a.Communities);

			modelBuilder.Entity<Community>()
				.HasMany(c => c.Members)
				.WithMany(a => a.Communities)
				.UsingEntity(j => j.ToTable("CommunityMmembers"));
		}
	}
}