using AlumniSystem.Infrastructure.Data;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AlumniSystem.Infrastructure.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly AlumniDbContext context;

		public EventRepository(AlumniDbContext _context)
		{
			context = _context;
		}

		public async Task AddAsync(Event e)
		{
			await context.Events.AddAsync(e);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var e = context.Events.FirstOrDefault(e => e.Id == id);
			
			if (e == null) 
			{
				throw new ArgumentNullException("Event not found!");
			}

			context.Events.Remove(e);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Event>> GetAllAsync()
		{
			return await context.Events.
				AsNoTracking()
				.ToListAsync();
		}

		public async Task<Event> GetByIdAsync(int id)
		{
			var e = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

			if (e == null) 
			{
				throw new ArgumentNullException("Event not found!");
			}

			return e;
		}

		public async Task UpdateAsync(Event e)
		{
			context.Events.Update(e);
			await context.SaveChangesAsync();
		}
	}
}