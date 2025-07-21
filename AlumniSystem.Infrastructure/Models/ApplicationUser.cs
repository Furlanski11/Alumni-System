using Microsoft.AspNetCore.Identity;

namespace AlumniSystem.Infrastructure.Models
{
	public class ApplicationUser : IdentityUser
	{
		public Alumni Alumni { get; set; }
	}
}
