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
        float dialogueLength;
        float timePerCharacter = 0.03F;
        float timeBetweenLines = 1F;

        Dialogue dialogue;

        void Awake()
        {
            if (DialogueFile == null)
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no dialogue file in ", name));

            else if (DialogueFile.text == "")
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no text in the dialogue file in ", name));

            else dialogue = ExtractDialogueData(DialogueFile);
        }

        Dialogue ExtractDialogueData(TextAsset DialogueFile)
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
                        // actor ID
                        string actorId = s.Substring(1, s.Length - 2);

                        string l = list[i + 1];

                        lines.Add(new DialogueLine { actorID = actorId, line = l });
                    }
                }
            }

            //dialogueLength = 3;

            //foreach (DialogueLine line in lines)
            //    dialogueLength += (line.line.Length * timePerCharacter + (timeBetweenLines + 0.03F * line.line.Length));

            //dialogueLength *= 2;

            return new Dialogue(lines.ToArray());
        }

        //void OnDrawGizmos()
        //{
        //    if (dialogue == null)
        //    {
        //        ExtractDialogueData(DialogueFile);
        //    }
        //    Gizmos.DrawLine(new Vector3(transform.position.x, 2.059772F, 5), new Vector3(transform.position.x, -2.059772F, 5));
        //    Gizmos.DrawLine(new Vector3(transform.position.x + dialogueLength, 2.059772F, 5), new Vector3(transform.position.x + dialogueLength, -2.059772F, 5));
        //}

        protected override void Trigger()
        {
            if (DialogueFile != null)
            {
                ConversationUi.Instance.Play(dialogue);
            }
        }
    }
}
