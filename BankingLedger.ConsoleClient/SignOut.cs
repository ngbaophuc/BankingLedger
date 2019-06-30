using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class SignOut : CommandBase
	{
		public SignOut(IContext context) : base("Sign Out", context) { }

		public override async Task ExecuteAsync()
		{
			await Context.RemoveTokenAsync();
		}
	}
}
