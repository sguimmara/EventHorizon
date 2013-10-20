using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.Objects;
using EventHorizon.Core;

namespace EventHorizon.Objects
{
    [RequireComponent(typeof(Laser))]
    public class Player : Ship, IPlayable, IMovable
    {
        [HideInInspector]
        public Vector3 Direction { get; set; }

        public float Acceleration = 1;
        public float Inertia = 0;
        bool isAccelerating = false;

        [HideInInspector]
        public float CurrentSpeed;
        public float Speed;

        Laser laser;

        bool LaserActive;
        bool ShieldActive;

        Animator animator;

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
            isAccelerating = true;
        }

        public void UpdatePosition()
        {
            if (!isAccelerating)
                CurrentSpeed *= (1 - Inertia);

            isAccelerating = false;

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, Speed / 100);

            if (CurrentSpeed < 0.01F)
            {
                Direction = Vector3.zero;
            }
            //transform.position = Vector3.SmoothDamp(transform.position, transform.position += (Direction * CurrentSpeed), ref nullVector, 1F);
            //transform.position = transform.position += (Direction * CurrentSpeed);
            transform.Translate(Direction * CurrentSpeed, Space.World);
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (!ShieldActive)
                base.OnTriggerEnter(other);
        }

        public override void NotifyHitByLaser(LaserType type)
        {
            if (!ShieldActive || type == LaserType.PierceThroughShield)
                Destroy();
        }

        private void ActivateShield()
        {
            animator.SetBool("Shield", true);
            laser.Stop();
            gameObject.layer = 11; // Reflective
            ShieldActive = true;
        }

        private void TurnOffShield()
        {
            animator.SetBool("Shield", false);
            ShieldActive = false;
            gameObject.layer = 8; // Player
        }

        private void ActivateLaser()
        {
            laser.Trigger();
        }

        private void TurnOffLaser()
        {
            laser.Stop();
        }

        protected void Start()
        {
            animator = transform.Find("DodecahedronShip").gameObject.GetComponent<Animator>();
            laser = gameObject.GetComponent<Laser>();

            enabled = true;
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
            if (true)
            {
                if (Input.GetKey(KeyCode.S))
                    Move(Vector3.down);

                if (Input.GetKey(KeyCode.Z))
                    Move(Vector3.up);

                if (Input.GetKey(KeyCode.Q))
                    Move(Vector3.left);

                if (Input.GetKey(KeyCode.D))
                    Move(Vector3.right);
            }

            if (Input.GetMouseButton(0))
            {
                if (!ShieldActive)
                {
                    ActivateLaser();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                TurnOffLaser();
            }

            if (Input.GetMouseButtonDown(1))
            {
                TurnOffLaser();
                ActivateShield();
            }

            if (Input.GetMouseButtonUp(1))
            {
                TurnOffShield();
            }
        }

        public override string ToString()
        {
            return "PlayerShip";
        }

        public bool IsPlayable { get; set; }

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