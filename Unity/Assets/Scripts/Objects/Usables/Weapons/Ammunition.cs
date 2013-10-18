using EventHorizon.Core;
using EventHorizon.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Ammunition : Mobile, ICollidable, IHarmful, ICreatable, IMovable
    {
        public Sprite Impact;

        public int damage = 1;

        public int Damage
        {
            get
            {
                return damage;
            }
        }

        public event EventMobile OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            Direction = transform.right;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" || other.tag == "Enemy")
                Collide(other as ICollidable);

            else Destroy();
        }

        public void Collide(ICollidable other)
        {
            Destroy();
        }

        public void Destroy()
        {
            Destroy(gameObject);

            if (Impact != null)
                Impact.Create(transform);

            if (OnDestroy != null)
                OnDestroy(this);
        }

        public void Create(Transform parent)
        {
            GameObject g = (GameObject)GameObject.Instantiate(gameObject, parent.position, parent.localRotation);
            g.transform.rotation = parent.rotation;
        }

        void Update()
        {
            UpdatePosition();
        }

        public float Speed;

        public Vector3 Direction { get; set; }

        public void UpdatePosition()
        {
            transform.Translate(Direction * Speed / 100);
        }
    }
}
