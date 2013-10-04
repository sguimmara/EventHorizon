using UnityEngine;
using System.Collections;
using EventHorizon.Core;

namespace EventHorizon.UserInterface
{
    public class Cutscene : GuiRenderer
    {
        Rect menuRect;
        Rect titleRect;
        Rect textZone;
        Rect titleZone;
        Rect[] buttons;
        string cutsceneText;
        float creditsY;
        public float speed = 10;

        public Texture2D title;

        public event GameEvent OnCutsceneStarted;
        public event GameEvent OnCutsceneFinished;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            creditsY = Screen.height;
            ComputeUIRectangles();
        }

        protected override void ComputeUIRectangles()
        {
            GUIContent content = new GUIContent(cutsceneText);
            Vector2 size = MainSkin.GetStyle("credits").CalcSize(content);
            container = new Rect(Screen.width / 2 - size.x / 2, creditsY, size.x, size.y);        

            textZone = new Rect(0, titleZone.height, container.width, size.y);
            container.height = titleZone.height + textZone.height;
        }

        protected override void Draw()
        {
            creditsY -= Time.deltaTime * speed;
            GUI.BeginGroup(container);
            container.y = creditsY;
            GUI.skin = MainSkin;
            GUI.Label(textZone, cutsceneText, "credits");
            GUI.EndGroup();          
        }

        void Update() 
        {
            if (creditsY + container.height < -100)
                Application.Quit();
        }
    }
}
