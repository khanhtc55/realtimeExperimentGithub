using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;
using rot.main.datamanager;
using rot.command;

public class StartNewOfflineGameCmd : Command
{
    [Inject]
    public SignalManager signalManager { get; set; }

    public override void Execute()
    {
        Application.LoadLevel("Main");
        signalManager.sendFirstSetupSignal.Dispatch(new FirstSetupData(1.5f, true));
        Debug.Log("000000000 loaf new offline game");

    }

}
