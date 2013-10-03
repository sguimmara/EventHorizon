using EventHorizon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon
{
    public interface ICollidable
    {
        void OnTriggerEnter(Collider other);
        void Collide(ICollidable other);
    }
}
