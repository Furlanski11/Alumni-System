using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Infrastructure.Interfaces
{
	public interface IJobPostingRepository
	{
		Task<IEnumerable<JobPosting>> GetAllAsync();

		Task<JobPosting> GetByIdAsync(int id);

		Task AddAsync(JobPosting job);

		Task UpdateAsync(JobPosting job);

		Task DeleteAsync(int id);
	}
}