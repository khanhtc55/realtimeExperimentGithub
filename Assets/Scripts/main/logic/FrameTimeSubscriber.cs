using UnityEngine;
using System.Collections;
using Artemis;
using nFury.Utils.Scheduling;
using nFury.Utils.Core;

namespace rot.main.logic
{
    public class FrameTimeSubscriber : ISimTimeObserver, IViewPhysicsTimeObserver
    {

        EntityWorld entityWorld;

        public FrameTimeSubscriber()
        {
            Subcribe();
        }

        public void SubscribeUpdate(EntityWorld entityWorld)
        {
            this.entityWorld = entityWorld;
        }

        public void UnSubscribeUpdate()
        {
            this.entityWorld = null;
        }

        public void OnViewPhysicsTime(float dt)
        {
            if (entityWorld != null)
                entityWorld.Draw(dt / 1000f);
        }

        public void OnSimTime(uint dt)
        {
            if (entityWorld != null)
                entityWorld.Update(dt / 1000f);
        }

        public void Subcribe()
        {
            Service.Get<SimTimeEngine>().RegisterSimTimeObserver(this);
            Service.Get<ViewTimeEngine>().RegisterPhysicsTimeObserver(this);
        }

        public void Unsubcribe()
        {
            Service.Get<SimTimeEngine>().UnregisterSimTimeObserver(this);
            Service.Get<ViewTimeEngine>().UnregisterPhysicsTimeObserver(this);
        }
    }
}
