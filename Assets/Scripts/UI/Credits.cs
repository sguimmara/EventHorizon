using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class Credits : GuiRenderer
    {
        public event Event OnLevelLoaded;

        bool firstTime = true;

        Rect menuRect;
        Rect titleRect;
        Rect[] buttons;
        string credits;
        float creditsY;
        float speed = 10;
        public GUISkin skin;

        public Texture2D title;

        // Use this for initialization
        void Start()
        {
            credits = LoadCredits("credits");
            creditsY = Screen.height;
            //credits = "afijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeaoafijeijgpaiae\neajfiefjâpifeaf\nfepfjeao";
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
            container = new Rect(Screen.width /2 - size.x /2, creditsY, size.x, size.y);
        }

        public override void OnGUI()
        {
            base.OnGUI();
            creditsY -= Time.deltaTime * speed;
            container.y = creditsY;
            //ComputeUIRectangles();
            GUI.skin = skin;
            GUI.Label(container, credits, "credits");
            //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, visibility);            
        }
    }
}
