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

        DialogueLine currentLine;

        Rect actorPortraitRect;
        Rect textRect;
        Rect actorNameRect;

        Rect TopStripe;
        Rect BottomStripe;

        bool displayDialogue;

        IEnumerator RunDialogueSequence(Dialogue dialogue, float timePerCharacter, float timeBetweeLines)
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

            for (int j = 0; j < dialogue.lines.Length; j++)
            {
                currentLine = dialogue.lines[j];
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

        public override void Awake()
        {
            base.Awake();
            Instance = this;

            displayDialogue = false;
        }

        public void Play(Dialogue dialogue)
        {
            StartCoroutine(RunDialogueSequence(dialogue, 0.03F, 1F));
        }

        void Start()
        {
            ComputeUIRectangles();
        }

        protected override void Draw()
        {
            GUI.color = guiColor;
            GUI.skin = MainSkin;
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
