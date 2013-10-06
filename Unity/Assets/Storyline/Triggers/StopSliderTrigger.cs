using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using EventHorizon.Storyline;
using EventHorizon.Core;

namespace EventHorizon.Triggers
{
    public class StopSliderTrigger : TriggerBehaviour
    {
        protected override void Trigger()
        {
            LevelSlider.Instance.speed = 0;
        }
    }
}
