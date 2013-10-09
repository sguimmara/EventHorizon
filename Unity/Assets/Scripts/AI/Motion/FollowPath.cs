using EventHorizon.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace EventHorizon.AI
{
    public class FollowPath : AIMotionPattern
    {
        public Transform[] waypoints;

        private Vector3[] positions;
        private Vector3[] rotations;

        public float duration;

        void Awake()
        {
            if (positions == null || positions.Length == 0)
                ComputePositions();
            Play();
        }

        void OnBecameVisible()
        {
            Play();
        }

        void ComputePositions()
        {
            if (waypoints != null && waypoints.Length > 0)
            {
                positions = new Vector3[waypoints.Length + 1];
                positions[0] = transform.position;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    positions[i + 1] = waypoints[i].position;
                }
            }
        }

        void OnDrawGizmos()
        {
            ComputePositions();

            for (int i = 0; i < positions.Length; i++)
            {
                if (i > 0)
                    Gizmos.DrawWireSphere(positions[i], 0.035F);

                if (i < positions.Length - 1)
                    Gizmos.DrawLine(positions[i], positions[i + 1]);
            }
        }

        IEnumerator RunWaypoints(float duration)
        {
            if (duration > 0 && objectToMove != null)
            {
                float f = 0;

                while (f <= duration)
                {
                    objectToMove.position = Interp.Lerp(positions, f / duration);
                    f += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public void Play()
        {
            StartCoroutine(RunWaypoints(duration));
        }

        void Update()
        {
            //if (objectToMove == null)
            //{
            //    StopAllCoroutines();
            //    Destroy(gameObject);
            //}
        }

    }
}
