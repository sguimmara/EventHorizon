using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.AI
{
    [Serializable]
    public abstract class AIMotionPattern : AIBehaviour, ICreatable
    {
        public bool Loop;

        [HideInInspector]
        public Transform objectToMove;

        public abstract void Play();

        public void Create(Transform parent)
        {
            GameObject g = (GameObject) GameObject.Instantiate(gameObject, parent.position, parent.rotation);
            AIMotionPattern m = g.GetComponent<AIMotionPattern>();
            m.objectToMove = parent;
            //g.transform.parent = parent.root;
            m.Play();
        }
    }
}
