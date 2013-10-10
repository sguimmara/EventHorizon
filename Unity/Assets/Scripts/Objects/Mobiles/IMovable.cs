using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    interface IMovable
    {
        Vector3 Direction { get; set; }
        void UpdatePosition();
    }
}
