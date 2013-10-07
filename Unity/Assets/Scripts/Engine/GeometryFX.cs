using UnityEngine;
using System.Collections;


namespace EventHorizon.Graphics
{
    public abstract class GeometryFX : FX
    {
        public virtual void Create(Transform parent)
        {
            Vector3 position = parent.position;
            GameObject.Instantiate(gameObject, new Vector3(position.x, position.y, position.z - 0.1F), Quaternion.identity);
        }
    }
}