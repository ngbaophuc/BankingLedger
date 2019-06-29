using System;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	abstract class CommandBase
	{
		private readonly string _name;

		protected IContext Context { get; }

		public bool Selected { get; set; }

		public CommandBase(string name, IContext context)
		{
			_name = name;
			Context = context;
		}

		public void Render()
		{
			if (!string.IsNullOrEmpty(_name))
				Console.WriteLine(Selected ? "[x] " + _name : "[ ] " + _name);
		}

		protected void RenderTitle()
		{
			Console.Clear();

			if (!string.IsNullOrEmpty(_name))
			{
				Console.WriteLine(_name.ToUpper());
				Console.WriteLine();
			}
		}

		public abstract Task ExecuteAsync();
	}
}
