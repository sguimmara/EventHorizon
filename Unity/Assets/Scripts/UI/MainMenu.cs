﻿using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class MainMenu : GuiRenderer
    {
        public event Event OnUserRequestLeave;
        public event Event OnUserRequestEnterGame;
        public event Event OnRequestPlay;
        public event Event OnRequestPause;

        bool UIEnabled = true;

        bool firstTime = true;

        Rect menuRect;
        Rect titleRect;
        Rect[] buttons;

        public Texture2D title;

        // Use this for initialization
        void Start()
        {
            EventHorizon.Instance.OnUserRequestHideMainMenu += Hide;
            EventHorizon.Instance.OnUserRequestShowMainMenu += Show;
            ComputeUIRectangles();
        }

        void Show()
        {
            //UIEnabled = true;
            OnRequestPause();
            StartCoroutine(FadeGUI(1F, 0.2F));
        }

        void Hide()
        {
            //UIEnabled = false;
            OnRequestPlay();
            StartCoroutine(FadeGUI(0F, 0.1F));
        }

        IEnumerator FadeGUI(float newValue, float duration)
        {
            float f = 0;
            float origValue = visibility;

            while (f <= duration)
            {
                visibility = Mathf.Lerp(origValue, newValue, f / duration);
                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            visibility = visibility > 0.98 ? 1 : 0;
        }

        protected override void ComputeUIRectangles()
        {
            base.ComputeUIRectangles();
            container = new Rect(-1, -1, Screen.width + 2, Screen.height + 2);
            titleRect = new Rect(container.width / 2 - 512, 100, 1024, 206);
            menuRect = new Rect(container.width / 2 - 150, container.height / 2 - 50, 300, 200);

            buttons = GuiRenderer.GetButtons(new Rect(0, 0, menuRect.width, menuRect.height), 2, 60F, Disposition.Vertical);
        }

        void OnGUI()
        {
            if (UIEnabled)
            {
                GUI.skin = EventHorizon.Instance.MainSkin;
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, visibility);

                GUI.DrawTexture(container, background);
                GUI.DrawTexture(titleRect, title);

                GUI.BeginGroup(menuRect);

                if (firstTime)
                {
                    if (GUI.Button(buttons[0], "Start"))
                    {
                        firstTime = false;
                        OnUserRequestEnterGame();
                        visibility = 0;
                    }
                }

                else
                {
                    if (GUI.Button(buttons[0], "Resume"))
                    {
                        Hide();
                    }
                }

                if (GUI.Button(buttons[1], "Quit"))
                {
                    Hide();
                    OnUserRequestLeave();
                }

                GUI.EndGroup();
            }
        }
    }
}