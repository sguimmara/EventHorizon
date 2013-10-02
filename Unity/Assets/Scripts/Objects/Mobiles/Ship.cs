using UnityEngine;
using System.Collections;
using EventHorizon.Objects;
using EventHorizon.FX;

namespace EventHorizon.Objects
{
    public class Ship : Mobile, ICollidable
    {
        public Usable[] Slots;
        public bool AutoTrigger;
        public SpriteSlots Sprites;

        public event EventMobile OnDestroy;

        protected int currentHp;
        public int maxHp;

        public void Trigger()
        {
            for (int i = 0; i < Slots.Length; i++)
                if (Slots[i].Active)
                {
                    Slots[i].Trigger();
                }
                else Debug.LogWarning(string.Concat("Slot ", i.ToString(), " is null"));
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

            foreach (Usable slot in Slots)
            {
                slot.Initialize();
                slot.Active = true;
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
            currentHp -= (other.CurrentHp);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (this.tag != other.tag)
            {
                Collide(other as ICollidable);
            }
        }

        public void Destroy()
        {
            if (OnDestroy != null)
                OnDestroy(this);

            if (Sprites.Explosion != null)
                GameObject.Instantiate(Sprites.Explosion, transform.position, Quaternion.identity);

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