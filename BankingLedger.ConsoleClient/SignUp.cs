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

			var username = InputHelpers.InputNonEmptyString("Username: ");
			var firstName = InputHelpers.InputString("First Name: ");
			var lastName = InputHelpers.InputString("Last Name: ");
			var password = "password";
			var confirmPassword = "confirmPassword";

			while (password != confirmPassword || password.Trim().Length < 3)
			{
				password = InputHelpers.InputPassword("Password: ");
				confirmPassword = InputHelpers.InputPassword("Confirm Password: ");

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
