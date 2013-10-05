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
        UpgradeScreen upgradeScreen;
        ScoreScreen scoreScreen;

        public Player player { get; private set; }

        public GUISkin MainSkin;
        public GUISkin StorylineSkin;

        Level CurrentLevel;
        int currentLevelIndex;
        Level[] levels;

        public GameData GameData;
        public LevelPhase levelPhase;

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
            levelPhase = LevelPhase.Init;
            mainMenu.ShutDown();
            cutScene.ShutDown();
            ingameUi.ShutDown();
            conversationUi.ShutDown();
            upgradeScreen.ShutDown();
            scoreScreen.ShutDown();

            levels[0].Load();
            MoveToIntroPhase();
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

        void MoveToIntroPhase()
        {
            levelPhase = LevelPhase.Intro;
            cutScene.Launch();
        }

        void MoveToUpgradePhase()
        {
            levelPhase = LevelPhase.Upgrade;
            cutScene.ShutDown();
            upgradeScreen.Launch();
        }

        void MoveToGamePhase()
        {
            levelPhase = LevelPhase.Game;
            upgradeScreen.ShutDown();
        }

        void MoveToScorePhase()
        {
            levelPhase = LevelPhase.Score;
            scoreScreen.Launch();
        }

        void MoveToNextLevel()
        {
            mainMenu.ShutDown();
            cutScene.ShutDown();
            ingameUi.ShutDown();
            conversationUi.ShutDown();
            upgradeScreen.ShutDown();
            scoreScreen.ShutDown();
            levelPhase = LevelPhase.Init;

            levels[currentLevelIndex].Load();

            MoveToIntroPhase();
        }

        void Awake()
        {
            Instance = this;
            mainMenu = GetComponent<MainMenu>();
            mainMenu.OnUserRequestEnterGame += StartGame;
            mainMenu.OnUserRequestLeave += LeaveGame;
            mainMenu.OnRequestPause += Pause;
            mainMenu.OnRequestPlay += Play;
            mainMenu.OnMainMenuOff += SwitchPlayablePhase;
            mainMenu.OnMainMenuOn += SwitchPlayablePhase;

            conversationUi = GetComponent<ConversationUi>();
            conversationUi.OnDialogueFinished += SwitchPlayablePhase;
            conversationUi.OnDialogueStarted += SwitchPlayablePhase;

            ingameUi = GetComponent<IngameUi>();

            cutScene = GetComponent<Cutscene>();
            cutScene.OnCutsceneFinished += MoveToUpgradePhase;

            upgradeScreen = GetComponent<UpgradeScreen>();
            upgradeScreen.OnUpgradeFinished += MoveToGamePhase;

            scoreScreen = GetComponent<ScoreScreen>();
            scoreScreen.OnScoreFinished += MoveToNextLevel;

            DontDestroyOnLoad(this);

            CreatePlayer();

            Application.LoadLevel(1);
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

        public void ReachEndOfLevel()
        {
            Debug.Log("End of level reached.");
            MoveToScorePhase();
        }

        void DeserializeLevels()
        {

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

