using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using rot.main.logic;
using Artemis;
using nFury.Utils.Core;

namespace rot.command
{
    public class InitGameSystemCmd : Command
    {

        [Inject]
        public FrameTimeSubscriber frameTimeSubscriber { get; set; }

        public override void Execute()
        {
            EntityWorld entityWorld;
            if (!Service.IsSet<EntityWorld>())
            {
                entityWorld = new EntityWorld();
                Service.Set<EntityWorld>(entityWorld);
            }
            else entityWorld = Service.Get<EntityWorld>();

            TestSystem testSystem = new TestSystem(true);
            entityWorld.SystemManager.SetSystem<TestSystem>(testSystem, Artemis.Manager.GameLoopType.Update, 1);

            MainGameloopSystem gameloopSystem = new MainGameloopSystem(true);
            entityWorld.SystemManager.SetSystem<MainGameloopSystem>(gameloopSystem, Artemis.Manager.GameLoopType.Update, 3);

            VisualSystem visualSystem = new VisualSystem(true);
            entityWorld.SystemManager.SetSystem<VisualSystem>(visualSystem, Artemis.Manager.GameLoopType.Update, 4);


            frameTimeSubscriber.SubscribeUpdate(entityWorld);
        }
    }
}
