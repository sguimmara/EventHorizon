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

        Vector3 nullVector;

        Laser laser;
        public GameObject Shield;

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

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, Speed / 100);
            //transform.position = Vector3.SmoothDamp(transform.position, transform.position += (Direction * CurrentSpeed), ref nullVector, 1F);
            //transform.position = transform.position += (Direction * CurrentSpeed);
            transform.Translate(Direction * CurrentSpeed, Space.World);
        }

        protected void Start()
        {
            Direction = Vector3.zero;
            CurrentSpeed = 0F;
            IsPlayable = true;
            laser = gameObject.GetComponent<Laser>();
        }

        protected override void Update()
        {
            base.Update();
            UpdatePosition();
            LimitShipPositionWithinBoundaries();

            if (IsPlayable)
                Control();
        }

        public void Trigger(int slot)
        {
            if (Slots.Length >= slot + 1)
            {
                if (Slots[slot] != null && Slots[slot].Active)
                    Slots[slot].Trigger();
            }
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

            if (Input.GetMouseButton(0) && !Shield.activeInHierarchy)
                CastLaser();

            if (Input.GetMouseButtonUp(0))
                Stop();

            if (Input.GetMouseButtonDown(1))
            {
                Shield.SetActive(true);
                //collider.enabled = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                //collider.enabled = true;
                Shield.SetActive(false);
            }
        }

        public override string ToString()
        {
            return "PlayerShip";
        }

        public bool IsPlayable { get; set; }

        void Stop()
        {
            laser.Stop();
        }

        void CastLaser()
        {
            laser.Trigger();
        }

        public override void Trigger()
        {
            laser.Trigger();
        }
    }
}