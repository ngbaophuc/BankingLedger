using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class UnauthenticatedScreen : CommandGroup
	{
		public UnauthenticatedScreen(IContext context) : base(null, context)
		{
			Children.Add(new SignIn(context));
			Children.Add(new SignUp(context));
			Children.Add(new Quit(context));
		}

		public override Task ExecuteAsync()
		{
			Context.CommandStack.Clear();
			return base.ExecuteAsync();
		}
	}
}
