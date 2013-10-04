using EventHorizon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class WeaponPart : Usable
    {
        public float rateOfFire = 5;
        public Ammunition Ammunition;
        float OriginalRotation;
        public float Spread;

        protected float lastShot;

        protected override void Awake()
        {
            base.Awake();
            OriginalRotation = transform.rotation.eulerAngles.z;
        }

        public override void Trigger()
        {
            if (Time.time - lastShot >= (1 / rateOfFire))
            {
                Vector3 orig = transform.rotation.eulerAngles;
                float rot = UnityEngine.Random.Range(-Spread, Spread);


                orig.z = rot;
                transform.rotation = Quaternion.Euler(orig);
                Ammunition.Create(transform);
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
