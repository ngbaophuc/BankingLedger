using System;

namespace BankingLedger.Api.Models
{
	public class Transaction
	{
		public DateTime DateTime { get; set; }
		public string Username { get; set; }
		public decimal Amount { get; set; }
	}
}
