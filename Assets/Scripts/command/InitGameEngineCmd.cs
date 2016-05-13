
using nFury.Utils.State;
using strange.extensions.command.impl;
using nFury.Utils.Diagnostics;
using nFury.Assets;
using nFury.Utils.Core;

using nFury.Utils;
using UnityEngine;
using rot.main.logic;

namespace rot.command
{
	class InitGameEngineCmd : Command
	{
		[Inject]
		public GameEngine engine {get;set;}


		public override void Execute ()
		{

		}
	}

}
