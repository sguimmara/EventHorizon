using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EventHorizon.Core
{
    public class MouseController : MonoBehaviour
    {
        Vector3 worldPos;
        Vector3 velocity = Vector3.zero;
        Vector3 target;

        // Use this for initialization
        void Start()
        {
            Screen.showCursor = false;
        }

        // Update is called once per frame
        void Update()
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));

            target = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, 0.3F);

            transform.position = target;
            transform.LookAt(worldPos, Vector3.right);
        }
    }
}
