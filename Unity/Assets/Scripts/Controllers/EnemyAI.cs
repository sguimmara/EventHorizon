using EventHorizonGame.Data;
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

        public bool IsRunning;

        Vector3 RandomSpawnPosition()
        {
            float y = UnityEngine.Random.Range(Globals.SpawnArea.y, Globals.SpawnArea.y + Globals.SpawnArea.height);
            float x = Globals.SpawnArea.x;

            return new Vector3(x, y, 0);
        }

        public void Run()
        {
                StartCoroutine(AddRandomEnemies(-1));
        }

        public void Stop()
        {
            StopAllCoroutines();
        }

        public IEnumerator AddRandomEnemies(int number)
        {
            int i = 0;

            IsRunning = true;
            while (i < number || number == -1)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5F, 1F));
                EnemyShip ship = Pool.Instance.Create<EnemyShip>("Enemy", RandomSpawnPosition(), "Mobiles/Ships/Enemy/Phantasm");
                ship.motionParams.MaxSpeed = UnityEngine.Random.Range(0.05F, 0.1F);
                i++;
            }
            IsRunning = false;
        }

        void Awake()
        {
            IsRunning = false;
            Instance = this;
        }
    }
}
