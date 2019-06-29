using System.ComponentModel.DataAnnotations;

namespace BankingLedger.Api.Models
{
	public class SigninInfo
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
