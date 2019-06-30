using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class Deposit : CommandBase
	{
		public Deposit(IContext context) : base("Deposit", context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			Console.CursorVisible = true;

			var amount = InputHelpers.InputMoney("Amount to Deposit: ");

			var depositMsg = await Context.HttpClient.PostAsJsonAsync(new Uri(Context.ApiUri, "transaction/deposit"), new { amount });

			Console.WriteLine();

			if (depositMsg.IsSuccessStatusCode)
			{
				OutputHelpers.Notify($"You has successfully deposited {amount} into your account.");
			}
			else if (depositMsg.StatusCode == HttpStatusCode.Unauthorized)
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
}
