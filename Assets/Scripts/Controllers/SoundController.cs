using System;
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
        public List<AudioClip> musicTracks;
        //public Dictionary<string, AudioClip> effects;

        void Awake()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            effectSource = gameObject.AddComponent<AudioSource>();  
        }

        void Start()
        {
            EventHorizon.Instance.OnEnterScene += PlayMusic;
            EventHorizon.Instance.OnPoolLoaded += Initialize;
            PlayMusic(0);
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

        void PlayMusic(int level)
        {
            if (musicSource == null)
				Debug.LogWarning("musicsource null");
			

			
            musicSource.Stop();
            musicSource.clip = musicTracks[level];
			
						if (musicSource.clip == null)
				Debug.Log("clip null");
			
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
