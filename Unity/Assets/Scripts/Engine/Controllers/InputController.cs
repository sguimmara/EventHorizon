using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EventHorizon.Core
{
    public class InputController : MonoBehaviour
    {
        Vector3 worldPos;
        public float depth;
        Vector3 originalRotation;

        // Use this for initialization
        void Start()
        {
            originalRotation = transform.eulerAngles;
            Screen.showCursor = false;
        }

        // Update is called once per frame
        void Update()
        {
             //Absolute position
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
            Vector3 velocity = new Vector3();
            Vector3 result = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, 0.1F);
            transform.position = new Vector3(result.x, result.y, -1F) ;

            float val = 1;

            // Relative position
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, 0, -val));
            }

            // Relative position
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, 0, val));
            }

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                val = 10;

            // Rotation
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(val, 0, 0));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(-val, 0, 0));
            }
        }
    }
}
