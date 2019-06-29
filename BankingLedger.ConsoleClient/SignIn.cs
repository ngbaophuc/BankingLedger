using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class SignIn : CommandBase
	{
		public SignIn(IContext context) : base("Sign In", context) { }

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

			Console.Write("Password: ");
			var password = string.Empty;

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

			var signInMsg = await Context.HttpClient.PostAsJsonAsync(new Uri(Context.ApiUri, "account/signin"), new { username, password });

			Console.WriteLine();

			if (signInMsg.StatusCode == HttpStatusCode.Unauthorized)
			{
				Console.WriteLine("Error: Username or password are incorrect.");
				Console.ReadKey();
				await Context.CommandStack.Pop().ExecuteAsync();
			}
			else if (signInMsg.IsSuccessStatusCode)
			{
				var tokenInfo = await signInMsg.Content.ReadAsAsync<TokenInfo>();

				Context.SetToken(tokenInfo.Token);
				await new AuthenticatedScreen(Context).ExecuteAsync();
			}
			else
			{
				Console.WriteLine("Error: Unknown. Please try again later.");
				Console.ReadKey();
				await Context.CommandStack.Pop().ExecuteAsync();
			}
		}
	}

	class TokenInfo
	{
		public string Token { get; set; }
	}
}
