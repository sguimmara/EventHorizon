using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon
{
    public sealed class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void Shake()
        {

        }
    }
}
