using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using EventHorizon.Storyline;
using EventHorizon.UserInterface;

namespace EventHorizon.Triggers
{
    public class DialogueTrigger : TriggerBehaviour
    {
        public TextAsset DialogueFile;
        Character[] characters;

        Dialogue dialogue;

        void Awake()
        {
            if (DialogueFile == null)
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no dialogue file in ", name));

            else if (DialogueFile.text == "")
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no text in the dialogue file in ", name));

            else dialogue = ExtractDialogueData(DialogueFile);
        }

        static Dialogue ExtractDialogueData(TextAsset DialogueFile)
        {
            string text = DialogueFile.text;

            string[] list = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < list.Length; i++)
                list[i] = list[i].Trim();

            List<DialogueLine> lines = new List<DialogueLine>();

            for (int i = 0; i < list.Length - 1; i = i + 2)
            {
                string s = list[i].Trim();

                if (!s.StartsWith("#"))
                {
                    if (s.StartsWith("["))
                    {
                        s = s.Substring(1, s.Length - 2);
                        Character a = CharacterPool.Find(s);
                        string l = list[i + 1];
                        lines.Add(new DialogueLine { actor = a, line = l });
                    }
                }
            }

            return new Dialogue(lines.ToArray());
        }

        protected override void Trigger()
        {
            if (DialogueFile != null)
            {
                ConversationUi.Instance.Play(dialogue);
            }
        }
    }
}
