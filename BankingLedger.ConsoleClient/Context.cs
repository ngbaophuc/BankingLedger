using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BankingLedger.ConsoleClient
{
	interface IContext : IDisposable
	{
		Uri ApiUri { get; }

		HttpClient HttpClient { get; }

		Stack<CommandBase> CommandStack { get; }

		void SetToken(string token);

		void RemoveToken();
	}

	class Context : IContext
	{
		public Context(Uri apiUrl)
		{
			ApiUri = apiUrl;
		}

		public HttpClient HttpClient { get; } = new HttpClient();

		public Uri ApiUri { get; }

		public Stack<CommandBase> CommandStack { get; } = new Stack<CommandBase>();

		public async System.Threading.Tasks.Task<bool> AuthenticatedAsync()
		{
			var profileUri = new Uri(ApiUri, "account/user_profile");
			HttpResponseMessage userProfileResponse = await HttpClient.GetAsync(profileUri);

			return userProfileResponse.IsSuccessStatusCode;
		}

		public void Dispose()
		{
			HttpClient.Dispose();
		}

		public void RemoveToken()
		{
			HttpClient.DefaultRequestHeaders.Authorization = null;
		}

		public async System.Threading.Tasks.Task RenderScreenAsync()
		{
			if (await AuthenticatedAsync())
			{
				await new AuthenticatedScreen(this).ExecuteAsync();
			}
			else
			{
				await new UnauthenticatedScreen(this).ExecuteAsync();
			}
		}

		public void SetToken(string token)
		{
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}
	}
}
