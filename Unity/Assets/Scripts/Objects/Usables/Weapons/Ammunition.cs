using EventHorizon.Core;
using EventHorizon.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Ammunition : Mobile, ICollidable, IHarmful, ICreatable
    {
        public Sprite Impact;

        public int damage = 1;

        public event EventMobile OnDestroy;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" || other.tag == "Enemy")
                Collide(other as ICollidable);
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

        public int Damage
        {
            get
            {
                return damage;
            }
        }

        public void Create(Transform parent)
        {
            GameObject g = (GameObject)GameObject.Instantiate(gameObject, parent.position, parent.localRotation);
            g.transform.rotation = parent.rotation;
            g.transform.parent = parent.root;
        }
    }
}
