using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Ammunition : Mobile, ICollidable
    {
        public event EventMobile OnDestroy;

        public void Collide(ICollidable other)
        {
            Destroy();
        }

        // Destroy the mobile when its rectangle is *totally* out of Spawn area.
        protected override void DestroyWhenOutOfVoidArea()
        {
            if (this.transform.position.x < Globals.GameArea.x - Size.width / 2
                || this.transform.position.x > Globals.GameArea.x + Globals.GameArea.width + Size.width / 2
                || this.transform.position.y < Globals.GameArea.y - Size.height / 2
                || this.transform.position.y > Globals.GameArea.y + Globals.GameArea.height + Size.height / 2)
            {
                Destroy(this.gameObject);
            }
        }

        public int CurrentHp
        {
            get { throw new NotImplementedException(); }
        }

        public int MaxHp
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
            //GameObject.Instantiate(Sprites.Explosion.gameObject, transform.position, Quaternion.identity);
            if (OnDestroy != null)
                OnDestroy(this);
        }

        public void UpdateHp()
        {
            throw new NotImplementedException();
        }

        public void OnTriggerEnter(Collider other)
        {
            Collide(other as ICollidable);
        }
    }
}
