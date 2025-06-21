using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Infrastructure.Interfaces
{
	public interface IEventRepository
	{
		Task<IEnumerable<Event>> GetAllAsync();

		Task<Event> GetByIdAsync(int id);

		Task AddAsync(Event e);

		Task UpdateAsync(Event e);

		Task DeleteAsync(int id);
	}
}