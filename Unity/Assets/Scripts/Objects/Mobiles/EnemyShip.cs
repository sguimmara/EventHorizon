using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;
using EventHorizon.AI;

namespace EventHorizon.Objects
{
    public class EnemyShip : Ship, ICollidable
    {
        public bool AutoTrigger;

        public AIContainer AIBehaviours;

        public override string ToString()
        {
            return "EnemyShip";
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();

            if (Engine.Instance != null)
                Engine.Instance.AddShip(this);

            if (AIBehaviours.motionPattern != null)
                AIBehaviours.motionPattern.Create(this.transform);
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                if (AIBehaviours.motionPattern != null)
                    if (AIBehaviours.motionPattern.GetType() == typeof(FollowPath))
                    {
                        ((FollowPath)AIBehaviours.motionPattern).currentTransform = transform;
                        ((FollowPath)AIBehaviours.motionPattern).OnDrawGizmos();
                    }
        }

        public override void Trigger()
        {
            for (int i = 0; i < Slots.Length; i++)
                Slots[i].Trigger();
        }

        protected override void Update()
        {
            base.Update();
            if (AutoTrigger)
                Trigger();
        }
    }
}