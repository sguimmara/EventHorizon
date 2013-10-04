
using EventHorizon.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHorizon.Storyline
{
    class CharacterPool
    {
        public static Character[] characters;

        // Find an actor by its ID
        public static Character Find(string ID)
        {
            for (int i = 0; i < characters.Length; i++)
                if (characters[i].ID == ID)
                    return characters[i];

            return null;
        } 
    }
}
