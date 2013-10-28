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

        Vector3 originalMousePosition;
        Vector3 delta;

        // Use this for initialization
        void Start()
        {
            originalMousePosition = Input.mousePosition;
            originalRotation = transform.eulerAngles;
            Screen.showCursor = false;
        }

        void OnGUI()
        {
            string s = string.Format("{0} - {1}", originalMousePosition, delta);
            GUI.Label(new Rect(0, 0, 300, 20), s);
        }

        // Update is called once per frame
        void Update()
        {
            //// Absolute position
            //Vector3 newMousePosition = Input.mousePosition - originalMousePosition;
            //delta = new Vector3(newMousePosition.x, newMousePosition.y, 0);
            //originalMousePosition = Input.mousePosition;

            //transform.Translate(delta / 100F, Space.World);

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
