using AlumniSystem.Infrastructure.Models.Enums;

namespace AlumniSystem.Infrastructure.Models
{
	public class Alumni
	{
		public int Id { get; set; }

		public string UserId { get; set; } = null!;

		public string FirstName { get; set; } = null!;

		public string Surname { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public string FacultyNumber { get; set; } = null!;

		public string Email { get; set; } = null!;

		public Faculty Faculty { get; set; }

		public string Degree { get; set; } = null!;

		public DateTime GraduationYear { get; set; }

		public string CurrentPosition { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

		public ICollection<JobPosting>? JobPostings { get; set; } = new List<JobPosting>();

		public ICollection<Community>? Communities { get; set; } = new List<Community>();
	}
}