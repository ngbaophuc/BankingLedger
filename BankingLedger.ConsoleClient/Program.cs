using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var apiUrl = new Uri(ConfigurationManager.AppSettings.Get("BankingLedgerApiUrl"));

			using (var context = new Context(apiUrl))
			{
				context.OnAuthenticated += async (sender, e) => await new AuthenticatedScreen(context).ExecuteAsync();
				context.OnUnauthenticated += async (sender, e) => await new UnauthenticatedScreen(context).ExecuteAsync();

				try
				{
					await context.StartAsync();
				}
				catch (HttpRequestException)
				{
					Console.WriteLine();
					OutputHelpers.Notify("Cannot connect to Banking Ledger API Server. Please try again later.");
					await context.RemoveTokenAsync();
				}
			}
		}
	}
}