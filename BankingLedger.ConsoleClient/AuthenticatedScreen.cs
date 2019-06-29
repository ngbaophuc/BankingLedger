using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class AuthenticatedScreen : CommandGroup
	{
		public AuthenticatedScreen(IContext context) : base(null, context)
		{
			Children.Add(new Deposit(context));
			Children.Add(new Withdraw(context));
			Children.Add(new ViewBalance(context));
			Children.Add(new ViewTransactionHistory(context));
			Children.Add(new SignOut(context));
		}

		public override Task ExecuteAsync()
		{
			Context.CommandStack.Clear();
			return base.ExecuteAsync();
		}
	}
}
