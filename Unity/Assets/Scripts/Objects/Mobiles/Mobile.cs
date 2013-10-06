using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.FX;
using UnityEditor;

namespace EventHorizon.Objects
{
    public abstract class Mobile : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 Direction;

        public float Acceleration = 1;
        public float Inertia = 0;

        [HideInInspector]
        public float CurrentSpeed;
        public float Speed;

        protected Rect Size;

        public Rect GetRectSize()
        {
            Bounds b = gameObject.GetComponent<MeshRenderer>().bounds;
            Rect size = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);

            return size;
        }

        public void Move(Vector3 direction)
        {
            Direction = Vector3.Normalize(Direction + direction);
            Accelerate();
        }

        public void Stop()
        {
            CurrentSpeed = 0;
        }

        public void Accelerate()
        {
            CurrentSpeed += Acceleration * Time.deltaTime;
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        protected virtual void Awake()
        {
            Direction = transform.right;
            Size = GetRectSize();
            enabled = false;
        }

        protected virtual void Start()
        {
            CurrentSpeed = Speed;
        }

        protected virtual void Update()
        {
            UpdatePosition();
        }

        protected virtual void UpdatePosition()
        {
            CurrentSpeed *= (1 - Inertia);

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, Speed);

            transform.Translate(Direction * CurrentSpeed);
        }

        protected virtual void OnBecameVisible()
        {
            enabled = true;
        }

        void OnBecameInvisible()
        {
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
