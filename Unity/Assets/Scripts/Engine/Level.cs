using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.Core
{
    public enum LevelPhase { Init, Intro, Upgrade, Game, Score }

    public class Level
    {
        public int number;
        public string Name;
        public string IntroText;
        AudioClip MainMusic;

        public EventLevel OnLevelLoaded;

        public void Load()
        {
            Application.LoadLevel(number);
            Debug.Log("Loading level " + this.ToString());
        }

        public override string ToString()
        {
            return (string.Concat(number.ToString(), " ", Name));
        }

        public Level(string name, int number, string introText)
        {
            this.number = number;
            this.Name = name;
            this.IntroText = introText;
        }
    }
}
