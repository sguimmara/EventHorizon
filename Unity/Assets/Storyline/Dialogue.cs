using EventHorizon.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHorizon.Storyline
{
    public struct DialogueLine
    {
        public string actorID;
        public string line;
    }

    public class Dialogue
    {
       public  DialogueLine[] lines;

        public Dialogue(DialogueLine[] lines)
        {
            this.lines = lines;
        }
    }
}
