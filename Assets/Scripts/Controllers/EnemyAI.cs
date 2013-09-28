using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.AI
{
    public sealed class EnemyAI : MonoBehaviour
    {
        public static EnemyAI Instance { get; private set; }

        public void AddRandomEnemies(int number)
        {

        }

        void Awake()
        {
            Instance = this;
        }
    }
}
