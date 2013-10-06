using EventHorizon.Objects;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    public class LevelSlider : MonoBehaviour
    {
        public static LevelSlider Instance;
        public int speed;
        public SceneryObject asteroid;

        void Update()
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }        

        public IEnumerator AddRandomSceneryObjects()
        {
            while (true)
            {                
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5F, 1F));

                Vector3 pos = new Vector3(Globals.SpawnArea.x, Random.Range(Globals.SpawnArea.yMin, Globals.SpawnArea.yMax), Random.Range(-10F, 10F));

                //Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360F));
                GameObject c = (GameObject) GameObject.Instantiate(asteroid.gameObject, pos, Quaternion.identity );
                c.transform.parent = transform;
                c.transform.localScale *= Random.Range(0.05F, 0.7F);

                SceneryObject m = c.GetComponent<SceneryObject>();
                m.Inertia = 0;
                m.Direction = Vector3.left;
                m.Speed = Random.Range(0.001F, 0.01F);
                m.CurrentSpeed = m.Speed;
                m.Acceleration = 100;
                m.Rotation = Random.Range(-3, 3);
            }
        }

        void Awake()
        {
            Instance = this;
            StartCoroutine(AddRandomSceneryObjects());
        }
    }
}
