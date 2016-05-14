using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class SendUpdateSnapshotCmd : Command
{
    [Inject]
    public UpdateSnapshotData updateSnapshotData { get; set; } 

    public override void Execute()
    {
        if (Service.Get<VisualSystem>().playerId == 0) 
            Service.Get<VisualSystem>().AddFrameData(updateSnapshotData.snapshots);
        else
        {
//            send [updateSnapshotData.snapshots] to [updateSnapshotData.playerId]
			Service.Get<RealtimeTestConnector>().SendUpdateSnapshot(updateSnapshotData.playerId, updateSnapshotData.snapshots);
        }
    }
}
