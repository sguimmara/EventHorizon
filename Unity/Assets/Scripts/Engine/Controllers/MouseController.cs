using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EventHorizon.Core
{
    public class MouseController : MonoBehaviour
    {
        Vector3 worldPos;

        // Use this for initialization
        void Start()
        {
            Screen.showCursor = false;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(worldPos, 0.1F);
        }

        // Update is called once per frame
        void Update()
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 25F));            
            transform.LookAt(worldPos, transform.up);
        }
    }
}
