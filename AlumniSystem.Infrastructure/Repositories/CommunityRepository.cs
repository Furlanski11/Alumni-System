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
				.Include(c => c.Members)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Community> GetByIdAsync(int id)
		{
			var community = await context.Communities
				.Include(c => c.Members)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

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

		public async Task AddMemberAsync(int communityId, int alumniId)
		{
			var community = await context.Communities
				.Include(c => c.Members)
				.FirstOrDefaultAsync(c => c.Id == communityId);

			var alumni = await context.Alumnis
				.FirstOrDefaultAsync(a => a.Id == alumniId);

			if (community == null || alumni == null)
			{
				throw new ArgumentNullException("Community or Alumni not found!");
			}

			if (community.Members == null)
			{
				community.Members = new List<Alumni>();
			}

			if (!community.Members.Any(a => a.Id == alumniId))
			{
				community.Members.Add(alumni);
				await context.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException("Alumni is already a member of this community.");
			}
		}

		public async Task RemoveMemberAsync(int communityId, int alumniId)
		{
			var community = await context.Communities
				.Include(c => c.Members)
				.FirstOrDefaultAsync(c => c.Id == communityId);

			if (community == null || community.Members == null)
			{
				throw new ArgumentNullException("Community not found or has no members!");
			}

			var alumni = community.Members.FirstOrDefault(a => a.Id == alumniId);
			
			if (alumni != null)
			{
				community.Members.Remove(alumni);
				await context.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException("Alumni is not a member of this community.");
			}
		}
	}
}