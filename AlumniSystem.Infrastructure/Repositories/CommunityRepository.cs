using AlumniSystem.Infrastructure.Data;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AlumniSystem.Infrastructure.Repositories
{
	public class CommunityRepository : ICommunityRepository
	{
		private readonly AlumniDbContext context;

		public CommunityRepository(AlumniDbContext _context)
		{
			context = _context;
		}

		public async Task AddAsync(Community community)
		{
			await context.Communities.AddAsync(community);
			await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var communityToDelete = await context.Communities.FindAsync(id);

			if (communityToDelete == null)
			{
				throw new ArgumentNullException("Community not found!");
			}

			context.Communities.Remove(communityToDelete);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Community>> GetAllAsync()
		{
			return await context.Communities
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Community> GetByIdAsync(int id)
		{
			var community = await context.Communities.FirstOrDefaultAsync(c => c.Id == id);

			if (community == null)
			{
				throw new ArgumentNullException("Community not found!");
			}

			return community;
		}

		public async Task UpdateAsync(Community community)
		{
			context.Communities.Update(community);
			await context.SaveChangesAsync();
		}
	}
}