using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Infrastructure.Interfaces
{
	public interface IAlumniRepository
	{
		Task<IEnumerable<Alumni>> GetAllAsync();

		Task<Alumni?> GetByIdAsync(int id);

		Task AddAsync(Alumni alumni);

		Task UpdateAsync(Alumni alumni);

		Task DeleteAsync(int id);
	}
}