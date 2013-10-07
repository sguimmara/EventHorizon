using UnityEngine;
using System.Collections;

namespace EventHorizon.Graphics
{
    // Generic class for visual effects
    public abstract class FX : MonoBehaviour, ICreatable
    {
        public bool PlayOnAwake;
        public float duration;
        public FXmode mode;

        public virtual void Create(Transform parent)
        {
            GameObject.Instantiate(gameObject, transform.position, transform.rotation);
        }

        public abstract void Play();
    }
}