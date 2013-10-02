using EventHorizonGame.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace EventHorizonGame.UI
{
    struct DialogueLine
    {
        public Actor actor;
        public string line;
    }

    public class DialogueScreen : GuiRenderer
    {
        public event Event OnDialogueFinished;

        public TextAsset DialogueFile;

        string[] originals;
        string currentDialogueLine;
        public AudioClip beep;

        public Texture2D marshallTexture;
        public Texture2D taeresaTexture;
        public Texture2D nobodyTexture;

        private Actor[] actors;

        List<DialogueLine> lines;
        DialogueLine currentLine;

        Rect actorPortraitRect;
        Rect textRect;
        Rect actorNameRect;

        IEnumerator RunDialogueSequence(DialogueLine[] lines, float timePerCharacter, float timeBetweeLines)
        {
            for (int j = 0; j < lines.Length; j++)
            {
                currentLine = lines[j];
                string original = currentLine.line;
                currentDialogueLine = "";

                for (int i = 0; i < original.Length; i++)
                {
                    currentDialogueLine = string.Concat(currentDialogueLine, original[i]);
                    audio.PlayOneShot(beep);
                    yield return new WaitForSeconds(timePerCharacter);
                }

                yield return new WaitForSeconds(timeBetweeLines + 0.03F * original.Length);
            }

            float f = 0;

            while (f <= 3)
            {
                guiColor = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - (f / 3F));
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (OnDialogueFinished != null)
                OnDialogueFinished();
        }

        // Find an actor by its ID
        Actor Find(string ID)
        {
            for (int i = 0; i < actors.Length; i++)
                if (actors[i].ID == ID)
                    return actors[i];

            return null;
        }

        protected override void ComputeUIRectangles()
        {
            float containerWidth = Screen.width / 2;
            float containerHeight = Screen.height * 0.2F;
            container = new Rect(Screen.width / 2 - containerWidth / 2, Screen.height - containerHeight, containerWidth, containerHeight);
            actorPortraitRect = new Rect(0, 0, containerHeight, containerHeight);
            textRect = new Rect(actorPortraitRect.width, 0, containerWidth - actorPortraitRect.width, containerHeight);
            actorNameRect = new Rect(0, 0, 100, 20);
        }

        void ExtractDialogueData()
        {
            string text = DialogueFile.text;

            string[] list = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < list.Length; i++)
                list[i] = list[i].Trim();

            lines = new List<DialogueLine>();

            for (int i = 0; i < list.Length - 1; i = i + 2)
            {
                string s = list[i].Trim();

                if (!s.StartsWith("#"))
                {
                    if (s.StartsWith("["))
                    {
                        s = s.Substring(1, s.Length - 2);
                        Actor a = Find(s);
                        string l = list[i + 1];
                        lines.Add(new DialogueLine { actor = a, line = l });
                    }
                }
            }
        }

        void Awake()
        {
            CREATE_TEST_ACTORS();

            if (!audio)
                gameObject.AddComponent<AudioSource>();

            if (DialogueFile == null)
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no dialogue file in ", name));

            else if (DialogueFile.text == "")
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no text in the dialogue file in ", name));

            else ExtractDialogueData();
        }

        void Play()
        {
            StartCoroutine(RunDialogueSequence(lines.ToArray(), 0.03F, 1F));
        }

        void Start()
        {
            ComputeUIRectangles();
            Play();
        }

        void CREATE_TEST_ACTORS()
        {
            Actor taeresa = new Actor { ID = "Taeresa", Name = "Taeresa", Portrait = taeresaTexture };
            Actor marshall = new Actor { ID = "Marshall", Name = "Marshall", Portrait = marshallTexture };
            Actor nobody = new Actor { ID = "Nobody", Name = "", Portrait = nobodyTexture };

            actors = new Actor[3] { taeresa, marshall, nobody };
        }

        public override void OnGUI()
        {
            base.OnGUI();
            GUI.BeginGroup(container);
            GUI.color = guiColor;
            GUI.skin = skin;

            GUI.Box(new Rect(0, 0, container.width, container.height), "");

            GUI.Label(actorNameRect, currentLine.actor.Name);
            GUI.DrawTexture(actorPortraitRect, currentLine.actor.Portrait);
            GUI.Label(textRect, currentDialogueLine);
            GUI.EndGroup();
        }

        void OnTriggerEnter(Collider other)
        {
            if (DialogueFile != null && other.tag == "Player")
                Play();
        }
    }
}
