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

			DateTime frm = default;
			string frmInput;
			do
			{
				Console.Write("From (e.g., Jan 1, 2019 1:05:30am): ");
				frmInput = Console.ReadLine();
			}
			while (!string.IsNullOrWhiteSpace(frmInput) && !DateTime.TryParse(frmInput, out frm));

			string dateFrm = string.IsNullOrWhiteSpace(frmInput) 
				? string.Empty 
				: frm.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");

			DateTime to = default;
			string toInput;
			do
			{
				Console.Write("To (e.g., Jan 31, 2019 11:55:55pm): ");
				toInput = Console.ReadLine();
			}
			while (!string.IsNullOrWhiteSpace(toInput) && !DateTime.TryParse(toInput, out to));

			string dateTo = string.IsNullOrWhiteSpace(toInput) 
				? string.Empty 
				: to.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");

			var viewTransactionsMsg = await Context.HttpClient.GetAsync(new Uri(Context.ApiUri, $"transaction/transactions?from={dateFrm}&to={dateTo}"));

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
			}
			else
			{
				Console.WriteLine("Error: Unknown. Please try again later.");
			}

			Console.ReadKey();
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
