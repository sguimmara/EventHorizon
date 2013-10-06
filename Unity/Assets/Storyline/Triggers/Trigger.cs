using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Triggers
{
    public enum TriggerType { Player }

    public abstract class TriggerBehaviour : MonoBehaviour
    {
        public TriggerType type;

        protected abstract void Trigger();

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.tag == type.ToString())
                Trigger();
        }
    }
}
