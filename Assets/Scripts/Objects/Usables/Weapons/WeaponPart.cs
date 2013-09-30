using EventHorizonGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.Items
{
    public class WeaponPart : Usable
    {
        public float rateOfFire = 5;
        public Ammunition Ammunition;

        protected float lastShot;

        public override void Trigger()
        {
            if (Time.time - lastShot >= (1 / rateOfFire))
            {
                GameObject.Instantiate(Ammunition, transform.position, transform.rotation);
                lastShot = Time.time;
            }
        }

        public override void Initialize()
        {
            lastShot = Time.time;
            renderer.enabled = false;
        }
    }

}
