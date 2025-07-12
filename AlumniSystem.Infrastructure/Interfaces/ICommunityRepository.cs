using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Infrastructure.Interfaces
{
	public interface ICommunityRepository
	{
		Task<IEnumerable<Community>> GetAllAsync();

		Task<Community> GetByIdAsync(int id);

		Task AddAsync(Community community);

		Task UpdateAsync(Community community);

		Task DeleteAsync(int id);

		Task AddMemberAsync(int communityId, int alumniId);

		Task RemoveMemberAsync(int communityId, int alumniId);
	}
}