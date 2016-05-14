using nFury.Utils.Core;
using rot.command;
using strange.extensions.signal.impl;
using System.Collections.Generic;

namespace rot.main.datamanager
{
    public class SignalManager
    {
        //[Inject]
        //public FinishLoadWorldMapSignal finishLoadWorldMapSignal { get; set; }

        [Inject]
        public SendFirstSetupSignal sendFirstSetupSignal { get; set; }

        [Inject]
        public SendClientAckSignal sendClientAckSignal { get; set; }

        [Inject]
        public SendUpdateSnapshotSignal sendUpdateSnapshotSignal { get; set; }

        [Inject]
        public OnReceiveClientAckSignal receiveClientAckSignal { get; set; }

        [Inject]
        public OnReceiveUpdateSnapshotSignal receiveUpdateSnapshotSignal { get; set; }

        [Inject]
        public StartNewGameSignal startNewGameSignal { get; set; }

        [Inject]
        public StartNewOfflineGameSignal startNewOfflineGameSignal { get; set; }

        [Inject]
        public SendUserInputSignal sendUserInputSignal { get; set; }

        [Inject]
        public OnReceiveUserInputSignal receiveUserInputSignal { get; set; }

        public void Initialization()
        {
            Service.Set(this);
        }
    }

    public class SendFirstSetupSignal : Signal<FirstSetupData> { }

    public class StartNewGameSignal : Signal { }

    public class StartNewOfflineGameSignal : Signal { }

    public class SendClientAckSignal : Signal<ClientAckData> { }

    public class SendUpdateSnapshotSignal : Signal<UpdateSnapshotData> { }

    public class OnReceiveClientAckSignal : Signal<ClientAckData> { }

    public class OnReceiveUpdateSnapshotSignal : Signal<UpdateSnapshotData> { }

    public class SendUserInputSignal : Signal { }

    public class OnReceiveUserInputSignal : Signal<int> { }

}