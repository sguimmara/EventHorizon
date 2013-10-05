using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    public static class AimController
    {
        public static float GetRotation(Transform t)
        {
            Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 obj = Camera.main.WorldToScreenPoint(t.position);
           
            Vector2 vector = mouse - obj;

            float diff = mouse.y - obj.y;
            float angle = Vector3.Angle(Vector3.right, vector);
            angle = diff > 0 ? angle : 360 - angle;

            return angle;
        }
    }
}
