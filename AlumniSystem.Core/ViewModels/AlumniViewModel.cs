using System.ComponentModel.DataAnnotations;

namespace AlumniSystem.Core.ViewModels
{
	public class AlumniViewModel
	{
		public int Id { get; set; }
		
		[Required]		
		[MinLength(10)]
		[MaxLength(10)]
		public string FacultyNumber { get; set; } = null!;

		[Required]
		public string FirstName { get; set; } = null!;

		[Required]
		public string Surname { get; set; } = null!;

		[Required]
		public string LastName { get; set; } = null!;

		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public int Faculty { get; set; }

		[Required]
		public string Degree { get; set; } = null!;

		[Required]
		public DateTime GraduationYear { get; set; }

		[Required]
		public string CurrentPosition { get; set; } = null!;
	}
}