namespace Artemis.System
{

    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>The entity processing system - a template for processing many entities, tied to components.</summary>
    public abstract class EntityProcessingSystemWithTime : EntitySystem
    {

        private bool subscribeSimTime;
        private float deltaTime;

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected EntityProcessingSystemWithTime(Aspect aspect, bool subscribeSimTime)
            : base(aspect)
        {
            this.subscribeSimTime = subscribeSimTime;
        }

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem"/> class.</summary>
        /// <param name="requiredType">The required Type.</param>
        /// <param name="otherTypes">The optional other types.</param>
        protected EntityProcessingSystemWithTime(Type requiredType, params Type[] otherTypes)
            : base(EntitySystem.GetMergedTypes(requiredType, otherTypes))
        {
        }

        /// <summary><para>Processes the specified entity.</para>
        /// <para>Users might extend this method when they want</para>
        /// <para>to process the specified entities.</para></summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity, float deltaTime);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            deltaTime = subscribeSimTime ? entityWorld.SimTimeDelta : entityWorld.ViewTimeDelta;
            foreach (Entity entity in entities.Values)
                if(!entity.DeletingState)
                {
                    this.Process(entity, deltaTime);
                }
            
        }
    }
}