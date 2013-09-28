using EventHorizonGame.AI;
using EventHorizonGame.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public delegate void Event();

    public class EventHorizon : MonoBehaviour
    {
        Player player;
        
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

        public event Event OnUserRequestMainMenu;

        private void ListenToKeyboard()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                OnUserRequestMainMenu();
        }

        void InitializeAreaRects()
        {
            SpawnArea = GameObject.Find("SpawnArea");
            GameArea = GameObject.Find("GameArea");
            VoidArea = GameObject.Find("VoidArea");

            if (SpawnArea != null)
            {
                Bounds b = SpawnArea.GetComponent<MeshRenderer>().bounds;
                Globals.SpawnArea.x = b.min.x;
                Globals.SpawnArea.y = b.min.y;
                Globals.SpawnArea.width = b.max.x - b.min.x;
                Globals.SpawnArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("SpawnArea null");

            if (VoidArea != null)
            {
                Bounds b = VoidArea.GetComponent<MeshRenderer>().bounds;
                Globals.VoidArea.x = b.min.x;
                Globals.VoidArea.y = b.min.y;
                Globals.VoidArea.width = b.max.x - b.min.x;
                Globals.VoidArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("VoidArea null");

            if (GameArea)
            {
                Bounds b = GameArea.GetComponent<MeshRenderer>().bounds;
                Globals.GameArea.x = b.min.x;
                Globals.GameArea.y = b.min.y;
                Globals.GameArea.width = b.max.x - b.min.x;
                Globals.GameArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("GameArea null");
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

        void Quit()
        {
            Application.Quit();
        }

        void Start()
        {
            Instance = this;
            mainMenu = GetComponent<MainMenu>();
            mainMenu.OnLevelLoaded += Initialize;
        }

        void Initialize()
        {
            player = gameObject.AddComponent<Player>();
            gameObject.AddComponent<EnemyAI>();
            gameObject.AddComponent<Pool>();

            InitializeAreaRects();
            InitializeDebugSettings();

            EnemyAI.Instance.Run();
        }

        void Update()
        {
            ListenToKeyboard();
        }
    }
}

