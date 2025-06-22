using AlumniSystem.Core.ViewModels;

namespace AlumniSystem.Core.Interfaces
{
	public interface IAlumniService
	{
		Task<IEnumerable<AlumniViewModel>> GetAllAsync();

		Task<AlumniViewModel> GetByIdAsync(int id);

		Task AddAsync(AlumniViewModel model);

		Task UpdateAsync(AlumniViewModel model);

		Task DeleteAsync(int id);
	}
}