using EventHorizon.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    public class Solver : MonoBehaviour
    {
        public int resolution;
        public bool KeepRunning;

        Laser laser;
        bool finished;
        bool solved;
        int activatedCrystals;
        int numberOfCrystals;

        List<Collider> volumes = new List<Collider>();
        Crystal[] crystals;

        private struct Solution
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        List<Solution> Solutions = new List<Solution>();

        private Bounds GetSceneBounds()
        {
            Renderer r = transform.Find("Limits").renderer;
            transform.Find("Limits").parent = null;
            r.enabled = false;
            return r.bounds;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward);

            if (true)
            {
                foreach (Solution solution in Solutions)
                {
                    Gizmos.DrawWireSphere(solution.Position, 0.05F);
                }
            }
        }

        private void Start()
        {
            Screen.SetResolution(1, 1, false);
            QualitySettings.vSyncCount = 0;
            SetRenderers(false);
            crystals = FindObjectsOfType(typeof(Crystal)) as Crystal[];

            foreach (var item in GameObject.FindGameObjectsWithTag("SolverVolume"))
            {
                volumes.Add(item.collider);
            }

            //foreach (Crystal crystal in FindObjectsOfType(typeof(Crystal)) as Crystal[])
            //{
            //    numberOfCrystals++;
            //    crystal.OnActivated += delegate { if (activatedCrystals < numberOfCrystals) activatedCrystals++; };
            //    crystal.OnDeactivated += delegate { if (activatedCrystals > 0) activatedCrystals--; };
            //}

            laser = GetComponent<Laser>();
            StartCoroutine(Solve());
        }

        private void CheckForSolved()
        {
            bool test = true;

            foreach (Crystal crystal in crystals)
            {
                if (crystal.Activated == false)
                {
                    test = false;
                    break;
                }
            }

            if (test)
            {
                Solutions.Add(new Solution { Position = transform.position, Rotation = transform.rotation });

                if (!KeepRunning)
                {
                    solved = true;
                    StopAllCoroutines();
                    SetRenderers(true);
                    finished = true;
                    Debug.Log(Solutions.Count);
                }
            }
        }

        private void SetRenderers(bool b)
        {
            foreach (Renderer r in FindObjectsOfType(typeof(Renderer)) as Renderer[])
                r.enabled = b;

            foreach (LineRenderer r in FindObjectsOfType(typeof(LineRenderer)) as LineRenderer[])
                r.enabled = b;
        }

        static public bool IsInside(Collider test, Vector3 point)
        {
            Vector3 center;
            Vector3 direction;
            Ray ray;
            RaycastHit hitInfo;
            bool hit;

            // Use collider bounds to get the center of the collider. May be inaccurate
            // for some colliders (i.e. MeshCollider with a 'plane' mesh)
            center = test.bounds.center;

            // Cast a ray from point to center
            direction = center - point;
            ray = new Ray(point, direction);
            hit = test.Raycast(ray, out hitInfo, direction.magnitude);

            // If we hit the collider, point is outside. So we return !hit
            return !hit;
        }

        private IEnumerator Solve()
        {
            Bounds sceneBounds = GetSceneBounds();

            int[] angles = new int[] { 90, 45, 10, 5, 2, 1 };

            int angleIncrement = 1;

            while (!solved || KeepRunning)
            {
                for (int a = 0; a < angles.Length; a++)
                {
                    angleIncrement = angles[a];

                    for (int i = 0; i < resolution; i++)
                    {
                        for (int j = 0; j < resolution; j++)
                        {
                            Vector3 currentPos = new Vector3(sceneBounds.min.x + (sceneBounds.max.x - sceneBounds.min.x) * i / resolution, sceneBounds.min.y + (sceneBounds.max.y - sceneBounds.min.y) * j / resolution, transform.position.z);

                            bool isInside = false;
                            foreach (Collider item in volumes)
                            {
                                if (IsInside(item, currentPos))
                                {
                                    isInside = true;
                                }
                            }

                            if (isInside)
                            {
                                transform.position = currentPos;

                                for (int k = 0; k < 360; k += angleIncrement)
                                {
                                    transform.Rotate(new Vector3(angleIncrement, 0, 0));
                                    laser.Trigger();
                                    CheckForSolved();
                                    yield return new WaitForEndOfFrame();
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
