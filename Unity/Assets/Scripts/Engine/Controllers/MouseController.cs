using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EventHorizon.Core
{
    public class MouseController : MonoBehaviour
    {
        Vector3 worldPos;
        public float depth;
        Vector3 originalRotation;

        // Use this for initialization
        void Start()
        {
            originalRotation = transform.eulerAngles;
            //Screen.showCursor = false;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(worldPos, 0.1F);
        }

        // Update is called once per frame
        void Update()
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
            transform.LookAt(worldPos, transform.up);

            //transform.rotation = Quaternion.Euer(transform.rotation.eulerAngles.x, originalRotation.y, originalRotation.z);
        }
    }
}
