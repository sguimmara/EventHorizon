using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.Effects;
using UnityEditor;

namespace EventHorizon.Objects
{
    public abstract class Mobile : MonoBehaviour
    {
        public override string ToString()
        {
            return gameObject.name;
        }

        public virtual void NotifyHitByLaser(LaserType type) { }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.5F);
        }

        void OnDrawGizmosSelected()
        {
            Handles.Label(transform.position, name);
        }
    }
}
