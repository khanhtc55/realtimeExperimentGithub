namespace Artemis.System
{

    #region Using statements

    using global::System;
    
    #endregion Using statements

    public abstract class EntitySystemWithTime : EntitySystem
    {

        private bool subscribeSimTime;

        public EntitySystemWithTime(bool subscribeSimTime)
            : base()
        {
            this.subscribeSimTime = subscribeSimTime;
        }

        public override void Process()
        {
            Process(subscribeSimTime ? entityWorld.SimTimeDelta : entityWorld.ViewTimeDelta);
        }

        public abstract void Process(float deltaTime);

    }
}
