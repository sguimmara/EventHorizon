using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;

namespace EventHorizon.Objects
{
    public class EnemyShip : Ship
    {
        public bool AutoTrigger = true;

        public override string ToString()
        {
            return "EnemyShip";
        }

        protected void Update()
        {

            if (AutoTrigger)
                Trigger();
        }
    }
}