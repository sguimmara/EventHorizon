using EventHorizon.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventHorizon.Objects
{
    class Crystal : Mobile, ICollidable
    {
        public event GameEvent OnActivated;
        public event GameEvent OnDeactivated;
        public bool Activated { get; private set; }
        private float originalShininess;
        private GameObject glow;

        private void Awake()
        {
            Material m = new Material(renderer.material);
            renderer.material = m;
            originalShininess = m.GetFloat("_Shininess");
            glow = transform.Find("crystal glow").gameObject;
            glow.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {

        }

        public void Collide(ICollidable other)
        {

        }

        public override void NotifyHitByLaser(Laser source)
        {
            Activated = true;
            if (OnActivated != null)
                OnActivated();
        }

        void Update()
        {
            if (Activated)
            {
                renderer.material.SetFloat("_Shininess", 0);

                if (!glow.activeSelf)
                    glow.SetActive(true);
            }

            else
            {
                renderer.material.SetFloat("_Shininess", originalShininess);

                if (glow.activeSelf)
                    glow.SetActive(false);
            }

            Activated = false;

            if (OnDeactivated != null)
                OnDeactivated();
        }
    }
}
