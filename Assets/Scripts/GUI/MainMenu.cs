using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class MainMenu : GuiRenderer
    {
        public event Event OnLevelLoaded;

        bool firstTime = true;

        Rect menuRect;
        Rect titleRect;
        Rect[] buttons;

        public Texture2D title;

        // Use this for initialization
        void Start()
        {
            OnLevelLoaded += delegate { StartCoroutine(FadeGUI(0F, 1F)); };
            EventHorizon.Instance.OnUserRequestMainMenu += delegate { StartCoroutine(FadeGUI(1F, 0.1F)); };
            ComputeUIRectangles();
        }

        IEnumerator LoadLevel()
        {
            Application.LoadLevelAdditive("Main");

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            OnLevelLoaded();
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
                    StartCoroutine(LoadLevel());
                }
            }

            else
            {
                if (GUI.Button(buttons[0], "Resume"))
                    StartCoroutine(FadeGUI(0F, 0.1F));
            }

            if (GUI.Button(buttons[1], "Quit"))
                Application.Quit();

            GUI.EndGroup();
        }
    }
}
