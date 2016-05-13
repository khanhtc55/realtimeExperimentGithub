using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class OnReceiveUserInputCmd : Command {

    [Inject]
    public int userInputFrame { get; set; }

    public override void Execute()
    {
        Service.Get<MainGameloopSystem>().OnReceiveUserInput(1, userInputFrame);
    }

}
