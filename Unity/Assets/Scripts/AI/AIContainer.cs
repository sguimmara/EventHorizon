﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHorizon.AI
{
    [Serializable]
    public class AIContainer
    {
        public AIMotionPattern motionPattern;
        public AIShootingPattern shootingPattern;
    }
}