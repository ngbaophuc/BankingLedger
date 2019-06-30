using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class ViewTransactionHistory : CommandBase
	{
		public ViewTransactionHistory(IContext context) : base("View Transaction History", context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			Console.CursorVisible = true;

			var frm = InputHelpers.InputDateTime("From (e.g., Jan 1, 2019 1:05:30am): ");
			var to = InputHelpers.InputDateTime("To (e.g., Jan 31, 2019 11:55:55pm): ");

			var viewTransactionsMsg = await Context.HttpClient.GetAsync(new Uri(Context.ApiUri, $"transaction/transactions?from={frm}&to={to}"));

			if (viewTransactionsMsg.IsSuccessStatusCode)
			{
				var transactionHistoryInfo = await viewTransactionsMsg.Content.ReadAsAsync<TransactionHistoryInfo>();

				Console.WriteLine();
				Console.WriteLine(transactionHistoryInfo.Transactions.Count() > 0
					? "Transactions:"
					: "You don't have any transactions in this period.");

				foreach (var tran in transactionHistoryInfo.Transactions)
				{
					Console.Write($"{tran.DateTime.ToLocalTime().ToString()}: {(tran.Amount > 0 ? "Deposit " : "Withdraw")} {Math.Abs(tran.Amount)}");
					Console.WriteLine();
				}

				Console.ReadKey();
			}
			else if (viewTransactionsMsg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				OutputHelpers.Notify("Your session has timed out.");
				await Context.RemoveTokenAsync();
			}
			else
			{
				OutputHelpers.Notify("Error: Unknown. Please try again later.");
			}

			if (Context.CommandStack.Count > 0)
				await Context.CommandStack.Pop().ExecuteAsync();
		}
	}

	class TransactionHistoryInfo
	{
		public IEnumerable<Transaction> Transactions { get; set; }
	}

	class Transaction
	{
		public DateTime DateTime { get; set; }
		public decimal Amount { get; set; }
	}
}
