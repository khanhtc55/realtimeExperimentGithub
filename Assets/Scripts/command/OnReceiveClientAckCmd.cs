using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class OnReceiveClientAckCmd : Command
{
    [Inject]
    public ClientAckData clientAckData { get; set; }

    public override void Execute()
    {
        Service.Get<MainGameloopSystem>().OnReceiveClientAckData(clientAckData);
    }
}
