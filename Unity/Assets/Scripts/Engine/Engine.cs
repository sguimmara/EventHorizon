using EventHorizon.AI;
using EventHorizon.Objects;
using EventHorizon.Sound;
using EventHorizon.Storyline;
using EventHorizon.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EventHorizon.Core
{
    public delegate void GameEvent();
    public delegate void EventLevel(Level level);
    public delegate void EventMobile(Mobile sender);

    public class Engine : MonoBehaviour
    {
        public Player playerShip;

        public static Engine Instance;

        public bool USE_PLACEHOLDERS = false;
        public Material PLACEHOLDER;

        public Vector3 STARTING_POSITION;

        MainMenu mainMenu;
        Cutscene cutScene;
        IngameUi ingameUi;
        ConversationUi conversationUi;

        public Player player { get; private set; }

        public GUISkin MainSkin;
        public GUISkin StorylineSkin;

        Level CurrentLevel;

        public GameData GameData;

        public event GameEvent OnUserRequestShowMainMenu;
        //public event GameEvent OnUserRequestHideMainMenu;
        //public event GameEvent OnUserRequestLeaveGame;
        //public event EventLevel OnEnterLevel;
        public event EventLevel OnLevelLoaded;

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

        void LoadLevel(Level level)
        {
            //Application.LoadLevelAdditive((int)level);
            Debug.LogException(new NotImplementedException());

            OnLevelLoaded(level);
        }

        void StartGame()
        {
            //LoadLevel();
            Debug.LogException(new NotImplementedException());
        }

        void LeaveGame()
        {
            Quit();
        }

        void SaveGame()
        {
            Debug.LogException(new NotImplementedException());
        }

        void LoadGame()
        {
            //LoadLevel("");
            //EnterLevel();
            Debug.LogException(new NotImplementedException());
        }

        void Play()
        {
            Time.timeScale = 1F;
        }

        void Pause()
        {
            Time.timeScale = 0F;
        }        

        void SwitchPlayablePhase()
        {
            player.IsPlayable = !player.IsPlayable;
        }

        void Awake()
        {
            Instance = this;
            mainMenu = GetComponent<MainMenu>();
            mainMenu.OnUserRequestEnterGame += StartGame;
            mainMenu.OnUserRequestLeave += LeaveGame;
            mainMenu.OnRequestPause += Pause;
            mainMenu.OnRequestPlay += Play;

            conversationUi = GetComponent<ConversationUi>();
            conversationUi.OnDialogueFinished += SwitchPlayablePhase;
            conversationUi.OnDialogueStarted += SwitchPlayablePhase;

            ingameUi = GetComponent<IngameUi>();

            cutScene = GetComponent<Cutscene>();
            cutScene.OnCutsceneFinished += SwitchPlayablePhase;
            cutScene.OnCutsceneStarted += SwitchPlayablePhase;

            DontDestroyOnLoad(this);

            CreatePlayer();

            Application.LoadLevel("Empty");
        }

        void Initialize()
        {
            InitializeDebugSettings();
        }

        void Update()
        {
            ListenToKeyboard();
        }

        void ListenToKeyboard()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                OnUserRequestShowMainMenu();
        }

        void CreatePlayer()
        {
            player = (Player)GameObject.Instantiate(playerShip, STARTING_POSITION, Quaternion.identity);
            DontDestroyOnLoad(player);
        }

        // Temporary
        void TEMP_CREATE_ACTORS()
        {
            Character taeresa = new Character { ID = "Taeresa", Name = "Taeresa Niemeyer", Portrait = GameData.Taeresa };
            Character marshall = new Character { ID = "Marshall", Name = "Marshall Elon", Portrait = GameData.Marshall };
            Character weilin = new Character { ID = "Weilin", Name = "Weilin Gu", Portrait = GameData.Weilin };
            Character jacob = new Character { ID = "Jacob", Name = "Jacob Freeman", Portrait = GameData.Jacob };
            Character nobody = new Character { ID = "Nobody", Name = "", Portrait = GameData.Nobody };

            CharacterPool.characters = new Character[5] { taeresa, marshall, nobody, weilin, jacob };
        }
    }
}

