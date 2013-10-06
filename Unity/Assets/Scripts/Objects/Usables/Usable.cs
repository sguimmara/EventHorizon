using UnityEngine;
using System.Collections;

namespace EventHorizon.Objects
{
    public abstract class Usable : MonoBehaviour
    {
        public abstract bool Trigger();
        public abstract void Initialize();
        public Texture2D Icon;
        SlotType type;

        protected virtual void OnBecameVisible()
        {
            enabled = true;
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
