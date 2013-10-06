using UnityEngine;
using System.Collections;
using EventHorizon.Core;

namespace EventHorizon.UserInterface
{
    public class Cutscene : GuiRenderer
    {
        Rect textZone;
        Rect startButtonRect;
        float textPosition;
        public float speed = 10;
        string text;

        public event GameEvent OnCutsceneFinished;

        public override void Launch()
        {
            base.Launch();
            text = Engine.Instance.CurrentLevel.IntroText;
            ComputeUIRectangles();
            StartCoroutine(Play());
        }

        IEnumerator Play()
        {
            textPosition = 0;
            while (textPosition + textZone.height > -100)
            {
                textPosition -= Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }

            OnCutsceneFinished();
        }

        void Stop()
        {
            StopAllCoroutines();
        }

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            textPosition = Screen.height;
        }

        protected override void ComputeUIRectangles()
        {
            textPosition = 0;
            GUIContent content = new GUIContent(text);
            Vector2 size = MainSkin.GetStyle("credits").CalcSize(content);
            container = new Rect(0, 0, Screen.width, Screen.height);
            startButtonRect = new Rect(Screen.width - 250, Screen.height - 100, 200, 80);
            float textWidth = container.width / 3;
            textZone = new Rect(container.width / 2 - textWidth / 2, textPosition, textWidth, size.y); 
        }

        protected override void Draw()
        {            
            GUI.BeginGroup(container);
            textZone.y = textPosition;
            GUI.skin = MainSkin;
            GUI.Label(textZone, text, "credits");
            if (GUI.Button(startButtonRect, "Skip"))
                OnCutsceneFinished();
            GUI.EndGroup();          
        }

        public override string ToString()
        {
            return "Cutscene";
        }
    }
}
