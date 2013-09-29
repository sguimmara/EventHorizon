using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class Credits : GuiRenderer
    {
        Rect menuRect;
        Rect titleRect;
        Rect textZone;
        Rect titleZone;
        Rect[] buttons;
        string credits;
        float creditsY;
        public float speed = 10;
        public GUISkin skin;

        public Texture2D title;

        // Use this for initialization
        void Start()
        {
            credits = LoadCredits("credits");
            creditsY = Screen.height;
            ComputeUIRectangles();
        }

        string LoadCredits(string path)
        {
            string s = ((TextAsset)Resources.Load(path)).text;

            return s;
        }

        protected override void ComputeUIRectangles()
        {
            base.ComputeUIRectangles();
            GUIContent content = new GUIContent(credits);
            Vector2 size = skin.GetStyle("credits").CalcSize(content);
            container = new Rect(Screen.width / 2 - size.x / 2, creditsY, size.x, size.y);        
            titleZone = new Rect(0, 0, container.width, title.height * container.width / title.width);

            textZone = new Rect(0, titleZone.height, container.width, size.y);
            container.height = titleZone.height + textZone.height;
        }

        public override void OnGUI()
        {
            base.OnGUI();
            creditsY -= Time.deltaTime * speed;
            GUI.BeginGroup(container);
            container.y = creditsY;
            //ComputeUIRectangles();
            GUI.skin = skin;
            GUI.DrawTexture(titleZone, title);
            GUI.Label(textZone, credits, "credits");
            GUI.EndGroup();          
        }

        void Update() 
        {
            if (creditsY + container.height < -100)
                Application.Quit();
        }
    }
}
