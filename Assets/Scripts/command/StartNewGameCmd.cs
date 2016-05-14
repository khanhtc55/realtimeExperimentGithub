using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using rot.main.datamanager;
using nFury.Utils.Core;

public class StartNewGameCmd : Command {

    [Inject]
    public SignalManager signalManager { get; set; }

    public override void Execute()
    {
		Service.Get<RealtimeTestConnector>().DoConnect();
    }
}
