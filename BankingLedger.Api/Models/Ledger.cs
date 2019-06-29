using System.Collections.Generic;

namespace BankingLedger.Api.Models
{
	public static class Ledger
	{
		public static List<Account> Accounts { get; } = new List<Account>();

		public static List<Transaction> Transactions { get; } = new List<Transaction>();
	}
}
