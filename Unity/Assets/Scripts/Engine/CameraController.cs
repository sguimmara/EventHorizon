using EventHorizon.Core;
using EventHorizon.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon
{
    public sealed class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        Vector3 originalPosition;

        void Awake()
        {
            Instance = this;
            originalPosition = transform.position;
        }

        public void Shake(Mobile ship)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeSequence());
        }

        void Start()
        {
            Engine.Instance.OnShipDestroyed += Shake;
        }

        IEnumerator ShakeSequence()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();


            float f = 0;
            float sign = 1;
            while (f < 0.02F)
            {
                sign *= -1;
                transform.position += (new Vector3(0.1F * sign, 0.1F * sign, transform.position.z));
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = originalPosition;

        }
    }
}
