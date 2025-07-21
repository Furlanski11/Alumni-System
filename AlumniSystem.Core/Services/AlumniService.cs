using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.ViewModels;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using AlumniSystem.Infrastructure.Models.Enums;

namespace AlumniSystem.Core.Services
{
	public class AlumniService : IAlumniService
	{
		private readonly IAlumniRepository repository;

		public AlumniService(IAlumniRepository _repository)
		{
			repository = _repository;
		}

		public async Task AddAsync(AlumniViewModel model)
		{
			var alumni = new Alumni
			{
				UserId = model.UserId,
				FacultyNumber = model.FacultyNumber,
				FirstName = model.FirstName,
				Surname = model.Surname,
				LastName = model.LastName,
				Email = model.Email,
				Faculty = (Faculty)model.Faculty,
				Degree = model.Degree,
				GraduationYear = model.GraduationYear,
				CurrentPosition = model.CurrentPosition
			};

			await repository.AddAsync(alumni);
		}

		public async Task DeleteAsync(string id)
		{
			var alumni = repository.GetByIdAsync(id);

			if (alumni == null) 
			{
				throw new ArgumentNullException("No alumni found!");
			}
			await repository.DeleteAsync(id);
		}

		public async Task<IEnumerable<AlumniViewModel>> GetAllAsync()
		{
			var alumnis = await repository.GetAllAsync();

			return alumnis.Select(a => new AlumniViewModel
			{
				Id = a.Id,
				UserId = a.UserId,
				FacultyNumber = a.FacultyNumber,
				FirstName = a.FirstName,
				Surname = a.Surname,
				LastName = a.LastName,
				Email= a.Email,
				Faculty = (int)a.Faculty,
				Degree = a.Degree,
				GraduationYear = a.GraduationYear,
				CurrentPosition = a.CurrentPosition
			}).ToList();
		}

		public async Task<AlumniViewModel> GetByIdAsync(string id)
		{
			var alumni = await repository.GetByIdAsync(id);

			if(alumni == null)
			{
				return null;
			}

			return new AlumniViewModel
			{
				Id = alumni.Id,
				UserId = alumni.UserId,
				FacultyNumber = alumni.FacultyNumber,
				FirstName = alumni.FirstName,
				Surname = alumni.Surname,
				LastName = alumni.LastName,
				Email = alumni.Email,
				Faculty = (int)alumni.Faculty,
				Degree = alumni.Degree,
				GraduationYear = alumni.GraduationYear,
				CurrentPosition = alumni.CurrentPosition
			};
		}

		public async Task UpdateAsync(AlumniViewModel model)
		{
			var alumni = new Alumni
			{
				Id = model.Id,
				UserId = model.UserId,
				FacultyNumber = model.FacultyNumber,
				FirstName = model.FirstName,
				Surname = model.Surname,
				LastName = model.LastName,
				Email = model.Email,
				Faculty = (Faculty)model.Faculty,
				Degree = model.Degree,
				GraduationYear = model.GraduationYear,
				CurrentPosition = model.CurrentPosition
			};

			await repository.UpdateAsync(alumni);
		}
	}
}