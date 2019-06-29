using System;
using System.Net.Http;
using System.Text.RegularExpressions;
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
				Console.WriteLine($"You has successfully withdrawed {amount} from your account.");
			}
			else if (withdrawMsg.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				Console.WriteLine($"Error: Your withdrawal amount is invalid.");
			}
			else
			{
				Console.WriteLine("Error: Unknown. Please try again later.");
			}

			Console.ReadKey();

			await Context.CommandStack.Pop().ExecuteAsync();
		}
	}
}
