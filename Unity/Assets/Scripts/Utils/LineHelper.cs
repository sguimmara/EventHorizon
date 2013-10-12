using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Helpers
{
    public class LineHelper : MonoBehaviour
    {
        public float Length = 10F;

        void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.right * Length);
        }
    }
}
