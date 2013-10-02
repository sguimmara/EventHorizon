﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.Sound
{
    public class SoundController : MonoBehaviour
    {
        float masterVolume = 1F;
        public static SoundController Instance { get; private set; }

        AudioSource musicSource;
        AudioSource effectSource;

        public List<AudioClip> effects;
        //public List<AudioClip> musicTracks;
        public Dictionary<string, AudioClip> musicTracks;

        void LoadMusics()
        {
            musicTracks.Add("Menu", Utils.Load<AudioClip>("Music/POL-evangelian-short"));
            musicTracks.Add("Main", Utils.Load<AudioClip>("Music/POL-impulses-short"));
            musicTracks.Add("EndCredits", musicTracks["Menu"]);
        }

        void Awake()
        {
            musicTracks = new Dictionary<string, AudioClip>();
            musicSource = gameObject.AddComponent<AudioSource>();
            effectSource = gameObject.AddComponent<AudioSource>();

            LoadMusics();
            EventHorizon.Instance.OnLevelLoaded += PlayMusic;
        }

        void Start()
        {
            EventHorizon.Instance.OnPoolLoaded += Initialize;
        }

        void Initialize()
        {
            Instance = this;
            Pool.Instance.OnMobileCreated += HookToNewMobile;  
        }

        void HookToNewMobile(object sender, MobileArgs args)
        {
            args.mobile.OnMobileExplosion += delegate { Play(args.explosionEffect); };
            args.mobile.OnMobileShoot += delegate { Play(args.shootEffect); };
        }

        void Play(string effectName)
        {
            effectSource.PlayOneShot(effects[1], masterVolume * 1F);
        }

        void PlayMusic(string name)
        {
            musicSource.Stop();
            musicSource.clip = musicTracks[name];
			
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}