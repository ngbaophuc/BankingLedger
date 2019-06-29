using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class ViewBalance : CommandBase
	{
		public ViewBalance(IContext context) : base("View Balance", context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			Console.CursorVisible = true;

			var viewBalanceMsg = await Context.HttpClient.GetAsync(new Uri(Context.ApiUri, "account/balance"));

			if (viewBalanceMsg.IsSuccessStatusCode)
			{
				var balanceInfo = await viewBalanceMsg.Content.ReadAsAsync<BalanceInfo>();
				Console.WriteLine($"Your balance is: {balanceInfo.Balance}");
			}
			else
			{
				Console.WriteLine("Error: Unknown. Please try again later.");
			}

			Console.ReadKey();
			await Context.CommandStack.Pop().ExecuteAsync();
		}
	}

	class BalanceInfo
	{
		public decimal Balance { get; set; }
	}

}
