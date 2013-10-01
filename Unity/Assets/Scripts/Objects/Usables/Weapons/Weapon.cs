using EventHorizonGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.Items
{
    public class Weapon : Usable
    {
        public bool AutoFire;
        WeaponPart[] weapons;
        public Texture2D Icon;

        public override void Trigger()
        {           
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Trigger();
            }
        }

        public override void Initialize()
        {
            weapons = GetComponentsInChildren<WeaponPart>();
            if (weapons == null || weapons.Length == 0)
                Debug.LogWarning(gameObject.name + " WeaponGroup empty");

            else
            {
                for (int i = 0; i < weapons.Length; i++)
                    weapons[i].Initialize();
            }
        }

        void Update()
        {

            if (AutoFire)
                Trigger();
        }
    }
}