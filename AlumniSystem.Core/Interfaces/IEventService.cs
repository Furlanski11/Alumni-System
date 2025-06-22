using AlumniSystem.Core.ViewModels;

namespace AlumniSystem.Core.Interfaces
{
	public interface IEventService
	{
		Task<IEnumerable<EventViewModel>> GetAllAsync();

		Task<EventViewModel> GetByIdAsync(int id);

		Task AddAsync(EventViewModel model);

		Task UpdateAsync(EventViewModel model);

		Task DeleteAsync(int id);
	}
}