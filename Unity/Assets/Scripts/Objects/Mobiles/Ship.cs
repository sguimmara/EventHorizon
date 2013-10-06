using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.FX;
using EventHorizon.Core;

namespace EventHorizon.Objects
{
    public class Ship : Mobile, ICollidable
    {
        public bool AutoTrigger;

        public Slot[] Slots;

        public Usable TEMP;

        public Slot PrimarySlot;
        public Slot SecondarySlot;
        public Slot HullSlot;
        public Slot EngineSlot;

        public Transform PrimarySlotLocation;
        public Transform SecondarySlotLocation;
        public Transform HullSlotLocation;
        public Transform EngineSlotLocation;
        
        public Effects effects;

        public event EventMobile OnDestroy;

        protected int currentHp;
        public int maxHp;

        public void Trigger()
        {
            if (PrimarySlot.Active)
                PrimarySlot.Trigger();
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

            PrimarySlot = new Slot(PrimarySlotLocation, SlotType.Primary);
            SecondarySlot = new Slot(SecondarySlotLocation, SlotType.Secondary);
            HullSlot = new Slot(HullSlotLocation, SlotType.Hull);
            EngineSlot = new Slot(EngineSlotLocation, SlotType.Engine);

            Slots = new Slot[4] { PrimarySlot, SecondarySlot, HullSlot, EngineSlot };

            foreach (Slot slot in Slots)
            {
                if (slot != null && slot.Content != null)
                {
                    slot.Initialize();
                    slot.Active = true;
                }
            }

            PrimarySlot.Set(TEMP);
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
            Destroy();
        }

        public void Damage(IHarmful other)
        {
            currentHp -= (other.Damage);
        }

        public void OnTriggerEnter(Collider other)
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