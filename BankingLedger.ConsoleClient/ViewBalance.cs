using System;
using System.Net;
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

				OutputHelpers.Notify($"Your balance is: {balanceInfo.Balance}");
			}
			else if (viewBalanceMsg.StatusCode == HttpStatusCode.Unauthorized)
			{
				OutputHelpers.Notify("Your session has timed out.");
				await Context.RemoveTokenAsync();
				return;
			}
			else
			{
				OutputHelpers.Notify("Error: Unknown. Please try again later.");
			}

			if (Context.CommandStack.Count > 0)
				await Context.CommandStack.Pop().ExecuteAsync();
		}
	}

	class BalanceInfo
	{
		public decimal Balance { get; set; }
	}

}
