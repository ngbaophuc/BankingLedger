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

			var username = InputHelpers.InputNonEmptyString("Username: ");
			var password = InputHelpers.InputPassword("Password: ");

			var signInMsg = await Context.HttpClient.PostAsJsonAsync(new Uri(Context.ApiUri, "account/signin"), new { username, password });

			Console.WriteLine();

			if (signInMsg.StatusCode == HttpStatusCode.Unauthorized)
			{
				Console.WriteLine("Error: Username or password are incorrect.");
				Console.ReadKey();

				if (Context.CommandStack.Count > 0)
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

				if (Context.CommandStack.Count > 0)
					await Context.CommandStack.Pop().ExecuteAsync();
			}
		}
	}

	class TokenInfo
	{
		public string Token { get; set; }
	}
}
