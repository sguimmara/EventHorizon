using UnityEngine;

namespace EventHorizonGame
{
    public class Player : MonoBehaviour
    {
        PlayerShip ship;

        // Use this for initialization
        void Start()
        {
            ship = Pool.Instance.Create<PlayerShip>("Player", EventHorizon.Instance.STARTING_POSITION, "Mobiles/Ships/Player/HarbingerOfDeath");
            ship.motionParams = new MotionParameters { Acceleration = 1F, CurrentSpeed = 0f, Velocity = Vector3.zero, MaxSpeed = 10F, Inertia = 0.9F };
            ship.data = new MobileData { damage = 0, hp = 5, isDestroyable = true };
        }

        // Update is called once per frame
        void Update()
        {
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
