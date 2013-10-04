using EventHorizon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Weapon : Usable
    {
        public bool AutoFire;
        WeaponPart[] subWeapons;

        public override void Trigger()
        {           
            for (int i = 0; i < subWeapons.Length; i++)
            {
                subWeapons[i].Trigger();
            }
        }

        public override void Initialize()
        {
            subWeapons = GetComponentsInChildren<WeaponPart>();
            if (subWeapons == null || subWeapons.Length == 0)
                Debug.LogWarning(gameObject.name + " WeaponGroup empty");

            else
            {
                for (int i = 0; i < subWeapons.Length; i++)
                    subWeapons[i].Initialize();
            }
        }

        void Update()
        {

            if (AutoFire)
                Trigger();
        }
    }
}