using EventHorizon.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.UserInterface
{
    public enum Disposition { Vertical, Horizontal };

    public abstract class GuiRenderer : MonoBehaviour
    {
        public GUISkin MainSkin;
        public GUISkin StorylineSkin;
        public Color guiColor = Color.white;
        protected float visibility = 1F;
        private bool GuiEnabled;
        protected Rect container;

        public Texture2D background;

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {
            MainSkin = Engine.Instance.MainSkin;
            StorylineSkin = Engine.Instance.StorylineSkin;
        }

        protected abstract void ComputeUIRectangles();

        public static Rect[] GetAreaRectangles(Rect area, int number, float space, Disposition d)
        {
            Rect[] result = new Rect[number];

            for (int i = 0; i < number; i++)
            {
                float x = area.x;
                float y = i * (area.height / number);
                float width = area.width;
                float height = area.height / number - space / 2;

                result[i] = new Rect(x, y, width, height);
            }

            return result;
        }

        public virtual void OnGUI()
        {
            if (GuiEnabled)
            {
                if (background != null)
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

                Draw();
            }
        }

        protected virtual void Show()
        {
            GuiEnabled = true;
        }
        
        protected virtual void Hide()
        {
            GuiEnabled = false;
        }
        
        protected abstract void Draw();
    }
}

