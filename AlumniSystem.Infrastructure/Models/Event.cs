namespace AlumniSystem.Infrastructure.Models
{
	public class Event
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		public string Description { get; set; } = null!;

		public DateTime Date { get; set; }

		public string Location { get; set; } = null!;
	}
}