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
        public AudioClip sound;
        WeaponPart[] subWeapons;

        bool notPlayed = true;

        void Awake()
        {
			if (transform.parent == null)
				Initialize();

            if (audio == null)
                gameObject.AddComponent<AudioSource>();
        }

        public override bool Trigger()
        {
            notPlayed = true;
            for (int i = 0; i < subWeapons.Length; i++)
            {
                if (subWeapons[i].Trigger() && notPlayed)
                {
                    notPlayed = false;
                    audio.PlayOneShot(sound);
                }
            }
            return true;
        }

        public override void Initialize()
        {
            //enabled = false;
            subWeapons = GetComponentsInChildren<WeaponPart>();
            if (subWeapons == null || subWeapons.Length == 0)
                Debug.LogWarning(gameObject.name + " WeaponGroup empty");

            else
            {
                for (int i = 0; i < subWeapons.Length; i++)
                    subWeapons[i].Initialize();
            }
        }

    }
}