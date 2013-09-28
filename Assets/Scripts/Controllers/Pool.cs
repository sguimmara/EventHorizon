using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public sealed class Pool : MonoBehaviour
    {
        public static Pool Instance { get; private set; }

        public T Create<T>(Vector3 position) where T : Mobile
        {
            return Create<T>("Default", position, "Mobiles/DefaultMobile");
        }

        public void CreateDecal(string name, Vector3 position, float duration)
        {
            Vector3 p = new Vector3(position.x + 0.5F, position.y, -0.1F);

            GameObject g = (GameObject)GameObject.Instantiate(Utils.Load<GameObject>(string.Concat("FX/", name)), p, Quaternion.identity);
            g.transform.parent = EventHorizon.Instance.mobileParent;
            float s = UnityEngine.Random.Range(0.5F, 1.5F);
            g.transform.localScale = new Vector3(s, s, 1);
            StartCoroutine(InstantiateDecal(g , duration));
        }

        IEnumerator InstantiateDecal(GameObject decal, float duration)
        {            
            yield return new WaitForSeconds(duration);
            Destroy(decal);
        }        

        public T Create<T>(string name, Vector3 position, string path) where T : Mobile
        {
            GameObject g = (GameObject)GameObject.Instantiate(Utils.Load<GameObject>(path));
            g.transform.parent = EventHorizon.Instance.mobileParent;
            T[] comp = g.GetComponents<T>();

            g.transform.position = position;

            T mobile = g.AddComponent<T>();
            mobile.SetModel(g);

            BoxCollider col = g.AddComponent<BoxCollider>();
            col.center = new Vector3(0, 0, 4);
            col.size = new Vector3(1, 1, 12);
            col.isTrigger = true;

            Rigidbody bod = g.AddComponent<Rigidbody>();
            bod.isKinematic = true;
            bod.freezeRotation = true;
            bod.useGravity = false;

            CollisionTester collisionTester = g.GetComponent<CollisionTester>();

            if (collisionTester == null)
                collisionTester = g.AddComponent<CollisionTester>();

            collisionTester.parent = mobile;

            return mobile;
        }

        void Awake()
        {
            Instance = this;
        }
    }
}
