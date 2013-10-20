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
        public LaserType laserType;
        public Material laserMat;
        List<Vector3> laserHits;
        float lastShot = 0;
        const float cooldown = 0.05F;
        const int laserHitsLimit = 20;
        Vector3 lastBounce;
        bool firstRay = true;
        public bool autoTrigger = false;

        public LayerMask bouncingLayer;
        public LayerMask hitLayer;

        LineRenderer[] segments;
        List<Ship> MarkedHit;

        private void Awake()
        {
            laserHits = new List<Vector3>(laserHitsLimit + 1);
            segments = new LineRenderer[laserHitsLimit + 1];
            MarkedHit = new List<Ship>();

            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = (new GameObject()).AddComponent<LineRenderer>();
                segments[i].SetWidth(0.0F, 0.0F);
                segments[i].material = laserMat;
            }
        }

        private void Update()
        {
            if (autoTrigger)
                Trigger();
        }

        private void OnDrawGizmos()
        {
            Color c = Color.white;
            switch (laserType)
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

        private void OnDestroy()
        {
            DisableAllSegments();
        }

        private void CheckForTouchedShips()
        {
            RaycastHit[] hits;
            MarkedHit.Clear();
            Ship touchedShip;

            for (int i = 0; i < laserHits.Count - 1; i++)
            {
                hits = Physics.RaycastAll(new Ray(laserHits[i], laserHits[i + 1] - laserHits[i]), Vector3.Distance(laserHits[i], laserHits[i + 1]), hitLayer);

                if (hits != null && hits.Length > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.tag == "Enemy" || hit.transform.tag == "Player")
                        {
                            touchedShip = hit.transform.gameObject.GetComponent<Ship>();

                            if (touchedShip != null)
                            {
                                if (!MarkedHit.Contains(touchedShip))
                                    MarkedHit.Add(touchedShip);
                            }

                            else Debug.LogWarning(string.Format("Laser.CheckForTouchedShips() : {0} is marked as {1} but doesnt contain Ship component.", hit.transform.name, hit.transform.tag));
                        }
                    }
                }
            }
        }

        private void CastLaserSegment(Ray r)
        {
            RaycastHit hit;
            if (laserHits.Count <= laserHitsLimit)
            {
                laserHits.Add(r.origin);

                if (Physics.Raycast(r, out hit, float.MaxValue, bouncingLayer))
                {
                    Vector3 v = Vector3.Normalize(r.direction);
                    float dot = Vector3.Dot(v, hit.normal);
                    Vector3 bounce = v - (2 * hit.normal * (Vector3.Dot(v, hit.normal)));
                    lastBounce = bounce;

                    firstRay = false;

                    int qq = LayerMask.NameToLayer("Reflective");

                    if (laserType == LaserType.Reflected && hit.collider.gameObject.layer == qq)
                    {
                        CastLaserSegment(new Ray(hit.point, bounce));
                        return;
                    }

                    else
                        laserHits.Add(hit.point);
                }

                // No hits
                else
                {
                    if (firstRay == true)
                        lastBounce = transform.forward;

                    laserHits.Add(laserHits[laserHits.Count - 1] + lastBounce * 50);
                }
            }

            CheckForTouchedShips();
            DrawLaser();
        }

        private void DisableAllSegments()
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] != null)
                    segments[i].enabled = false;
            }
        }

        private void DrawLaser()
        {
            DisableAllSegments();

            for (int i = 0; i < laserHits.Count - 1; i++)
            {
                segments[i].enabled = true;
                segments[i].SetPosition(0, laserHits[i]);
                segments[i].SetPosition(1, laserHits[i + 1]);
            }

            StartCoroutine(DisplayLaser(0.02F));
        }

        IEnumerator DisplayLaser(float seconds)
        {
            float f = 0;
            float t = 0;

            float[] values = new float[2] { 0.15F, 0.2F };

            while (f <= seconds)
            {
                t = f / seconds;
                float v = Mathf.Lerp(values[0], values[1], t);

                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].SetWidth(v, v);
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

                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i].SetWidth(v, v);
                }

                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            NotifyMarkedShips();
        }

        private void NotifyMarkedShips()
        {
            if (MarkedHit.Count > 0)
            {
                foreach (Ship ship in MarkedHit)
                {
                    if (ship != null && ship.gameObject != null && !ship.isDestroying)
                        ship.NotifyHitByLaser(laserType);
                }
            }
        }

        public void Stop()
        {
            DisableAllSegments();
        }

        //Entry point
        public void Trigger()
        {
            if (Time.time - lastShot >= cooldown)
            {
                lastShot = Time.time;
                laserHits.Clear();

                Ray laser = new Ray(transform.position, transform.forward);
                CastLaserSegment(laser);
                firstRay = true;
            }
        }
    }
}
