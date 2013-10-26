using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;
using EventHorizon.AI;

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

        IEnumerator Flash()
        {
            StopCoroutine("Flash");
            Material m = renderer.material;

            float f = 0;

            while (f < 0.04F)
            {
                m.SetFloat("_Flash", Mathf.Clamp01(f / 0.01F));
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            while (f < 0.08F)
            {
                m.SetFloat("_Flash", Mathf.Clamp01(1 - f / 0.01F));
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            Mobile m = other.gameObject.GetComponent<Mobile>();

            if (other.tag == "Projectile" || other.tag == "Enemy" || other.tag == "Player")
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