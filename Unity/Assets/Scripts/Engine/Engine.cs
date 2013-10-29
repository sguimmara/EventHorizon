using EventHorizon.AI;
using EventHorizon.Objects;
using EventHorizon.Sound;
using EventHorizon.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace EventHorizon.Core
{
    public delegate void GameEvent();
    public delegate void EventLevel(Level level);
    public delegate void EventMobile(Mobile sender);

    public class Engine : MonoBehaviour
    {
        public bool DESIGN_MODE;
        public Player playerShip;

        public Texture2D black;

        public static Engine Instance;

        public bool USE_PLACEHOLDERS = false;
        public Material PLACEHOLDER;

        public Vector3 STARTING_POSITION;
        public Vector3 END_POSITION;

        MainMenu mainMenu;
        ScoreScreen scoreScreen;

        public Player player { get; private set; }

        public GUISkin MainSkin;
        public GUISkin StorylineSkin;

        public Level CurrentLevel;
        int currentLevelIndex;
        Level[] levels;

        public LevelPhase levelPhase;

        float visibility = 0;

        List<Ship> ships;

        public event GameEvent OnUserRequestShowMainMenu;
        //public event GameEvent OnUserRequestHideMainMenu;
        //public event GameEvent OnUserRequestLeaveGame;
        //public event EventLevel OnEnterLevel;
        public event EventLevel OnLevelLoaded;
        public event EventMobile OnShipDestroyed;

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
            currentLevelIndex = 0;
            MoveToInitPhase();
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
            //            player.IsPlayable = !player.IsPlayable;
        }

        void Start()
        {
            if (!DESIGN_MODE)
                mainMenu.Launch();
        }

        void Awake()
        {
            Instance = this;

            mainMenu = GetComponent<MainMenu>();
            mainMenu.OnUserRequestEnterGame += StartGame;
            mainMenu.OnUserRequestLeave += LeaveGame;

            scoreScreen = GetComponent<ScoreScreen>();
            scoreScreen.OnScoreFinished += MoveToNextLevel;

            DontDestroyOnLoad(this);
			
			if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            mainMenu.Init();

            scoreScreen.Init();

            Initialize();
        }

        void Initialize()
        {
            currentLevelIndex = 0;
            DeserializeLevels();
            InitializeDebugSettings();
        }

        void MoveToInitPhase()
        {
            CurrentLevel = levels[currentLevelIndex];
            levelPhase = LevelPhase.Init;
            mainMenu.ShutDown();

            scoreScreen.ShutDown();

            CurrentLevel.Load();
            MoveToIntroPhase();
        }

        void MoveToIntroPhase()
        {
            levelPhase = LevelPhase.Intro;
        }

        void MoveToUpgradePhase()
        {
            levelPhase = LevelPhase.Upgrade;
        }

        void MoveToGamePhase()
        {
            levelPhase = LevelPhase.Game;

        }

        void MoveToScorePhase()
        {
            levelPhase = LevelPhase.Score;
            scoreScreen.Launch();
        }

        void MoveToNextLevel()
        {
            currentLevelIndex++;
            MoveToInitPhase();
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
            StartCoroutine(FadeOut());
            StartCoroutine(ForcePlayerToLocation(END_POSITION));
        }

        IEnumerator FadeOut()
        {
            float f = 0;

            while (f <= 1)
            {
                visibility = f;
                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        void OnGUI()
        {
            GUI.color = new Color(0, 0, 0, visibility);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
        }

        void DeserializeLevels()
        {
            string text = ((TextAsset)Resources.Load("Levels")).text;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(text);

            XmlNodeList list = doc.SelectNodes("levels/level");
            levels = new Level[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                int levelNumber = 0;

                if (!int.TryParse(list[i].SelectSingleNode("number").InnerText, out levelNumber))
                    Debug.LogError("Error parsing Levels.xml, invalid level number " + list[i].SelectSingleNode("number").InnerText);

                string name = list[i].SelectSingleNode("name").InnerText;

                if (name == "")
                    Debug.LogWarning("Parsing Levels.Xml, level name empty");

                string intro = list[i].SelectSingleNode("intro").InnerText;

                if (intro == "")
                    Debug.LogWarning("Parsing Levels.Xml, level intro empty");

                levels[i] = new Level(name, levelNumber, ((TextAsset)Resources.Load(intro)).text);
            }

            foreach (Level level in levels)
            {
                Debug.Log(level.ToString() + " added to level list");
            }
        }

        public void AddShip(Ship ship)
        {
            ship.OnDestroy += delegate(Mobile m) { OnShipDestroyed(m as Ship); };
        }

        void MoveToNonPlayablePhase()
        {
            StartCoroutine(ForcePlayerToLocation(STARTING_POSITION));
        }
          
        void MoveToPlayablePhase()
        {
            player.IsPlayable = true;
        }

        // In non playable phases, the player's ship must go back to the center of the screen.
        IEnumerator ForcePlayerToLocation(Vector3 location)
        {
            player.IsPlayable = false;
            Vector3 delta;

            while (Vector3.Distance(player.transform.position, location) > 0.01F)
            {
                delta = location - player.transform.position;
                player.transform.position += delta * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

