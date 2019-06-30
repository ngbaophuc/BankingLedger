using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class Withdraw : CommandBase
	{
		public Withdraw(IContext context) : base("Withdraw", context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			Console.CursorVisible = true;

			var amount = InputHelpers.InputMoney("Amount to Withdraw: ");

			var withdrawMsg = await Context.HttpClient.PostAsJsonAsync(new Uri(Context.ApiUri, "transaction/withdraw"), new { amount });

			Console.WriteLine();

			if (withdrawMsg.IsSuccessStatusCode)
			{
				OutputHelpers.Notify($"You has successfully withdrawed {amount} from your account.");
			}
			else if (withdrawMsg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				OutputHelpers.Notify("Your session has timed out.");
				await Context.RemoveTokenAsync();

				return;
			}
			else if (withdrawMsg.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				OutputHelpers.Notify($"Error: Your withdrawal amount is invalid.");
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
