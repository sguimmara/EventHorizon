using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.Data
{
    [System.Serializable]
    public class Properties
    {
        public bool Indestructible;
        [HideInInspector]
        public int currentHP;
        public int maxHP;
        public int damage;
        //public Color trailColor;
        //public float trailSize;
    }
}
