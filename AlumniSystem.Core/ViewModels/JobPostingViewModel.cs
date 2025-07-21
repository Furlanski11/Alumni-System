namespace AlumniSystem.Core.ViewModels
{
	public class JobPostingViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		public string Company { get; set; } = null!;

		public string Description { get; set; } = null!;

		public string Location { get; set; } = null!;

		public DateTime PostedOn { get; set; }
	}
}