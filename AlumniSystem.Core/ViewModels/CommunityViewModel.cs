using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Core.ViewModels
{
	public class CommunityViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int Members { get; set; }
	}
}