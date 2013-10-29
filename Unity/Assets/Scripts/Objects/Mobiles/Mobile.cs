using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.Effects;

namespace EventHorizon.Objects
{
    public abstract class Mobile : MonoBehaviour
    {
        public override string ToString()
        {
            return gameObject.name;
        }

        public virtual void NotifyHitByLaser(LaserType type) { }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.5F);
        }

        void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.Label(transform.position, name);
        }
#endif
    }
}
