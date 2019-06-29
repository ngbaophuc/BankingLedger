using System;
using System.Configuration;
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
				await context.RenderScreenAsync();
			}
		}
	}
}