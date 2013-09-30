using EventHorizonGame.AI;
using EventHorizonGame.Sound;
using EventHorizonGame.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public delegate void Event();
    public delegate void EventLevel(string level);
    public delegate void EventMobile(object sender, MobileArgs args);

    public class EventHorizon : MonoBehaviour
    {
        public Player player;

        [NonSerialized]
        public static EventHorizon Instance;

        private GameObject SpawnArea;
        private GameObject GameArea;
        private GameObject VoidArea;

        public Transform mobileParent;

        public bool USE_PLACEHOLDERS = false;
        public Material PLACEHOLDER;

        public Vector3 STARTING_POSITION;

        MainMenu mainMenu;

        public GUISkin MainSkin;

        public event Event OnUserRequestShowMainMenu;
        public event Event OnUserRequestHideMainMenu;
        public event Event OnUserRequestLeaveGame;
        public event Event OnPoolLoaded;
        public event EventLevel OnEnterScene;
        public event EventLevel OnLevelLoaded;

        private void ListenToKeyboard()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                OnUserRequestShowMainMenu();

            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                LoadLevel("EndCredits");
            }
        }

        void InitializeDebugSettings()
        {
            if (USE_PLACEHOLDERS)
            {
                MeshRenderer[] renderers = UnityEngine.Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];

                foreach (MeshRenderer r in renderers)
                {
                    for (int i = 0; i < r.materials.Length; i++)
                    {
                        r.materials[i] = PLACEHOLDER;
                    }
                }
            }
        }

        void Start()
        {
            OnLevelLoaded("Menu");
        }

        void Quit()
        {
            Application.Quit();
        }

        void LoadLevel(string level)
        {
            StartCoroutine(ExecuteLoadingSequene(level));
        }

        void EnterGame()
        {
            LoadLevel("Main");
            OnUserRequestHideMainMenu();
        }

        void LeaveGame()
        {
            EnemyAI.Instance.Stop();
            Pool.Instance.Stop();
            LoadLevel("EndCredits");
        }

        IEnumerator ExecuteLoadingSequene(string level)
        {
            OnLevelLoaded(level);
            Application.LoadLevelAdditive(level);

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            if (level == "Main")
                Initialize();
        }

        void Play()
        {
            Time.timeScale = 1F;
        }

        void Pause()
        {
            Time.timeScale = 0F;
        }

        void Awake()
        {
            Instance = this;
            mainMenu = GetComponent<MainMenu>();
            mainMenu.OnUserRequestEnterGame += EnterGame;
            mainMenu.OnUserRequestLeave += LeaveGame;
            mainMenu.OnRequestPause += Pause;
            mainMenu.OnRequestPlay += Play;
        }

        void Initialize()
        {
            player = gameObject.AddComponent<Player>();
            gameObject.AddComponent<EnemyAI>();
            gameObject.AddComponent<Pool>();
            gameObject.AddComponent<HUD>();

            if (OnPoolLoaded != null)
                OnPoolLoaded();

            InitializeDebugSettings();

            EnemyAI.Instance.Run();
        }

        void Update()
        {
            ListenToKeyboard();
        }
    }
}

