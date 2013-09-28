using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.AI
{
    public sealed class EnemyAI : MonoBehaviour
    {
        public static EnemyAI Instance { get; private set; }

        Vector3 RandomSpawnPosition()
        {
           float y = UnityEngine.Random.Range(Globals.SpawnArea.y, Globals.SpawnArea.y + Globals.SpawnArea.height);
           float x = Globals.SpawnArea.x;

           return new Vector3(x, y, 0);
        }

        public void Run()
        {
            StartCoroutine(AddRandomEnemies());
        }

        public IEnumerator AddRandomEnemies()
        {
            MotionParameters p = new MotionParameters { Acceleration = 1, CurrentSpeed = 0, Velocity = Vector3.left, Inertia = 1F, MaxSpeed = 0.05F };
            MobileData d = new MobileData { damage = 0, hp = 5, isDestroyable = true };

            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5F, 1F));
                EnemyShip ship = Pool.Instance.Create<EnemyShip>("Enemy", RandomSpawnPosition(), "Mobiles/Ships/Enemy/Testaros");
                ship.motionParams = p;
                ship.data = d;
            }
        }

        void Awake()
        {
            Instance = this;
        }
    }
}
