using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.Objects;
using EventHorizon.Core;

namespace EventHorizon.Objects
{
    public class Player : Ship, IPlayable, IMovable
    {
        [HideInInspector]
        public Vector3 Direction { get; set; }

        public float Acceleration = 1;
        public float Inertia = 0;

        [HideInInspector]
        public float CurrentSpeed;
        public float Speed;

        CharacterSheet characterSheet;

        void LimitShipPositionWithinBoundaries()
        {
            Vector3 pos = transform.position;
            Rect wb = Globals.GameArea;

            float newX = Mathf.Clamp(pos.x, wb.x + Size.width / 2, wb.x + wb.width - Size.width / 2);
            float newY = Mathf.Clamp(pos.y, wb.y + Size.height / 2, wb.y + wb.height - Size.height / 2);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }

        public void Move(Vector3 direction)
        {
            Direction = Vector3.Normalize(Direction + direction);
            Accelerate();
        }

        public void Accelerate()
        {
            CurrentSpeed += Acceleration * Time.deltaTime;
        }

        public void UpdatePosition()
        {
            CurrentSpeed *= (1 - Inertia);

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, Speed/100);

            transform.Translate(Direction * CurrentSpeed);
        }

        protected void Start()
        {
            Direction = Vector3.zero;
            CurrentSpeed = 0F;
            IsPlayable = true;
        }

        protected override void Update()
        {
            base.Update();
            UpdatePosition();
            LimitShipPositionWithinBoundaries();

            if (IsPlayable)
                Control();
        }

        public void Trigger(Slot slot)
        {
            if (slot != null && slot.Active)
                slot.Trigger();
        }

        public void Control()
        {
            if (Input.GetKey(KeyCode.S))
                Move(Vector3.down);

            if (Input.GetKey(KeyCode.Z))
                Move(Vector3.up);

            if (Input.GetKey(KeyCode.Q))
                Move(Vector3.left);

            if (Input.GetKey(KeyCode.D))
                Move(Vector3.right);

            if (Input.GetMouseButton(0)) 
                Trigger();

            if (Input.GetMouseButton(1))
                Trigger(Slots[1]);
        }

        public override string ToString()
        {
            return "PlayerShip";
        }

        public bool IsPlayable { get; set; }

        public override void Trigger()
        {
            Trigger(Slots[0]);
        }
    }
}