using EventHorizonGame.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueScreen : GuiRenderer
    {
        string[] originals;
        string newString;
        public AudioClip beep;

        IEnumerator BuildStringOverTime(string[] originals, float timePerCharacter, float timeBetweenStrings, bool ponderate)
        {
            for (int j = 0; j < originals.Length; j++)
            {
                string original = originals[j];
                newString = "";
                for (int i = 0; i < original.Length; i++)
                {
                    newString = string.Concat(newString, original[i]);
                    audio.PlayOneShot(beep);
                    yield return new WaitForSeconds(timePerCharacter);
                }

                yield return new WaitForSeconds(timeBetweenStrings + 0.03F * newString.Length);
            }

            float f = 0;

            while (f <= 3)
            {
                guiColor = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - (f / 3F));
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        void Start()
        {
            originals = new string[8];
            originals[0] = "Command, this is LMS Matsuri, do you copy ?";
            originals[1] = "...";
            originals[2] = "Command, this is Teraesa Niemeyer from LMS Matsuri, do you receive me ? ";
            originals[3] = "They don't reply. This wormhole must have thrown us damn far from home...";
            originals[4] = "Captain !";
            originals[5] = "What is it, Marshall ?";
            originals[6] = "I'm picking up strange signatures on the radar.";
            originals[7] = "Let's have a look...";
            StartCoroutine(BuildStringOverTime(originals, 0.03F, 1F, true));
        }

        public override void OnGUI()
        {
         	 base.OnGUI();
            GUI.color = guiColor;
            GUI.skin = skin;
            //GUI.Label(new Rect(0, 0, 400, 30), originalString);
            GUI.Label(new Rect(50, Screen.height - 600, 700, 200), newString);
        }
    }
}
