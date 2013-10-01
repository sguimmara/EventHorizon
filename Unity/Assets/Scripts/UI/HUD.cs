using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class HUD : GuiRenderer
    {
        Rect shipRect;
        Rect HUDZone;
        public Texture2D ship;
        float t;
        float resultInterp;

        public Texture2D title;
        Color[] shipGradient;
        const int numberOfColors = 4;
        GUISkin skin;

        // Use this for initialization
        void Start()
        {
            ComputeUIRectangles();
            shipGradient = new Color[numberOfColors] { Color.green, Color.yellow, Color.red, Color.black };
            ship = Utils.Load<Texture2D>("ship_representation");
            skin = EventHorizon.Instance.MainSkin;
        }


        protected override void ComputeUIRectangles()
        {
            //base.ComputeUIRectangles();
            float height = 60;
            container = new Rect(0, Screen.height - height, Screen.width, height);
            HUDZone = new Rect(0, 0, container.width, container.height);
            shipRect = new Rect(HUDZone.width / 2 - 50, 0, 100, container.height);
        }

        Color GetGradientColor(float t)
        {
            float stepSize = 1F / (shipGradient.Length - 1);

            int currentStep = Mathf.FloorToInt(t / stepSize);
            float lowThreshold = currentStep * stepSize;
            resultInterp = Mathf.Clamp01((t - lowThreshold) / stepSize);

            if (currentStep < shipGradient.Length - 1)
                return Color.Lerp(shipGradient[currentStep], shipGradient[currentStep + 1], resultInterp);
            else return (shipGradient[shipGradient.Length - 1]);
        }

        public override void OnGUI()
        {
            GUI.skin = skin;
            float hp = ((float)EventHorizon.Instance.player.GetMobileData().currentHP / EventHorizon.Instance.player.GetMobileData().maxHP);
            base.OnGUI();
            GUI.BeginGroup(container);
            GUI.Box(HUDZone, "", "HudBox");
            GUI.color = GetGradientColor(1-hp);
            GUI.Box(shipRect, ship, "HudBox");
            GUI.color = Color.white;
            GUI.EndGroup();
        }
    }
}
