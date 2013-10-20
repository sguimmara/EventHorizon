using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;
using EventHorizon.AI;

namespace EventHorizon.Objects
{
    public class Ship : Mobile, ICollidable
    {
        public Slot[] Slots;

        public EffectsContainer effects;

        public event EventMobile OnDestroy;

        [HideInInspector]
        public bool isDestroying = false;

        protected int currentHp;
        public int maxHp;

        public virtual void Trigger()
        {

        }

        public virtual void NotifyHitByLaser(LaserType type)
        {
            Destroy();
        }

        protected override void Awake()
        {
            //base.Awake();

            maxHp = Mathf.Clamp(maxHp, 1, 10000);
            currentHp = maxHp;

            foreach (Slot slot in Slots)
            {
                if (slot != null && slot.Content != null)
                {
                    slot.Initialize();
                    slot.Active = true;
                }
            }
        }

        protected virtual void Update()
        {
            UpdateHp();
        }

        public void Collide(ICollidable other)
        {
            StartCoroutine(Flash());
            currentHp -= (other as Ship).maxHp;
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

        public void Damage(IHarmful other)
        {
            StartCoroutine(Flash());
            currentHp -= (other.Damage);
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            Mobile m = other.gameObject.GetComponent<Mobile>();

            if (other.tag == "Projectile" || other.tag == "Enemy" || other.tag == "Player")
                //Damage(m as IHarmful);
                Destroy();
                ;
            //else if (other.tag == "Enemy" || other.tag == "Player")
            //    Collide(m as ICollidable);
        }

        public virtual void Destroy()
        {
            if (OnDestroy != null)
                OnDestroy(this);

            isDestroying = true;

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

        public void UpdateHp()
        {
            if (!NeverDestroy && currentHp <= 0)
                Destroy();
        }

        public int CurrentHp
        {
            get { return currentHp; }
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();

            if (Engine.Instance != null)
                Engine.Instance.AddShip(this);
        }

        [SerializeField]
        public int MaxHp
        {
            get
            {
                return maxHp;
            }
            set
            {
                maxHp = Mathf.Clamp(value, 1, 10000);
            }
        }
    }
}