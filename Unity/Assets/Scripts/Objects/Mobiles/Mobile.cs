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
        public bool NeverDestroy;

        protected Rect Size;

        public Rect GetRectSize()
        {
            Bounds b = gameObject.GetComponent<MeshRenderer>().bounds;
            Rect size = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

            return size;
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        protected virtual void Awake()
        {
            Size = GetRectSize();
            enabled = false;
        }

        protected virtual void OnBecameVisible()
        {
            enabled = true;
        }

        void OnBecameInvisible()
        {
            if (!NeverDestroy)
                Destroy(gameObject);
        }

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
