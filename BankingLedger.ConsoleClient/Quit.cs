using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class Quit : CommandBase
	{
		public Quit(IContext context) : base("Quit", context) { }

		public override async Task ExecuteAsync() { }
	}
}
