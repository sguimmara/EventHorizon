using EventHorizonGame.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public class MobileArgs : EventArgs
    {
        public Mobile mobile;
        public string explosionEffect;
        public string shootEffect;

        //public MobileArgs(Mobile m)
        //{
        //    this.mobile = m;
        //}
    }

    public sealed class Pool : MonoBehaviour
    {
        public static Pool Instance { get; private set; }

        List<Mobile> mobiles;

        public event EventMobile OnMobileCreated;

        public T Create<T>(Vector3 position) where T : Mobile
        {
            return Create<T>("Default", position, "Mobiles/DefaultMobile");
        }

        public void CreateDecal(string name, Vector3 position, float duration, float scaleMin, float scaleMax)
        {
            Vector3 p = new Vector3(position.x + 0.5F, position.y, -0.1F);

            GameObject g = (GameObject)GameObject.Instantiate(Utils.Load<GameObject>(string.Concat("FX/", name)), p, Quaternion.identity);
            g.transform.parent = EventHorizon.Instance.mobileParent;
            float s = UnityEngine.Random.Range(scaleMin, scaleMax);
            g.transform.localScale = new Vector3(s, s, 1);


            Sprite sprite = g.GetComponent<Sprite>();
            if (sprite != null)
                sprite.Play();

            else StartCoroutine(InstantiateDecal(g, duration));
        }

        IEnumerator InstantiateDecal(GameObject decal, float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(decal);
        }

        public T Create<T>(string name, Vector3 position, string path) where T : Mobile
        {
            GameObject prefab = (GameObject)GameObject.Instantiate(Utils.Load<GameObject>(path));
            prefab.transform.parent = EventHorizon.Instance.mobileParent;

            prefab.transform.position = position;

            T mobile = prefab.GetComponent<T>();

            if (EventHorizon.Instance.USE_PLACEHOLDERS)
                prefab.renderer.material = EventHorizon.Instance.PLACEHOLDER;

            mobiles.Add(mobile);

            BoxCollider col = prefab.AddComponent<BoxCollider>();
            col.center = new Vector3(0, 0, 4);
            col.size = new Vector3(1, 1, 12);
            col.isTrigger = true;

            Rigidbody bod = prefab.AddComponent<Rigidbody>();
            bod.isKinematic = true;
            bod.freezeRotation = true;
            bod.useGravity = false;

            if (OnMobileCreated != null)
                OnMobileCreated(this, new MobileArgs { mobile = mobile, shootEffect = "", explosionEffect = "" });

            return mobile;
        }

        public void Stop()
        {
            for (int i = 0; i < mobiles.Count; i++)
            {
                if (mobiles[i] != null)
                    Destroy(mobiles[i].Model);
            }
            mobiles.Clear();
        }

        void Awake()
        {
            Instance = this;
            mobiles = new List<Mobile>();
        }
    }
}
