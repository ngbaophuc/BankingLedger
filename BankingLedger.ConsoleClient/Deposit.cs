using System;
using System.Net.Http;
using System.Text.RegularExpressions;
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

			decimal amount;
			string amountString;
			var moneyRegex = new Regex(@"^[0-9]+(\.)?([0-9]{1,2})?$");

			do
			{
				amountString = string.Empty;
				Console.Write("Amount to Deposit: ");

				var keyInfo = Console.ReadKey(true);
				while (keyInfo.Key != ConsoleKey.Enter)
				{
					if (keyInfo.Key != ConsoleKey.Backspace)
					{
						string temp = amountString + keyInfo.KeyChar;
						if (moneyRegex.IsMatch(temp))
						{
							Console.Write(keyInfo.KeyChar);
							amountString = temp;
						}
					}
					else if (amountString.Length > 0)
					{
						amountString = amountString.Substring(0, amountString.Length - 1);
						Console.Write("\b \b");
					}

					keyInfo = Console.ReadKey(true);
				}

				Console.WriteLine();
			}
			while (!decimal.TryParse(amountString, out amount) || amount <= 0);

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
