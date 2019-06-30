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
				OutputHelpers.Notify("Error: Username or password are incorrect.");

				if (Context.CommandStack.Count > 0)
					await Context.CommandStack.Pop().ExecuteAsync();
			}
			else if (signInMsg.IsSuccessStatusCode)
			{
				var tokenInfo = await signInMsg.Content.ReadAsAsync<TokenInfo>();

				await Context.SetTokenAsync(tokenInfo.Token);
			}
			else
			{
				OutputHelpers.Notify("Error: Unknown. Please try again later.");

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
