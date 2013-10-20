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
            //enabled = false;
        }

        //void ListenToEnterOnGameArea()
        //{
        //    Vector3 pos = transform.position;
        //    Rect wb = Globals.GameArea;

        //    if (
        //    float newX = Mathf.Clamp(pos.x, wb.x + Size.width / 2, wb.x + wb.width - Size.width / 2);
        //    float newY = Mathf.Clamp(pos.y, wb.y + Size.height / 2, wb.y + wb.height - Size.height / 2);
        //    transform.position = new Vector3(newX, newY, transform.position.z);
        //}

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
