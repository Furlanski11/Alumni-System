using AlumniSystem.Core.ViewModels;

namespace AlumniSystem.Core.Interfaces
{
	public interface ICommunityService
	{
		Task<IEnumerable<CommunityViewModel>> GetAllAsync();

		Task<CommunityViewModel> GetByIdAsync(int id);

		Task AddAsync(CommunityViewModel model);

		Task UpdateAsync(CommunityViewModel model);

		Task DeleteAsync(int id);

		Task JoinAsync(int communityId, string alumniId);

		Task LeaveAsync(int communityId, string alumniId);
	}
}