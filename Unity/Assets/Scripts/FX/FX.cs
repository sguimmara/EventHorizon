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

        public abstract void Create(Transform parent);

        public abstract void Play();
    }
}