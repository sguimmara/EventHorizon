using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public struct MotionParameters
    {
        public Vector3 Velocity;
        public float MaxSpeed;
        public float Acceleration;
        public float Inertia;
        public float CurrentSpeed;
    }
}
