using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using rot.main.datamanager;

public class StartNewGameCmd : Command {

    [Inject]
    public SignalManager signalManager { get; set; }

    public override void Execute()
    {
        
    }
}
