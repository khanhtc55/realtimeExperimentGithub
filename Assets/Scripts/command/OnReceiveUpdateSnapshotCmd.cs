using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using nFury.Utils.Core;

public class OnReceiveUpdateSnapshotCmd : Command
{

    [Inject]
    public UpdateSnapshotData updateSnapshotData { get; set; }

    public override void Execute()
    {
        Service.Get<VisualSystem>().AddFrameData(updateSnapshotData.snapshots);
    }
}
