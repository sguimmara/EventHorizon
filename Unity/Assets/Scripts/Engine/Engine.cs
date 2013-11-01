using EventHorizon.Objects;
using UnityEngine;

namespace EventHorizon.Core
{
    public delegate void GameEvent();
    public delegate void EventLevel(Level level);
    public delegate void EventMobile(Mobile sender);

    public class Engine : MonoBehaviour, ISingleton
    {
        public static Engine Instance { get; private set; }

        Crystal[] crystals;
        public bool ProblemSolved { get; private set; }

        private void Awake()
        {
            Register();
            crystals = Object.FindObjectsOfType(typeof(Crystal)) as Crystal[];
        }

        private void Update()
        {
            if (!ProblemSolved)
                CheckForProblemSolved();
        }

        public void Register()
        {
            if (Instance == null)
                Instance = this;
            else Debug.LogError(string.Format("Singleton marked class {0} has more than one instance running.", this.GetType().ToString()));
        }

        private void OnProblemSolved()
        {
            ProblemSolved = true;
            Debug.Log("PROBLEM SOLVED");
            Application.CaptureScreenshot("IconicDeception_solved.png");
        }

        private void CheckForProblemSolved()
        {
            foreach (Crystal crystal in crystals)
            {
                if (!crystal.Activated)
                {
                    return;
                }
            }

            OnProblemSolved();
        }
    }
}

