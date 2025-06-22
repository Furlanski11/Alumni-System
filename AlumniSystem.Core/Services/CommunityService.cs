using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Core.Services
{
	public class CommunityService : ICommunityService
	{
		private readonly ICommunityRepository communityRepository;

		public CommunityService(ICommunityRepository _communityRepository)
		{
			communityRepository = _communityRepository;
		}

		public async Task AddAsync(CommunityViewModel model)
		{
			var community = new Community
			{
				Name = model.Name,
				Description = model.Description
			};

			await communityRepository.AddAsync(community);
		}

		public async Task DeleteAsync(int id)
		{
			var community = communityRepository.GetByIdAsync(id);

			if (community == null)
			{
				throw new ArgumentNullException("No Community found!");
			}
			await communityRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<CommunityViewModel>> GetAllAsync()
		{
			var communities = await communityRepository.GetAllAsync();

			return communities.Select(c => new CommunityViewModel
			{
				Name = c.Name,
				Description = c.Description,
				Members = c.Members.Count()
			}).ToList();
		}

		public async Task<CommunityViewModel> GetByIdAsync(int id)
		{
			var community = await communityRepository.GetByIdAsync(id);

			return new CommunityViewModel
			{
				Name = community.Name,
				Description = community.Description,
				Members = community.Members.Count()
			};
		}

		public async Task UpdateAsync(CommunityViewModel model)
		{
			var community = new Community
			{
				Name = model.Name,
				Description = model.Description,
			};

			await communityRepository.UpdateAsync(community);
		}
	}
}