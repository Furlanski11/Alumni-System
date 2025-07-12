using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using AlumniSystem.Infrastructure.Models.Enums;

namespace AlumniSystem.Core.Services
{
	public class CommunityService : ICommunityService
	{
		private readonly ICommunityRepository communityRepository;
		private readonly IAlumniRepository alumnirepository;

		public CommunityService(ICommunityRepository _communityRepository, IAlumniRepository _alumniRepository)
		{
			communityRepository = _communityRepository;
			alumnirepository = _alumniRepository;
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
				Id = c.Id,
				Name = c.Name,
				Description = c.Description,
				Members = c.Members?
				.Select(a => new AlumniViewModel
				{
					Id = a.Id,
					FirstName = a.FirstName,
					Surname = a.Surname,
					LastName = a.LastName,
					FacultyNumber = a.FacultyNumber,
					Faculty = (int)a.Faculty,
					Degree = a.Degree,
					Email = a.Email,
					GraduationYear = a.GraduationYear,
					CurrentPosition = a.CurrentPosition
				}).ToList() ?? new List<AlumniViewModel>()
			});
		}

		public async Task<CommunityViewModel> GetByIdAsync(int id)
		{
			var community = await communityRepository.GetByIdAsync(id);
			if(community == null)
			{
				throw new ArgumentNullException("Community not found!");
			}

			var viewModel = new CommunityViewModel
			{
				Id = community.Id,
				Name = community.Name,
				Description = community.Description,
				Members = community.Members?
				.Select(a => new AlumniViewModel
				{
					Id = a.Id,
					FirstName = a.FirstName,
					Surname = a.Surname,
					LastName = a.LastName,
					FacultyNumber = a.FacultyNumber,
					Faculty = (int)a.Faculty,
					Degree = a.Degree,
					Email = a.Email,
					GraduationYear = a.GraduationYear,
					CurrentPosition = a.CurrentPosition
				}).ToList() ?? new List<AlumniViewModel>()
			};

			return viewModel;
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

		public async Task JoinAsync(int communityId, int userId)
		{
			var alumni = await alumnirepository.GetByIdAsync(userId);
			if (alumni == null)
			{
				throw new ArgumentNullException("Alumni not found!");
			}

			await communityRepository.AddMemberAsync(communityId, alumni.Id);
		}

		public async Task LeaveAsync(int communityId, int userId)
		{
			var alumni = await alumnirepository.GetByIdAsync(userId);
			if (alumni == null)
			{
				throw new ArgumentNullException("Alumni not found!");
			}
			await communityRepository.RemoveMemberAsync(communityId, alumni.Id);
		}
	}
}