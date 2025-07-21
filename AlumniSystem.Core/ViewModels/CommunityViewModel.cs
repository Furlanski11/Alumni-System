using AlumniSystem.Infrastructure.Models;

namespace AlumniSystem.Core.ViewModels
{
	public class CommunityViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public ICollection<AlumniViewModel>? Members { get; set; } = new List<AlumniViewModel>();
	}
}