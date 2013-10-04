using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.UserInterface
{
    // Class for storing data about plot characters
    public class Character
    {
        public string Name;
        public string ID;
        public Texture2D Portrait;

        public override string ToString()
        {
            return ID;
        }
    }
}
