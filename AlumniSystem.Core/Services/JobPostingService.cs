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
				PostedOn = model.PostedOn,
				AlumniId = model.AlumniId
			};

			await jobRepository.AddAsync(jobPosting);
		}

		public async Task DeleteAsync(int id)
		{
			var job = jobRepository.GetByIdAsync(id);

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
				Title=j.Title,
				Company=j.Company,
				Description=j.Description,
				Location=j.Location,
				PostedOn=j.PostedOn,
				AlumniId=j.AlumniId
			}).ToList();
		}

		public async Task<JobPostingViewModel> GetByIdAsync(int id)
		{
			var jobPosting = await jobRepository.GetByIdAsync(id);

			return new JobPostingViewModel
			{
				Title = jobPosting.Title,
				Company = jobPosting.Company,
				Description = jobPosting.Description,
				Location = jobPosting.Location,
				PostedOn = jobPosting.PostedOn,
				AlumniId = jobPosting.AlumniId
			};
		}

		public async Task UpdateAsync(JobPostingViewModel model)
		{
			var jobPosting = new JobPosting
			{
				Title = model.Title,
				Company = model.Company,
				Description = model.Description,
				Location = model.Location,
				PostedOn = model.PostedOn,
				AlumniId = model.AlumniId
			};

			await jobRepository.UpdateAsync(jobPosting);
		}
	}
}