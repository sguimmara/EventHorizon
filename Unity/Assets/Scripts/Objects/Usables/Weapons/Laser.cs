using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace EventHorizon.Objects
{
    public enum LaserType { Normal, Reflected, PierceThroughShield };

    struct Gravitation
    {
        // Is the ray influenced by a gravitational field ?
        public bool Active;

        // The actual gravitational body;
        public Transform body;

        // The bezier curve describing the deviation of light due to gravitation.
        public Vector3 P0, P1, P2, P3;

        public Vector3 pDeviation;

        // The entry vector (the actual laser ray entering the gravitational field), 
        // the deviation vector ( = P3 - P0 )
        // and the exit vector (The ray that exits the gravitational field)
        public Vector3 vEntry, vDeviation, vExit;

        // alpha : the angle between vEntry and the segment passing through the center of mass and P0;
        // theta : the angle between vEntry and vDeviation;
        public float alpha, theta;

        // The distance between the projected point of M and vEntry  
        // and the center of mass of the gravitational body.
        public float d;

        public Vector3 pProjDeviation;

        public Vector3 k;

        public Vector3 vAlpha, pProj;

        public Vector3[] deviatedPath;

        // The attraction force of the gravitational body.
        public float pull;
    }

    public class Laser : MonoBehaviour
    {
        public LaserType laserType;
        public Material laserMat;
        List<Vector3> laserHits;
        float lastShot = 0;
        const float cooldown = 0.005F;
        const int laserHitsLimit = 20;
        Vector3 lastBounce;
        bool firstRay = true;
        public bool autoTrigger = false;

        public LayerMask HitLayer;
        public LayerMask NonReactiveLayer;

        LineRenderer[] segments;
        List<Mobile> MarkedHit;

        Dictionary<Transform, Gravitation> gravitationData;

        private Vector3 BezierInterp(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float cache = 1f - t;
            Vector3 block1 = p0 * Mathf.Pow(cache, 3);
            Vector3 block2 = 3 * p1 * t * Mathf.Pow(cache, 2);
            Vector3 block3 = 3 * p2 * t * t * cache;
            Vector3 block4 = p3 * Mathf.Pow(t, 3);
            return block1 + block2 + block3 + block4;
        }

        private Ray CalculateGravitationalPull(Transform body, Ray entry, Vector3 entryPoint)
        {
            Gravitation g;

            if (!gravitationData.ContainsKey(body))
                g = new Gravitation();
            else g = gravitationData[body];

            g.Active = true;
            g.P0 = entryPoint;
            g.vEntry = entry.direction;
            g.body = body;

            g.pProj = entry.origin + Vector3.Project(g.body.position - entry.origin, entry.direction);
            g.d = Vector3.Distance(g.pProj, g.body.position);

            g.vAlpha = g.body.position - g.P0;
            g.alpha = Vector3.Angle(g.vEntry, g.vAlpha);
            g.theta = g.alpha / (1 / g.d);

            g.pull = 1 * g.d / Vector3.Distance(g.P0, g.body.position);
            g.pDeviation = g.pProj + (1 - g.pull) * (g.body.position - g.pProj);
            g.vDeviation = g.pDeviation - g.P0;

            g.pProjDeviation = g.P0 + Vector3.Project(g.body.position - g.P0, g.vDeviation);

            g.P3 = g.P0 + (g.pProjDeviation - g.P0) * 2;

            float f = 0.333F * Vector3.Distance(g.P0, g.P3);

            g.P1 = g.P0 + Vector3.Normalize(entry.direction) * f;

            g.k = g.P0 + Vector3.Project(g.P1 - g.P0, g.vDeviation);

            g.P2 = g.P1 - g.k + g.P3 + Vector3.Normalize(g.P0 - g.k) * f;

            g.deviatedPath = new Vector3[11];

            for (int i = 0; i < 11; i++)
            {
                g.deviatedPath[i] = BezierInterp(g.P0, g.P1, g.P2, g.P3, (float)i * 0.1f);
                laserHits.Add(g.deviatedPath[i]);
            }

            if (!gravitationData.ContainsKey(body))
                gravitationData.Add(body, g);

            else gravitationData[body] = g;

            // Returns the exit vector to continue the ray casting.
            return new Ray(g.P3, g.P3 - g.P2);
        }

        private void Awake()
        {
            laserHits = new List<Vector3>(laserHitsLimit + 1);
            segments = new LineRenderer[laserHitsLimit + 1];
            MarkedHit = new List<Mobile>();
            gravitationData = new Dictionary<Transform, Gravitation>();

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
            //Gizmos.DrawRay(transform.position, transform.forward * 20);

            if (gravitationData != null && gravitationData.Count > 0)
            {
                Gravitation g;
                foreach (KeyValuePair<Transform, Gravitation> kvp in gravitationData)
                {
                    g = kvp.Value;

                    if (g.Active && g.body != null)
                    {
                        float radius = 0.03F;
                        Gizmos.DrawWireSphere(g.P0, radius);
                        Handles.Label(g.P0, "P0");

                        Gizmos.DrawWireSphere(g.P3, radius);
                        Handles.Label(g.P3, "P3");

                        Gizmos.DrawWireSphere(g.P2, radius);
                        Handles.Label(g.P2, "P2");

                        Gizmos.DrawWireSphere(g.P1, radius);
                        Handles.Label(g.P1, "P1");

                        Gizmos.DrawWireSphere(g.body.position, radius);
                        Handles.Label(g.body.position, "M");

                        Gizmos.DrawWireSphere(g.body.position, g.body.GetComponent<SphereCollider>().radius);

                        Gizmos.DrawWireSphere(g.pProj, radius);
                        Handles.Label(g.pProj, "pProj");

                        Gizmos.DrawWireSphere(g.k, radius);
                        Handles.Label(g.k, "k");

                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(g.P0 + g.vAlpha, g.P0);

                        Gizmos.color = Color.white;
                        Gizmos.DrawWireSphere(g.pDeviation, radius);
                        Handles.Label(g.pDeviation, "pDeviation");

                        Gizmos.DrawWireSphere(g.pProjDeviation, radius);
                        Handles.Label(g.pProjDeviation, "pProjDeviation");

                        Gizmos.color = Color.cyan;
                        Gizmos.DrawRay(g.P0, g.vDeviation);

                        Gizmos.DrawLine(transform.position, g.P0);
                        Gizmos.DrawLine(g.P0, g.P1);
                        //Gizmos.DrawLine(g.P1, g.P2);
                        //Gizmos.DrawLine(g.P2, g.P3);
                        Gizmos.DrawRay(g.P3, (g.P3 - g.P2) * 10);

                        for (int i = 0; i < g.deviatedPath.Length - 1; i++)
                        {
                            Gizmos.DrawLine(g.deviatedPath[i], g.deviatedPath[i + 1]);
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            DisableAllSegments();
        }

        private void CheckForTouchedShips()
        {
            RaycastHit[] hits;
            MarkedHit.Clear();
            Mobile touchedObject;

            for (int i = 0; i < laserHits.Count - 1; i++)
            {
                hits = Physics.RaycastAll(new Ray(laserHits[i], laserHits[i + 1] - laserHits[i]), Vector3.Distance(laserHits[i], laserHits[i + 1]), NonReactiveLayer);

                if (hits != null && hits.Length > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.tag == "Enemy" || hit.transform.tag == "Player" || hit.transform.tag == "Crystal")
                        {
                            touchedObject = hit.transform.gameObject.GetComponent<Mobile>();

                            if (touchedObject != null)
                            {
                                if (!MarkedHit.Contains(touchedObject))
                                    MarkedHit.Add(touchedObject);
                            }

                            else Debug.LogWarning(string.Format("Laser.CheckForTouchedShips() : {0} is marked as {1} but doesnt contain Ship component.", hit.transform.name, hit.transform.tag));
                        }
                    }
                }
            }

            NotifyMarkedShips();
        }

        private void CastLaserSegment(Ray r)
        {
            RaycastHit hit;
            if (laserHits.Count <= laserHitsLimit)
            {
                laserHits.Add(r.origin);

                if (Physics.Raycast(r, out hit, float.MaxValue, HitLayer))
                {
                    Vector3 v = Vector3.Normalize(r.direction);
                    float dot = Vector3.Dot(v, hit.normal);
                    Vector3 bounce = v - (2 * hit.normal * (Vector3.Dot(v, hit.normal)));
                    lastBounce = bounce;

                    firstRay = false;

                    if (hit.collider.gameObject.tag == "Gravitation")
                    {
                        laserHits.Add(hit.point);
                        Ray temp = CalculateGravitationalPull(hit.collider.transform, r, hit.point);
                        CastLaserSegment(temp);
                    }

                    else if (laserType == LaserType.Reflected && hit.collider.gameObject.tag == "Reflective")
                    {
                        CastLaserSegment(new Ray(hit.point, bounce));
                        return;
                    }

                    else if (hit.collider.gameObject.tag == "Normal")
                        laserHits.Add(hit.point);
                }

                // No hits
                else
                {
                    if (firstRay == true)
                        lastBounce = transform.forward;
                    else lastBounce = r.direction;

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

                segments[i].SetWidth(0.2F, 0.2F);
            }

            //StartCoroutine(DisplayLaser(0.02F));
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


        }

        private void NotifyMarkedShips()
        {
            if (MarkedHit.Count > 0)
            {
                foreach (Mobile collidable in MarkedHit)
                {
                    if (collidable != null)
                        collidable.NotifyHitByLaser(laserType);
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
