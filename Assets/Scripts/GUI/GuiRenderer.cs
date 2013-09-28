using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame.UserInterface
{
    public enum Disposition { Vertical, Horizontal };

    public abstract class GuiRenderer : MonoBehaviour
    {
        protected float visibility = 1F;
        protected Rect container;

        public Texture2D background;



        protected virtual void ComputeUIRectangles()
        {

        }

        public static Rect[] GetButtons(Rect area, int number, float space, Disposition d)
        {
            Rect[] result = new Rect[number];

            for (int i = 0; i < number; i++)
            {
                float x = area.x;
                float y = i * (area.height / number);
                float width = area.width;
                float height = area.height / number - space /2;

                result[i] = new Rect(x, y, width, height);
            }

            return result;
        }
    }
}

