using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon
{
    public class LevelSlider : MonoBehaviour
    {
        void Update()
        {
            transform.Translate(Vector3.left * Time.deltaTime * 4);
        }
    }
}
