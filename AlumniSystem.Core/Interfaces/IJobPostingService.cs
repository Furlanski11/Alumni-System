using AlumniSystem.Core.ViewModels;

namespace AlumniSystem.Core.Interfaces
{
	public interface IJobPostingService
	{
		Task<IEnumerable<JobPostingViewModel>> GetAllAsync();

		Task<JobPostingViewModel> GetByIdAsync(int id);

		Task AddAsync(JobPostingViewModel model);

		Task UpdateAsync(JobPostingViewModel model);

		Task DeleteAsync(int id);
	}
}