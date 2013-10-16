using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Objects
{
    public class Laser : MonoBehaviour
    {
        public Material laserMat;
        List<Vector3> laserHits = new List<Vector3>();
        LineRenderer laser;
        float lastShot = 0;
        float cooldown = 0.3F;
        int bounceLimit = 6;

        void Start()
        {
            GameObject laserG = new GameObject();
            laser = laserG.AddComponent<LineRenderer>();
            laser.SetWidth(0.1F, 0.1F);
            laser.material = laserMat;
        }

        void CastLaser(Ray r)
        {
            RaycastHit hit;
            if (laserHits.Count < bounceLimit)
            {
                laserHits.Add(r.origin);

                if (Physics.Raycast(r, out hit))
                {
                    Vector3 v = Vector3.Normalize(r.direction);
                    float dot = Vector3.Dot(v, hit.normal);
                    Vector3 bounce = v - (2 * hit.normal * (Vector3.Dot(v, hit.normal)));

                    Debug.DrawLine(r.origin, hit.point);
                    Debug.Log(hit.normal);
                    CastLaser(new Ray(hit.point, bounce));
                }
                else
                {
                    Debug.DrawRay(r.origin, r.direction);
                    DrawLaser();
                }
            }
            else DrawLaser();
        }

        private void DrawLaser()
        {
            laser.SetVertexCount(laserHits.Count);

            for (int i = 0; i < laserHits.Count; i++)
            {
                laser.SetPosition(i, laserHits[i]);
            }
            StartCoroutine(DisplayLaser(0.2F));
        }

        public void Trigger()
        {
            if (Time.time - lastShot >= cooldown)
            {
                lastShot = Time.time;
                laserHits.Clear();
                Ray laser = new Ray(transform.position, transform.forward);
                CastLaser(laser);
            }
        }

        IEnumerator DisplayLaser(float seconds)
        {
            float f = 0;
            float t = 0;

            while (f < seconds / 5F)
            {
                t = f / seconds / 2F;
                laser.SetWidth(t, t);
                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            while (f < seconds)
            {
                t = f / seconds / 2F;
                laser.SetWidth(t, t);
                yield return new WaitForEndOfFrame();
                f -= Time.deltaTime;
            }

            laser.SetVertexCount(0);
        }
    }
}
