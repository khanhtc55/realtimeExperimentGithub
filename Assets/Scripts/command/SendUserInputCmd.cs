using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class SendUserInputCmd : Command
{

    public override void Execute()
    {
        VisualSystem visualSys = Service.Get<VisualSystem>();
        MainGameloopSystem gameloopSystem = Service.Get<MainGameloopSystem>();

        if (visualSys.playerId == 0)
        {
            gameloopSystem.OnReceiveUserInput(0, visualSys.curGameFrameIndex);
        }
        else
        {
            //send the user input to the client which is playing as server role
            //attract: visualSys.curGameFrameIndex
        }
    }

}
