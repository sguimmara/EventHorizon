using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon.UserInterface
{
    public class StartScreen : GuiRenderer
    {
        public GUISkin MenuSkin;

        Rect eventRect;
        public Texture2D eventText;
        public float timeForEvent;
        float eventOpacity = 0F;

        Rect horizonRect;
        public Texture2D horizonText;
        public float timeForHorizon;
        float horizonOpacity = 0F;

        Rect startButtonRect;
        float startButtonOpacity = 0F;

        float backgroundOpacity;

        //float backgroundOpacity;

        public override void Awake()
        {
            base.Awake();
            ComputeUIRectangles();
            
        }

        public override void Start()
        {
            base.Start();
            StartCoroutine(DisplayMenuProgressively());
        }

        IEnumerator FadeOutAndExecute(Action method)
        {
            float f = 0;
            float t = 0;
            float duration = 4F;

            while (f <= duration)
            {
                t = f / duration;
                backgroundOpacity = Mathf.Lerp(backgroundOpacity, 0F, t);
                eventOpacity = Mathf.Lerp(eventOpacity, 0F, t);
                horizonOpacity = Mathf.Lerp(horizonOpacity, 0F, t);
                startButtonOpacity = Mathf.Lerp(startButtonOpacity, 0F, t);
                audio.volume = Mathf.Lerp(audio.volume, 0F, t);
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            method();
        }

        IEnumerator DisplayMenuProgressively()
        {
            yield return new WaitForSeconds(2);

            float f = 0;

            while (f <= timeForEvent)
            {
                eventOpacity = f / timeForEvent;
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            f = 0;

            while (f <= timeForHorizon)
            {
                horizonOpacity = f / timeForHorizon;
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            f = 0;

            while (f <= 0.2F)
            {
                startButtonOpacity = f / 0.2F;
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        void Load()
        {
            Application.LoadLevel(2);
        }

        protected override void ComputeUIRectangles()
        {
            float width = Screen.width;
            float height = Screen.height;

            container = new Rect(0, 0, width, height);
            eventRect = new Rect(0.1696F * width, 0.2629F * height, 0.439F * width, 0.1916F * height);
            horizonRect = new Rect(0.252F * width, 0.511F * height, 0.603F * width, 0.19722F * height);
            startButtonRect = new Rect(0.165F * width, 0.769F * height, 0.1398F * width, 0.0898F * height);
        }

        public override void OnGUI()
        {
            Draw();
        }

        protected override void Draw()
        {
            GUI.skin = MenuSkin;

            GUI.color = new Color(1, 1, 1, backgroundOpacity);
            GUI.DrawTexture(container, background);

            GUI.color = new Color(1, 1, 1, eventOpacity);
            GUI.DrawTexture(eventRect, eventText);

            GUI.color = new Color(1, 1, 1, horizonOpacity);
            GUI.DrawTexture(horizonRect, horizonText);

            GUI.color = new Color(1, 1, 1, startButtonOpacity);
            if (GUI.Button(startButtonRect, "", "StartButton"))
                StartCoroutine(FadeOutAndExecute(Load));
        }
    }
}
