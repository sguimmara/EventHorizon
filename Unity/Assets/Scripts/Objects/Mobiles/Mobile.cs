using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.FX;

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
            enabled = true;
        }

        // Destroy the mobile when its rectangle is *totally* out of Spawn area.
        protected virtual void DestroyWhenOutOfVoidArea()
        {
            if (this.transform.position.x < Globals.VoidArea.x - Size.width / 2
                || this.transform.position.x > Globals.VoidArea.x + Globals.VoidArea.width + Size.width / 2
                || this.transform.position.y < Globals.VoidArea.y - Size.height / 2
                || this.transform.position.y > Globals.VoidArea.y + Globals.VoidArea.height + Size.height / 2)
            {
                Destroy(this.gameObject);
            }
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

        void OnGUI()
        {
            string s = string.Concat(
                "Speed: ", CurrentSpeed,
                " Acc: ", Acceleration,
                " X: ", Direction.x,
                " Y: ", Direction.y);

            GUI.Label(new Rect(0, 0, 200, 20), s);
        }

        void OnBecameVisible()
        {
            enabled = true;
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
