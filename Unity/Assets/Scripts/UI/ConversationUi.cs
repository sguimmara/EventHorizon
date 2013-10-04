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
    public class ConversationUi : GuiRenderer
    {
        public static ConversationUi Instance;

        public event GameEvent OnDialogueFinished;

        public TextAsset DialogueFile;
        public float FadeTime = 1;

        float stripeHeight;

        string[] originals;
        string currentDialogueLine;
        public AudioClip beep;

        public Texture2D marshallTexture;
        public Texture2D taeresaTexture;
        public Texture2D nobodyTexture;
        public Texture2D weilinTexture;
        public Texture2D jacobTexture;

        public Texture2D black;

        Character[] actors;

        List<DialogueLine> lines;
        DialogueLine currentLine;

        Rect actorPortraitRect;
        Rect textRect;
        Rect actorNameRect;

        Rect TopStripe;
        Rect BottomStripe;

        bool displayDialogue;

        IEnumerator RunDialogueSequence(DialogueLine[] lines, float timePerCharacter, float timeBetweeLines)
        {
            float f = 0;
            float delta;

            Show();

            while (f <= 1)
            {
                f += Time.deltaTime;

                delta = Mathf.Lerp(0, stripeHeight, f);

                TopStripe.height = delta;
                BottomStripe.y = Screen.height - delta;

                yield return new WaitForEndOfFrame();
            }

            displayDialogue = true;

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

            if (FadeTime > 0)
            {
                f = 0;
                while (f <= FadeTime)
                {
                    guiColor = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1 - (f / FadeTime));
                    f += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }

            if (OnDialogueFinished != null)
                OnDialogueFinished();

            f = 1;

            while (f >= 0)
            {
                f -= Time.deltaTime;

                delta = Mathf.Lerp(0, stripeHeight, f);

                TopStripe.height = delta;
                BottomStripe.y = Screen.height - delta;

                yield return new WaitForEndOfFrame();
            }

            displayDialogue = false;
            Hide();
        }

        // Find an actor by its ID
        Character Find(string ID)
        {
            for (int i = 0; i < actors.Length; i++)
                if (actors[i].ID == ID)
                    return actors[i];

            return null;
        }

        protected override void ComputeUIRectangles()
        {
            stripeHeight = Screen.height * 0.12F;

            float containerWidth = Screen.width / 2;
            float containerHeight = stripeHeight;
            container = new Rect(Screen.width / 2 - containerWidth / 2, Screen.height - containerHeight, containerWidth, containerHeight);
            actorPortraitRect = new Rect(0, 0, containerHeight, containerHeight);
            textRect = new Rect(actorPortraitRect.width, 0, containerWidth - actorPortraitRect.width, containerHeight);
            actorNameRect = new Rect(0, 0, 100, 20);            

            TopStripe = new Rect(-2, 0, Screen.width+6, 0);
            BottomStripe = new Rect(-2, Screen.height, Screen.width+6, stripeHeight);
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
                        Character a = Find(s);
                        string l = list[i + 1];
                        lines.Add(new DialogueLine { actor = a, line = l });
                    }
                }
            }
        }

        void Awake()
        {
            Instance = this;

            TEMP_CREATE_ACTORS();
            displayDialogue = false;

            if (!audio)
                gameObject.AddComponent<AudioSource>();

            if (DialogueFile == null)
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no dialogue file in ", name));

            else if (DialogueFile.text == "")
                Debug.LogWarning(String.Concat(Application.loadedLevelName, " error : there is no text in the dialogue file in ", name));

            else ExtractDialogueData();
        }

        public void Play(Dialogue conversation)
        {
            StartCoroutine(RunDialogueSequence(lines.ToArray(), 0.03F, 1F));
        }

        void Start()
        {
            ComputeUIRectangles();
        }

        // Temporary
        void TEMP_CREATE_ACTORS()
        {
            Character taeresa = new Character { ID = "Taeresa", Name = "Taeresa Niemeyer", Portrait = taeresaTexture };
            Character marshall = new Character { ID = "Marshall", Name = "Marshall Elon", Portrait = marshallTexture };
            Character weilin = new Character { ID = "Weilin", Name = "Weilin Gu", Portrait = weilinTexture };
            Character jacob = new Character { ID = "Jacob", Name = "Jacob Freeman", Portrait = jacobTexture };
            Character nobody = new Character { ID = "Nobody", Name = "", Portrait = nobodyTexture };

            actors = new Character[5] { taeresa, marshall, nobody, weilin, jacob };
        }

        protected override void Draw()
        {
            GUI.color = guiColor;
            GUI.skin = skin;
            GUI.DrawTexture(TopStripe, black);
            GUI.DrawTexture(BottomStripe, black);

            if (displayDialogue)
            {
                GUI.BeginGroup(container);
                GUI.DrawTexture(actorPortraitRect, currentLine.actor.Portrait);
                GUI.Label(textRect, currentDialogueLine);
                GUI.EndGroup();
            }
        }
    }
}
