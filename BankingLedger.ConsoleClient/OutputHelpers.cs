using System;

namespace BankingLedger.ConsoleClient
{
	class OutputHelpers
	{
		public static void Notify(string message)
		{
			Console.WriteLine(message);
			Console.ReadKey();
		}
	}
}
