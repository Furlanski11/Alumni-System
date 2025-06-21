namespace AlumniSystem.Infrastructure.Models
{
	public class Community
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Description { get; set; } = null!;

		public ICollection<Alumni>? Members { get; set; } = new List<Alumni>();
	}
}