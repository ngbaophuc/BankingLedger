using System;
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
				Console.WriteLine($"You has successfully deposited {amount} into your account.");
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
