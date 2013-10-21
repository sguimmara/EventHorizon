using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Follow;

        void Update()
        {
            if (Follow != null)
                transform.position = new Vector3(Follow.position.x, transform.position.y, transform.position.z);

        }
    }
}
