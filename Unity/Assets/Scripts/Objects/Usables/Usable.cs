using UnityEngine;
using System.Collections;

namespace EventHorizon.Objects
{
    public abstract class Usable : MonoBehaviour
    {
        public abstract void Trigger();
        public abstract void Initialize();
        public Texture2D Icon;
        SlotType type;
    }
}
