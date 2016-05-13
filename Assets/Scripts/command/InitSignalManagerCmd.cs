using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using rot.main.datamanager;

namespace rot.command
{
    public class InitSignalManagerCmd : Command
    {

        //[Inject(ContextKeys.CONTEXT_VIEW)]
        //public GameObject contextView { get; set; }

        [Inject]
        public SignalManager signalManager { get; set; }

        public override void Execute()
        {
            signalManager.Initialization();
        }

    }
}
