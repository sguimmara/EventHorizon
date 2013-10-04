using UnityEngine;
using System.Collections;
using UnityExtended;
using EventHorizon.Objects;
using EventHorizon.Core;

namespace EventHorizon.UserInterface
{
    public class IngameUi : GuiRenderer
    {
        Rect shipRect;
        Rect HUDZone;
        public Texture2D shipIcon;
        float t;
        float resultInterp;

        public Texture2D title;
        Color[] shipGradient;
        const int numberOfColors = 4;

        Rect moduleArea;
        Rect[] moduleRects;

        Player player;

        // Use this for initialization
        void Start()
        {
            player = Engine.Instance.player;
            ComputeUIRectangles();
            shipGradient = new Color[numberOfColors] { Color.green, Color.yellow, Color.red, Color.black };
            shipIcon = Utils.Load<Texture2D>("ship_representation");
        }


        protected override void ComputeUIRectangles()
        {
            float height = 100;
            float width = 200;
            container = new Rect(Screen.width / 2 - width / 2, Screen.height - height, width, height);

            shipRect = new Rect(0, 0, container.width, container.height * 0.7F);

            moduleArea = new Rect(0, container.height * 0.7F, container.width, container.height * 0.3F);
            moduleRects = new Rect[4];

            for (int i = 0; i < moduleRects.Length; i++)
            {
                moduleRects[i] = new Rect(i * moduleArea.width / 4F, 0, moduleArea.width / 4, moduleArea.height);
            }
        }

        void DrawShipUI()
        {
            float hp = Mathf.Clamp01(((float) player.CurrentHp / player.MaxHp));
            GUI.color = UnityExtended.Interp.Lerp(shipGradient,1 - hp);
            GUI.Box(shipRect, shipIcon);
            GUI.Box(moduleArea, "");
            GUI.color = Color.white;
            GUI.BeginGroup(moduleArea);
            for (int i = 0; i < moduleRects.Length; i++)
                GUI.Box(moduleRects[i], player.Slots[i].Icon);  
            GUI.EndGroup();
        }

        protected override void Draw()
        {
            GUI.skin = MainSkin;
            GUI.BeginGroup(container);
            DrawShipUI();
            GUI.EndGroup();
        }
    }
}
