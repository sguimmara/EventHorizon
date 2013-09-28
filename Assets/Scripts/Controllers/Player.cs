using UnityEngine;

namespace EventHorizonGame
{
    public class Player : MonoBehaviour
    {
        PlayerShip ship;
        EventHorizon eventHorizon;

        public void SetShip<T>() where T : PlayerShip
        {
            if (ship)
                Destroy(ship);

            ship = gameObject.AddComponent<T>();
            ship.LoadDefaultModel();
            ship.motionParams = new MotionParameters { MaxSpeed = 10, Inertia = 0.9F, Acceleration = 2F, Velocity = Vector3.zero, CurrentSpeed = 0};
        }

        // Use this for initialization
        void Start()
        {
            eventHorizon = GetComponent<EventHorizon>();

            SetShip<PlayerShip>();

            eventHorizon.AddMobile(ship);
        }

        // Update is called once per frame
        void Update()
        {
            //ship.Stop();

            if (Input.GetKey(KeyCode.DownArrow))
            {
                ship.Move(Vector3.down);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                ship.Move(Vector3.up);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                ship.Move(Vector3.left);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                ship.Move(Vector3.right);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                ship.FirePrimary(ship.Model.transform.position);
            }
        }
    }
}
