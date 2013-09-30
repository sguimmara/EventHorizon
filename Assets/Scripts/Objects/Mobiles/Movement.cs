using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.Data
{
    [Serializable]
    public class Movement
    {
        //[HideInInspector]
        //[NonSerialized]
        public Vector3 Velocity;
        public Vector3 Direction;
        public float MaxSpeed;
        public float Acceleration;
        public float Inertia;
        //[HideInInspector]
        public float CurrentSpeed;
    }
}
