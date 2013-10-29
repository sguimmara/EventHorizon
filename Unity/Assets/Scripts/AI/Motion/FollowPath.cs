using EventHorizon.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExtended;

namespace EventHorizon.AI
{
    [Serializable]
    public class FollowPath : AIMotionPattern
    {
        public bool Rotate;
        public bool MirrorX;
        public bool MirrorY;

        public Transform currentTransform { get; set; }
        public Transform[] waypoints;

        private Vector3[] positions;
        private Vector3[] rotations;
        Vector3 delta;

        public float duration;

        bool validPattern = true;

        void Awake()
        {
            if (positions == null || positions.Length == 0)
                ComputePositions();
        }

        void OnBecameVisible()
        {
            ComputePositions();
            Play();
        }

        void MirrorOnY()
        {
            Vector3 reference = positions[0];

            for (int i = 1; i < positions.Length; i++)
            {
                float newVal = positions[i].y - 2 * (positions[i].y - reference.y);
                positions[i] = new Vector3(positions[i].x, newVal, positions[i].z);
            }
        }

        public void ComputePositions()
        {
            validPattern = true;
            foreach (Transform t in waypoints)
                if (t == null)
                {
                    validPattern = false;
                    return;
                }

            if (currentTransform != null)
                delta = currentTransform.position - transform.position;

            else delta = Vector3.zero;

            if (waypoints != null && waypoints.Length > 0)
            {
                positions = new Vector3[waypoints.Length + 1];
                positions[0] = currentTransform == null ? transform.position : currentTransform.position;

                for (int i = 0; i < waypoints.Length; i++)
                {
                    positions[i + 1] = waypoints[i].position + delta;
                }

                if (MirrorY)
                    MirrorOnY();
            }
        }
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {

            ComputePositions();

            if (validPattern)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (i > 0)
                        Gizmos.DrawWireSphere(positions[i], 0.035F);

                    if (i < positions.Length - 1)
                        Gizmos.DrawLine(positions[i], positions[i + 1]);
                }
            }
            else UnityEditor.Handles.Label(transform.position, "Invalid motion pattern");
        }
#endif

        IEnumerator RunWaypoints(float duration)
        {
            if (duration > 0)
            {
                Vector3 pos;
                float f = 0;

                while (f <= duration)
                {
                    pos = Interp.Lerp(positions, f / duration);
                    transform.position = pos;
                    f += Time.deltaTime;
                    if (Loop && f >= duration)
                        f = 0;
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        void OnDestroy()
        {
            foreach (Transform t in waypoints)
                Destroy(t.gameObject);
        }

        public override void Play()
        {
            foreach (Transform t in waypoints)
                t.parent = null;

            transform.parent = null;

            StartCoroutine(RunWaypoints(duration));
        }
    }
}
