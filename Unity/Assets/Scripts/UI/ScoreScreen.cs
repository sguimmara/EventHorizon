using EventHorizon.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using EventHorizon.Storyline;
using EventHorizon.Core;

namespace EventHorizon.UserInterface
{
    public class ScoreScreen : GuiRenderer
    {
        public static ScoreScreen Instance;

        public event GameEvent OnScoreFinished;       
    }
}
