using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;
using rot.main.datamanager;

namespace rot.command
{
    public class FirstSetupCmd : Command
    {

        [Inject]
        public FirstSetupData firstSetupData { get; set; }

        [Inject]
        public SignalManager signalManager { get; set; }

        public override void Execute()
        {
            VisualSystem visualSys = Service.Get<VisualSystem>();
            MainGameloopSystem mainGameloopSys = Service.Get<MainGameloopSystem>();

            visualSys.ResetGame();
            visualSys.StartIn(firstSetupData.visualStartIn);

            if (firstSetupData.isPlayingServerRole)
            {
                visualSys.playerId = 0;
                mainGameloopSys.ResetGame();
                mainGameloopSys.StartIn(firstSetupData.visualStartIn - mainGameloopSys.futureTimeAdvance);
            }
            else visualSys.playerId = 1;

            //Note: require ack signal later
        }

    }

    public class FirstSetupData
    {
        public float visualStartIn;
        public bool isPlayingServerRole;

        public FirstSetupData(float visualStartIn, bool isPlayingServerRole)
        {
            this.visualStartIn = visualStartIn;
            this.isPlayingServerRole = isPlayingServerRole;
        }
    }
}
