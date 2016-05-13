using nFury.Utils.State;
using strange.extensions.command.impl;
using nFury.Utils.Diagnostics;
using nFury.Assets;
using nFury.Utils.Core;
using nFury.Utils;

namespace rot.command
{
	class InitRoutineRunnerCmd : Command
	{
		[Inject]
		public IRoutineRunner runner {get;set;}

		public override void Execute ()
		{

		}

	}

}
