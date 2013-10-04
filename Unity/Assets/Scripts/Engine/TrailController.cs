using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.FX
{
    // Changes size of trail according to speed. 
    // The greater the speed on the +X axis, the bigger.
    // The greater the speed on the -X axis, the smaller.
    public class TrailController : MonoBehaviour
    {
        float lastPosX;

        void Update()
        {
            float x = transform.position.x;
            float xVelocity = (x - lastPosX)*5F;
            lastPosX = x;

            xVelocity = xVelocity + 1;

            xVelocity = Mathf.Clamp(xVelocity, 0.4F, 2F);

            transform.localScale = new Vector3(xVelocity, UnityEngine.Random.Range(0.9F, 1.1F), 1);
        }

        void OnBecameVisible()
        {
            lastPosX = transform.position.x;
            enabled = true;
        }
    }
}
