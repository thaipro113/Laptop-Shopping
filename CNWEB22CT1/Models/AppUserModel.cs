using Microsoft.AspNetCore.Identity;

namespace CNWEB22CT1.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation { get; set; }
		public string RoleId { get; set; }
	}
}
