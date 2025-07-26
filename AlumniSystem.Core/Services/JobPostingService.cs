using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Core.Services
{
	public class JobPostingService : IJobPostingService
	{
		private readonly IJobPostingRepository jobRepository;

		public JobPostingService(IJobPostingRepository _jobRepository)
		{
			jobRepository = _jobRepository;
		}

		public async Task AddAsync(JobPostingViewModel model)
		{
			var jobPosting = new JobPosting
			{
				Title = model.Title,
				Company = model.Company,
				Description = model.Description,
				Location = model.Location,
				PostedOn = model.PostedOn
			};

			await jobRepository.AddAsync(jobPosting);
		}

		public async Task DeleteAsync(int id)
		{
			var job = await jobRepository.GetByIdAsync(id);

			if (job == null)
			{
				throw new ArgumentNullException("No Job Posting found!");
			}
			await jobRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<JobPostingViewModel>> GetAllAsync()
		{
			var jobPostings = await jobRepository.GetAllAsync();

			return jobPostings.Select(j => new JobPostingViewModel
			{
				Id = j.Id,
				Title = j.Title,
				Company = j.Company,
				Description = j.Description,
				Location = j.Location,
				PostedOn = j.PostedOn
			}).ToList();
		}

		public async Task<JobPostingViewModel> GetByIdAsync(int id)
		{
			var jobPosting = await jobRepository.GetByIdAsync(id);

			if (jobPosting == null)
			{
				return null;
			}

			return new JobPostingViewModel
			{
				Id = jobPosting.Id,
				Title = jobPosting.Title,
				Company = jobPosting.Company,
				Description = jobPosting.Description,
				Location = jobPosting.Location,
				PostedOn = jobPosting.PostedOn
			};
		}

		public async Task UpdateAsync(JobPostingViewModel model)
		{
			var jobPosting = new JobPosting
			{
				Id = model.Id,
				Title = model.Title,
				Company = model.Company,
				Description = model.Description,
				Location = model.Location,
				PostedOn = model.PostedOn
			};

			await jobRepository.UpdateAsync(jobPosting);
		}
	}
}