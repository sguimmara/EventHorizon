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
        int CurrentHp { get; }
        int MaxHp {get; set;}

        void Destroy();
        event EventMobile OnDestroy;
        void UpdateHp();

        void OnTriggerEnter(Collider other);
    }
}
