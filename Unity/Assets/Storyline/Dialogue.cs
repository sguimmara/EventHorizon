using EventHorizon.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHorizon.Storyline
{
    public struct DialogueLine
    {
        public Character actor;
        public string line;
    }

    public class Dialogue
    {
        DialogueLine[] lines;

        public Dialogue(DialogueLine[] lines)
        {
            this.lines = lines;
        }
    }
}
