using EventHorizon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Behaviour
{
    public class BossBehaviour : MonoBehaviour
    {
        protected virtual void OnDestroy()
        {
            LevelSlider.Instance.speed = 2;
        }
    }
}
