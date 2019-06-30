using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingLedger.ConsoleClient
{
	class CommandGroup : CommandBase
	{
		public readonly List<CommandBase> Children = new List<CommandBase>();

		public CommandGroup(string name, IContext context) : base(name, context) { }

		public override async Task ExecuteAsync()
		{
			RenderTitle();
			
			if (Children.Count <= 0) return;

			Console.CursorVisible = false;
			Console.WriteLine($"{(Context.UserProfile != null ? $"Hello {Context.UserProfile.FirstName} {Context.UserProfile.LastName}, p" : "P")}lease choose:");

			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Render();
			}

			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
			}
			while (
				keyInfo.Key != ConsoleKey.UpArrow 
				&& keyInfo.Key != ConsoleKey.DownArrow 
				&& (keyInfo.Key != ConsoleKey.Enter || !Children.Exists(c => c.Selected))
			);

			if (keyInfo.Key == ConsoleKey.UpArrow)
			{
				if (Children[0].Selected || !Children.Exists(c => c.Selected))
				{
					Children[0].Selected = false;
					Children[Children.Count - 1].Selected = true;
				}
				else
				{
					for (int i = 1; i < Children.Count; i++)
					{
						if (Children[i].Selected)
						{
							Children[i].Selected = false;
							Children[i - 1].Selected = true;
							break;
						}
					}
				}

				await ExecuteAsync();
			}
			else if (keyInfo.Key == ConsoleKey.DownArrow)
			{
				if (Children[Children.Count - 1].Selected || !Children.Exists(c => c.Selected))
				{
					Children[Children.Count - 1].Selected = false;
					Children[0].Selected = true;
				}
				else
				{
					for (int i = 0; i < Children.Count - 1; i++)
					{
						if (Children[i].Selected)
						{
							Children[i].Selected = false;
							Children[i + 1].Selected = true;
							break;
						}
					}
				}

				await ExecuteAsync();
			}
			else
			{
				Context.CommandStack.Push(this);
				CommandBase selectedCommand = Children.SingleOrDefault(c => c.Selected);
				await selectedCommand?.ExecuteAsync();
			}
		}
	}
}
