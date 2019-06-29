using System.ComponentModel.DataAnnotations;

namespace BankingLedger.Api.Models
{
	public class SignupInfo
	{
		[Required]
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[Required, MinLength(3)]
		public string Password { get; set; }
	}
}
