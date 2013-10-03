using EventHorizon.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Ammunition : Mobile, ICollidable, IHarmful
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
                //GameObject.Instantiate(Impact.gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360F))));
                Impact.Create(transform.position);

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
    }
}
