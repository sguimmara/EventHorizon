using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;
using EventHorizon.AI;

namespace EventHorizon.Objects
{
    public class EnemyShip : Ship
    {
        public bool AutoTrigger = true;

        public AIContainer AIBehaviours;

        public override string ToString()
        {
            return "EnemyShip";
        }

        protected void OnBecameVisible()
        {
            if (Engine.Instance != null)
                Engine.Instance.AddShip(this);

            if (AIBehaviours.motionPattern != null)
                AIBehaviours.motionPattern.Create(this.transform);
        }

        protected void Update()
        {

            if (AutoTrigger)
                Trigger();
        }
    }
}