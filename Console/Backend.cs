using System.Collections.Generic;



public static class Backend
{
	public delegate void CommandFunction(string[] Args);

	public struct CommandInfo
	{
		public string[] HelpMessages;
		public CommandFunction Function;
	}


	public static Dictionary<string, CommandInfo> Commands =
		new Dictionary<string, CommandInfo> {
		{
			"help",
			new CommandInfo {
				HelpMessages = new string[] {
					"'help' Lists all commands",
					"'help <command>' Displays the help message for an individual command",
				},
				Function = (Args) => API.Help(Args)
			}
		},

		{
			"quit",
			new CommandInfo {
				HelpMessages = new string[] {
					"'quit' Immediately closes the game",
				},
				Function = (Args) => Game.Quit()
			}
		},


		{
			"host",
			new CommandInfo {
				HelpMessages = new string[] {
					$"'host new <savefile>' Starts hosting new savefile on port {Net.Port}",
				},
				Function = (Args) => API.Host(Args)
			}
		},

		{
			"give",
			new CommandInfo {
				HelpMessages = new string[] {
					"'give <player> <item> <count>' Gives the specified player an item of count",
				},
				Function = (Args) => API.Give(Args)
			}
		},

		{
			"chunk_render_distance",
			new CommandInfo {
				HelpMessages = new string[] {
					"'chunk_render_distance <distance>' Sets tile render distance in chunks",
				},
				Function = (Args) => API.ChunkRenderDistance(Args)
			}
		},

		{
			"fps_max",
			new CommandInfo {
				HelpMessages = new string[] {
					"'fps_max' Prints the current max fps",
					"'fps_max <fps>' Sets the max fps",
				},
				Function = (Args) => API.FpsMax(Args)
			}
		},
	};


	public static void RunCommand(string Line)
	{

		string[] Split = Line.Split(null);
		if(Split.Length >= 1)
		{
			string Name = Split[0];

			string[] Args = new string[Split.Length - 1];
			for(int Index = 1; Index < Split.Length; Index += 1)
				Args[Index - 1] = Split[Index];

			foreach(KeyValuePair<string, CommandInfo> Command in Commands)
			{
				if(Command.Key == Name)
				{
					Command.Value.Function(Args);
					return;
				}
			}

			Console.ThrowPrint($"No command '{Name}', try running 'help' to view  a list of all commands");
		}
	}
}
