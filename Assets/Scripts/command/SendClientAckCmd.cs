using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class SendClientAckCmd : Command
{
    [Inject]
    public ClientAckData clientAckData { get; set; }

    public override void Execute()
    {
        if (Service.Get<VisualSystem>().playerId == 0)
            Service.Get<MainGameloopSystem>().OnReceiveClientAckData(clientAckData);
        else
        {
            //send to player that playing server role
        }
    }
}