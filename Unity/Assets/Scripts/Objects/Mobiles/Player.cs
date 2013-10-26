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

        public float CurrentSpeed;
        public float Speed;

        Laser laser;

        bool LaserActive;
        bool ShieldActive;

        Animator animator;

        public void Move(Vector3 direction)
        {
            rigidbody.AddForce(direction * Acceleration, ForceMode.Impulse);
            Accelerate();
        }

        public void Accelerate()
        {
            isAccelerating = true;
        }

        public void UpdatePosition()
        {
            if (!isAccelerating)
            {
                Direction *= (1 - Inertia);
                rigidbody.velocity *= (1 - Inertia);
            }

            isAccelerating = false;
            CurrentSpeed = rigidbody.velocity.magnitude;
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
            Inertia = 0F;
            animator.SetBool("Shield", true);
            laser.Stop();
            gameObject.layer = 11; // Reflective
            ShieldActive = true;
        }

        private void TurnOffShield()
        {
            Inertia = 0.5F;
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

        protected void Update()
        {
            UpdatePosition();
            //LimitShipPositionWithinBoundaries();

            if (IsPlayable)
                Control();
        }

        public void Control()
        {
            if (!ShieldActive)
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

