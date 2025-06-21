using AlumniSystem.Infrastructure.Data;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AlumniSystem.Infrastructure.Repositories
{
	public class JobPostingRepository : IJobPostingRepository
	{
		private readonly AlumniDbContext context;

		public JobPostingRepository(AlumniDbContext _context)
		{
			context = _context;
		}

		public async Task AddAsync(JobPosting job)
		{
			await context.JobPostings.AddAsync(job);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var job = await context.JobPostings.FindAsync(id);

			if (job == null)
			{
				throw new Exception("Job not found!");
			}

			context.JobPostings.Remove(job);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<JobPosting>> GetAllAsync()
		{
			return await context.JobPostings
				.Include(j => j.Alumni)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<JobPosting> GetByIdAsync(int id)
		{
			var job = await context.JobPostings.FindAsync(id);

			if (job == null)
			{
				throw new Exception("Job not found!");
			}

			return job;
		}

		public async Task UpdateAsync(JobPosting job)
		{
			context.JobPostings.Update(job);
			await context.SaveChangesAsync();
		}
	}
}