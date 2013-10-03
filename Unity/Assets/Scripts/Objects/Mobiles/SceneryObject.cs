using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EventHorizon;
using EventHorizon.Graphics;
using EventHorizon.FX;

namespace EventHorizon.Objects
{
    public class SceneryObject : Mobile
    {
        public float Rotation;

        protected override void UpdatePosition()
        {
            transform.Translate(Direction * CurrentSpeed, Space.World);
            transform.Rotate(Vector3.forward, Rotation);
        }
    }
}
