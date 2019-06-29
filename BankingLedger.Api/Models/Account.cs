namespace BankingLedger.Api.Models
{
	public class Account
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public decimal Balance { get; set; }
	}
}
