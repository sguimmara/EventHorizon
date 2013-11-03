using EventHorizon.Core;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Player : Ship, IMovable
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

        public GameObject ShieldObj { get; private set; }
        public GameObject ShipObj { get; private set; }
        public GameObject LaserObj { get; private set; }
        public GameObject BeaconObj { get; private set; }
        //public GameObject LaserCannon;
        //private GameObject PreviousLaserCannon;

        protected void Start()
        {
            ShieldObj = transform.Find("Dodecahedron shield").gameObject;
            ShipObj = transform.Find("Player ship").gameObject;
            LaserObj = transform.Find("PlayerLaser").gameObject;
            BeaconObj = transform.Find("Beacon").gameObject;

            laser = LaserObj.GetComponent<Laser>();

            enabled = true;
            Direction = Vector3.zero;
            CurrentSpeed = 0F;
            IsPlayable = true;
        }

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

        public override void NotifyHitByLaser(Laser source)
        {
            if (!ShieldActive || source.IgnoreShield)
                Destroy();
        }

        private void ActivateShield()
        {
            //Inertia = 0F;
            ShieldObj.SetActive(true);
            ShipObj.SetActive(false);
            BeaconObj.SetActive(false);
            laser.Stop();
            gameObject.layer = 11; // Reflective
            //ShieldActive = true;
        }

        private void TurnOffShield()
        {
            Inertia = 0.5F;
            ShieldObj.SetActive(false);
            ShipObj.SetActive(true);
            BeaconObj.SetActive(true);
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

        protected void Update()
        {
            UpdatePosition();

            if (IsPlayable)
                Control();
        }

        public void Control()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

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

        //private void DropLaserCannon()
        //{
        //    if (PreviousLaserCannon != null)
        //        Destroy(PreviousLaserCannon);

        //    PreviousLaserCannon = GameObject.Instantiate(LaserCannon, transform.position, transform.rotation) as GameObject;
        //    PreviousLaserCannon.transform.Find("Laser").GetComponent<Laser>().autoTrigger = true;
        //}

    }
}

