using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	public delegate Task AsyncEventHandler(object sender, EventArgs e);

	interface IContext : IDisposable
	{
		Uri ApiUri { get; }

		HttpClient HttpClient { get; }

		Stack<CommandBase> CommandStack { get; }

		UserProfile UserProfile { get; }

		event AsyncEventHandler OnAuthenticated;

		event AsyncEventHandler OnUnauthenticated;

		Task SetTokenAsync(string token);

		Task RemoveTokenAsync();

		Task StartAsync();
	}

	class Context : IContext
	{
		string _token = default;

		public Context(Uri apiUrl)
		{
			ApiUri = apiUrl;
		}

		public HttpClient HttpClient { get; } = new HttpClient();

		public Uri ApiUri { get; }

		public Stack<CommandBase> CommandStack { get; } = new Stack<CommandBase>();

		public UserProfile UserProfile { get; internal set; }

		public event AsyncEventHandler OnAuthenticated;

		public event AsyncEventHandler OnUnauthenticated;

		public void Dispose()
		{
			HttpClient.Dispose();
		}

		public async Task RemoveTokenAsync()
		{
			if (_token == default) return;

			_token = default;
			UserProfile = default;
			HttpClient.DefaultRequestHeaders.Authorization = default;

			if (OnUnauthenticated != null)
				await OnUnauthenticated(this, EventArgs.Empty);
		}

		public async Task SetTokenAsync(string token)
		{
			if (token == _token) return;

			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var getUserProfileMsg = await HttpClient.GetAsync(new Uri(ApiUri, "account/user_profile"));

			if (!getUserProfileMsg.IsSuccessStatusCode)
			{
				await RemoveTokenAsync();
				return;
			}

			_token = token;
			UserProfile = await getUserProfileMsg.Content.ReadAsAsync<UserProfile>();

			if (OnAuthenticated != null)
				await OnAuthenticated(this, EventArgs.Empty);
		}

		public async Task StartAsync()
		{
			if (_token == default)
			{
				if (OnUnauthenticated != null)
					await OnUnauthenticated(this, EventArgs.Empty);
			}
			else
			{
				if (OnAuthenticated != null)
					await OnAuthenticated(this, EventArgs.Empty);
			}
		}
	}

	class UserProfile
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
