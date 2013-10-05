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
    public class UpgradeScreen : GuiRenderer
    {
        Rect startButtonRect;
        public static UpgradeScreen Instance;

        public event GameEvent OnUpgradeFinished;

        protected override void ComputeUIRectangles()
        {
            container = new Rect(0, 0, Screen.width, Screen.height);
            startButtonRect = new Rect(Screen.width - 250, Screen.height - 100, 200, 80);
        }

        protected override void Show()
        {
            base.Show();
            ComputeUIRectangles();
        }

        protected override void Draw()
        {
            GUI.skin = MainSkin;
           if (GUI.Button(startButtonRect, "Start"))
               OnUpgradeFinished();
        }

        public override string ToString()
        {
            return "UpgradeScreen";
        }
    }
}
