using UnityEngine;
using System.Collections;

namespace EventHorizon.Core
{
    public class InputController : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Screen.showCursor = false;
        }

        // Update is called once per frame
        void Update()
        {
            float val = 1;

            // Relative position
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, 0, -val/20F));
            }

            // Relative position
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, 0, val/20F));
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                val = 10;

            // Rotation
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                transform.Rotate(new Vector3(val, 0, 0));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                transform.Rotate(new Vector3(-val, 0, 0));
            }
        }
    }
}
