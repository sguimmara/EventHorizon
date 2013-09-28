using UnityEngine;
using System.Collections;

namespace EventHorizonGame.UserInterface
{
    public class MainMenu : GuiRenderer
    {
        public event Event OnLevelLoaded;

        bool firstTime = true;

        Rect menuRect;
        Rect[] buttons;

        // Use this for initialization
        void Start()
        {
            OnLevelLoaded += delegate { StartCoroutine(FadeGUI(0F, 1F)); };
            EventHorizon.Instance.OnUserRequestMainMenu += delegate { StartCoroutine(FadeGUI(1F, 0.1F)); };
            ComputeUIRectangles();
        }

        void LoadLevel()
        {
            Application.LoadLevelAdditive("Main");
            OnLevelLoaded();
        }

        IEnumerator FadeGUI(float newValue, float duration)
        {
            Debug.Log("fade");
            float f = 0;
            float origValue = visibility;

            while (f <= duration)
            {
                visibility = Mathf.Lerp(origValue, newValue, f / duration);
                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }

            //GUI.enabled = f == 1;
        }

        protected override void ComputeUIRectangles()
        {
            base.ComputeUIRectangles();
            container = new Rect(0, 0, Screen.width, Screen.height);
            menuRect = new Rect(container.width / 2 - 150, container.height / 2 - 50, 300, 100);

            buttons = GuiRenderer.GetButtons(new Rect(0, 0, menuRect.width, menuRect.height), 2, 60F, Disposition.Vertical);
        }

        void OnGUI()
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, visibility);

            GUI.DrawTexture(container, background);

            GUI.BeginGroup(menuRect);

            if (firstTime)
            {
                if (GUI.Button(buttons[0], "Start"))
                {
                    firstTime = false;
                    LoadLevel();
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
