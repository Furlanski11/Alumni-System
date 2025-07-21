using AlumniSystem.Infrastructure.Data;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AlumniSystem.Infrastructure.Repositories
{
	public class AlumniRepository : IAlumniRepository
	{
		private readonly AlumniDbContext context;

		public AlumniRepository(AlumniDbContext _context)
		{
			context = _context;
		}

		public async Task AddAsync(Alumni alumni)
		{
			await context.Alumnis.AddAsync(alumni);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string id)
		{
			var alumniToDelete = await context.Alumnis.FindAsync(id);
			
			if (alumniToDelete == null)
			{
				throw new ArgumentNullException("Alumni not found!");
			}

			context.Alumnis.Remove(alumniToDelete);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Alumni>> GetAllAsync()
		{
			return await context.Alumnis
				.AsNoTracking() 
				.ToListAsync();
		}

		public async Task<Alumni?> GetByIdAsync(string id)
		{
			var alumni = await context.Alumnis
				.AsNoTracking()
				.FirstOrDefaultAsync(a => a.UserId == id);

			return alumni;
		}

		public async Task UpdateAsync(Alumni alumni)
		{
			context.Alumnis.Update(alumni);
			await context.SaveChangesAsync();
		}
	}
}