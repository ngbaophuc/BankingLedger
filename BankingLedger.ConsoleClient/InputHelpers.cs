using System;
using System.Text.RegularExpressions;

namespace BankingLedger.ConsoleClient
{
	static class InputHelpers
	{
		public static string InputPassword(string message)
		{
			string password;
			Console.Write(message);
			password = string.Empty;

			var keyInfo = Console.ReadKey(true);
			while (keyInfo.Key != ConsoleKey.Enter)
			{
				if (keyInfo.Key != ConsoleKey.Backspace)
				{
					password += keyInfo.KeyChar.ToString();
				}
				else
				{
					password = password.Substring(0, password.Length - 1);
				}

				keyInfo = Console.ReadKey(true);
			}

			Console.WriteLine();

			return password;
		}

		public static string InputString(string message)
		{
			Console.Write(message);
			return Console.ReadLine();
		}

		public static string InputNonEmptyString(string message)
		{
			string result;
			do
			{
				Console.Write(message);
				result = Console.ReadLine();
			}
			while (string.IsNullOrWhiteSpace(result));

			return result;
		}

		public static string InputDateTime(string message)
		{
			DateTime dt = default;
			string dtInput;
			do
			{
				Console.Write(message);
				dtInput = Console.ReadLine();
			}
			while (!string.IsNullOrWhiteSpace(dtInput) && !DateTime.TryParse(dtInput, out dt));

			return string.IsNullOrWhiteSpace(dtInput)
				? string.Empty
				: dt.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
		}

		public static decimal InputMoney(string message)
		{
			decimal amount;
			string amountString;
			var moneyRegex = new Regex(@"^[0-9]+(\.)?([0-9]{1,2})?$");

			do
			{
				amountString = string.Empty;
				Console.Write(message);

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

			return amount;
		}
	}
}
