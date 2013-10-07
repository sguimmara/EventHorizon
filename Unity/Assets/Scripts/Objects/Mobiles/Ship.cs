using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.Effects;
using EventHorizon.Core;

namespace EventHorizon.Objects
{
    public class Ship : Mobile, ICollidable
    {
        public bool AutoTrigger;

        public Slot[] Slots;

        public Slot PrimarySlot;
        public Slot SecondarySlot;
        public Slot HullSlot;
        public Slot EngineSlot;

        public Transform PrimarySlotLocation;
        public Transform SecondarySlotLocation;
        public Transform HullSlotLocation;
        public Transform EngineSlotLocation;

        public EffectsContainer effects;

        public event EventMobile OnDestroy;

        protected int currentHp;
        public int maxHp;

        public void Trigger()
        {
            Trigger(PrimarySlot);
        }

        public void Trigger(Slot slot)
        {
            if (slot.Active)
                slot.Trigger();
        }

        public override string ToString()
        {
            return "Ship";
        }

        protected override void Awake()
        {
            base.Awake();

            maxHp = Mathf.Clamp(maxHp, 1, 10000);
            currentHp = maxHp;

            PrimarySlot.Type = SlotType.Primary;
            SecondarySlot.Type = SlotType.Secondary;
            HullSlot.Type = SlotType.Hull;
            EngineSlot.Type = SlotType.Engine;

            PrimarySlot.location = PrimarySlotLocation;
            SecondarySlot.location = SecondarySlotLocation;
            HullSlot.location = HullSlotLocation;
            EngineSlot.location = EngineSlotLocation;

            Slots = new Slot[4] { PrimarySlot, SecondarySlot, HullSlot, EngineSlot };

            foreach (Slot slot in Slots)
            {
                if (slot != null && slot.Content != null)
                {
                    slot.Initialize();
                    slot.Active = true;
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (AutoTrigger)
                Trigger();

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
                m.SetFloat("_Flash",Mathf.Clamp01(1 - f / 0.01F));
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

            if (other.tag == "Projectile")
                Damage(m as IHarmful);
            else if (other.tag == "Enemy" || other.tag == "Player")
                Collide(m as ICollidable);
        }

        public void Destroy()
        {
            if (OnDestroy != null)
                OnDestroy(this);

            if (effects.Explosion != null)
            {
                GameObject.Instantiate(effects.Explosion, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360F))));
                Engine.Instance.audio.PlayOneShot(effects.ExplosionSound);
            }

            Destroy(gameObject);
        }

        public void UpdateHp()
        {
            if (currentHp <= 0)
                Destroy();
        }

        public int CurrentHp
        {
            get { return currentHp; }
        }

        protected override void OnBecameVisible()
        {
            base.OnBecameVisible();
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