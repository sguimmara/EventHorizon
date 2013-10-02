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
        public int ScreenDepth;
        protected Rect Size;

        public Vector3 Direction;

        public float Acceleration;
        public float Inertia;

        public float CurrentSpeed;
        public float MaxSpeed;


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

        protected virtual void Update()
        {
            UpdatePosition();

            EnforceDepth();
            DestroyWhenOutOfVoidArea();
        }

        public void Accelerate()
        {
            CurrentSpeed += Acceleration * Time.deltaTime;
        }

        protected void UpdatePosition()
        {
            CurrentSpeed *= (1 - Inertia);

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxSpeed);

            transform.Translate(Direction * CurrentSpeed);
        }

        void EnforceDepth()
        {
            Vector3 p = transform.position;
            transform.position = new Vector3(p.x, p.y, (float)ScreenDepth);
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        protected virtual void Awake()
        {
            Size = GetRectSize();
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
            ScreenDepth = 0;
            CurrentSpeed = MaxSpeed;
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
    }
}
