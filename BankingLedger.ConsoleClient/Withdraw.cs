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

			decimal amount;
			string amountString;
			var moneyRegex = new Regex(@"^[0-9]+(\.)?([0-9]{1,2})?$");

			do
			{
				amountString = string.Empty;
				Console.Write("Amount to Withdraw: ");

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
