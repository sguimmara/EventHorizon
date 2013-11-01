using EventHorizon.Core;
using EventHorizon.Effects;
using System.Collections;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Ship : Mobile
    {
        public EffectsContainer effects;

        public event EventMobile OnDestroy;

        public virtual void Trigger()
        {

        }

        public override void NotifyHitByLaser(LaserType type)
        {
            Debug.Log("Destroying " + gameObject.name);
            Destroy();
        }        

        public virtual void Destroy()
        {
            if (OnDestroy != null)
                OnDestroy(this);

            if (effects.Explosion != null)
                GameObject.Instantiate(effects.Explosion, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360F))));

            if (effects.ExplosionSound != null)
            {
                if (Engine.Instance != null)
                    Engine.Instance.audio.PlayOneShot(effects.ExplosionSound);
                else audio.PlayOneShot(effects.ExplosionSound);
            }

            Destroy(gameObject);
        }
    }
}