using EventHorizon.Core;
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
        Vector3 rotation;

        void Awake()
        {
            rotation = transform.rotation.eulerAngles;
        }

        public override void Trigger()
        {
            rotation.z = AimController.GetRotation(transform);

            transform.rotation = Quaternion.Euler(rotation);

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