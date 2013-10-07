using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using EventHorizon.Storyline;
using EventHorizon.Core;

namespace EventHorizon.Triggers
{
    public class MusicTrigger : TriggerBehaviour
    {
        public AudioClip Clip;
        public bool Loop;

        void Awake()
        {
            if (!audio)
                gameObject.AddComponent<AudioSource>();
        }

        protected override void Trigger()
        {
            if (!Loop)
                audio.PlayOneShot(Clip);

            else
            {
                audio.clip = Clip;
                audio.loop = Loop;
                audio.Play();
            }
        }
    }
}
