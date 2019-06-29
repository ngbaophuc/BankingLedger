using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class SignUp : CommandBase
	{
		public SignUp(IContext context) : base("Sign Up", context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			Console.CursorVisible = true;

			string username;
			do
			{
				Console.Write("Username: ");
				username = Console.ReadLine();
			}
			while (string.IsNullOrWhiteSpace(username));

			Console.Write("First Name: ");
			var firstName = Console.ReadLine();

			Console.Write("Last Name: ");
			var lastName = Console.ReadLine();

			var password = "password";
			var confirmPassword = "confirmPassword";

			while (password != confirmPassword || password.Trim().Length < 3)
			{
				Console.Write("Password: ");
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

				Console.Write("Confirm Password: ");
				confirmPassword = string.Empty;

				keyInfo = Console.ReadKey(true);
				while (keyInfo.Key != ConsoleKey.Enter)
				{
					if (keyInfo.Key != ConsoleKey.Backspace)
					{
						confirmPassword += keyInfo.KeyChar.ToString();
					}
					else
					{
						confirmPassword = confirmPassword.Substring(0, password.Length - 1);
					}

					keyInfo = Console.ReadKey(true);
				}

				Console.WriteLine();

				if (password != confirmPassword)
				{
					Console.WriteLine();
					Console.WriteLine("Error: Password and Confirmation Password are mismatched.");
					Console.WriteLine();
				}

				if (password.Trim().Length < 3)
				{
					Console.WriteLine();
					Console.WriteLine("Error: Password must contain at least 3 characters.");
					Console.WriteLine();
				}
			}

			var signUpMsg = await Context.HttpClient.PostAsJsonAsync(new Uri(Context.ApiUri, "account/signup"), new { username, firstName, lastName, password });

			Console.WriteLine();

			if (signUpMsg.StatusCode == HttpStatusCode.BadRequest)
			{
				Console.WriteLine("Error: Username is registered.");
			}
			else if (signUpMsg.IsSuccessStatusCode)
			{
				Console.WriteLine("Your account has been created.");
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
