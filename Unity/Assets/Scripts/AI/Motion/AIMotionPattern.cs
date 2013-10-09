using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.AI
{
    public class AIMotionPattern : AIBehaviour, ICreatable
    {
        [HideInInspector]
        public Transform objectToMove;

        public void Create(Transform parent)
        {
            GameObject g = (GameObject) GameObject.Instantiate(gameObject, parent.position, parent.rotation);
            AIMotionPattern m = g.GetComponent<AIMotionPattern>();
            m.objectToMove = parent;
            g.transform.parent = parent.root;
        }
    }
}
