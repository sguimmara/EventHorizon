using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public enum LaserType { Normal, Reflected, PierceThroughShield };
    public class Laser : MonoBehaviour
    {
        public LaserType type;
        public Material laserMat;
        List<Vector3> laserHits = new List<Vector3>();
        float lastShot = 0;
        const float cooldown = 0.05F;
        const int bounceLimit = 100;
        Vector3 lastBounce;
        bool firstRay = true;
        public bool autoTrigger = false;

        public LayerMask bouncingLayer;
        public LayerMask hitLayer;

        LineRenderer[] lineRenderers;
        List<Ship> MarkedDestroyed;

        void Awake()
        {
            lineRenderers = new LineRenderer[bounceLimit + 1];
            MarkedDestroyed = new List<Ship>();

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i] = (new GameObject()).AddComponent<LineRenderer>();
                lineRenderers[i].SetWidth(0.0F, 0.0F);
                lineRenderers[i].material = laserMat;
            }
        }

        void CheckForShipHits()
        {
            RaycastHit[] hits;
            MarkedDestroyed.Clear();

            for (int i = 0; i < laserHits.Count - 1; i++)
            {
                hits = Physics.RaycastAll(new Ray(laserHits[i], laserHits[i + 1] - laserHits[i]), Vector3.Distance(laserHits[i], laserHits[i+1]), hitLayer);
                if (hits == null || hits.Length == 0)
                    continue;

                foreach (RaycastHit rh in hits)
                {
                    if (rh.transform.tag == "Enemy" || rh.transform.tag == "Player")
                    {
                        Ship es = rh.transform.gameObject.GetComponent<Ship>();
                        if (!MarkedDestroyed.Contains(es))
                            MarkedDestroyed.Add(es);
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            Color c = Color.white;
            switch (type)
            {
                case LaserType.Normal: c = Color.blue;
                    break;
                case LaserType.Reflected: c = Color.red;
                    break;
                case LaserType.PierceThroughShield: c = Color.cyan;
                    break;
                default:
                    break;
            }
            Gizmos.color = c;
            Gizmos.DrawRay(transform.position, transform.forward * 20);
        }

        void CastLaser(Ray r)
        {
            RaycastHit hit;
            if (laserHits.Count <= bounceLimit)
            {
                laserHits.Add(r.origin);

                if (Physics.Raycast(r, out hit, float.MaxValue, bouncingLayer))
                {
                    Vector3 v = Vector3.Normalize(r.direction);
                    float dot = Vector3.Dot(v, hit.normal);
                    Vector3 bounce = v - (2 * hit.normal * (Vector3.Dot(v, hit.normal)));
                    lastBounce = bounce;
                    Debug.DrawLine(r.origin, hit.point);

                    firstRay = false;

                    if (type == LaserType.Normal || hit.transform.gameObject.layer != LayerMask.NameToLayer("Reflective"))
                    {
                        laserHits.Add(hit.point);
                        CheckForShipHits();
                        DrawLaser();
                    }

                    else CastLaser(new Ray(hit.point, bounce));

                }
                else
                {
                    Debug.DrawRay(r.origin, r.direction);
                    if (firstRay == true)
                        lastBounce = transform.forward;

                    laserHits.Add(laserHits[laserHits.Count - 1] + lastBounce * 50);

                    CheckForShipHits();
                    DrawLaser();
                }
            }
            else
            {
                CheckForShipHits();
                DrawLaser();
            }
        }

        private void DrawLaser()
        {
            DisableAllLasers();

            for (int i = 0; i < laserHits.Count - 1; i++)
            {
                lineRenderers[i].enabled = true;
                lineRenderers[i].SetPosition(0, laserHits[i]);
                lineRenderers[i].SetPosition(1, laserHits[i + 1]);
            }

            StartCoroutine(DisplayLaser(0.02F));
        }

        private void DisableAllLasers()
        {
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].enabled = false;
            }
        }

        public void Trigger()
        {
            if (Time.time - lastShot >= cooldown)
            {
                lastShot = Time.time;
                laserHits.Clear();

                Ray laser = new Ray(transform.position, transform.forward);
                CastLaser(laser);
                firstRay = true;
            }
        }

        IEnumerator DisplayLaser(float seconds)
        {
            float f = 0;
            float t = 0;

            float[] values = new float[2] { 0.1F, 0.2F };

            while (f <= seconds)
            {
                t = f / seconds;
                float v = Mathf.Lerp(values[0], values[1], t);

                for (int i = 0; i < lineRenderers.Length; i++)
                {
                    lineRenderers[i].SetWidth(v, v);
                }

                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            f = 0;
            t = 0;

            while (f <= seconds)
            {
                t = f / seconds;
                float v = Mathf.Lerp(values[1], values[0], t);

                for (int i = 0; i < lineRenderers.Length; i++)
                {
                    lineRenderers[i].SetWidth(v, v);
                }

                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            //DisableAllLasers();

            foreach (Ship ship in MarkedDestroyed)
            {
                if (ship != null && ship.gameObject != null && !ship.isDestroying)
                    ship.Destroy();
            }
        }

        public void Stop()
        {
            DisableAllLasers();
        }

        void Update()
        {
            if (autoTrigger)
                Trigger();
        }
    }
}
