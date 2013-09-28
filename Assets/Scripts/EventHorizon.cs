using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizonGame
{
    public class EventHorizon : MonoBehaviour
    {
        Player player;

        [NonSerialized]
        public static EventHorizon Instance;

        List<Mobile> mobiles;

        public GameObject SpawnArea;
        public GameObject GameArea;

        public bool USE_PLACEHOLDERS = false;
        public Material PLACEHOLDER;

        public Vector3 STARTING_POSITION;

        void InitializeAreaRects()
        {
            if (SpawnArea)
            {
                Bounds b = SpawnArea.GetComponent<MeshRenderer>().bounds;
                Globals.SpawnArea.x = b.min.x;
                Globals.SpawnArea.y = b.min.y;
                Globals.SpawnArea.width = b.max.x - b.min.x;
                Globals.SpawnArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("EventHorizon - SpawnArea is null");

            if (GameArea)
            {
                Bounds b = GameArea.GetComponent<MeshRenderer>().bounds;
                Globals.GameArea.x = b.min.x;
                Globals.GameArea.y = b.min.y;
                Globals.GameArea.width = b.max.x - b.min.x;
                Globals.GameArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("EventHorizon - GameArea is null");
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

        void Awake()
        {
           
        }

        void Start()
        {
            Instance = this;
            mobiles = new List<Mobile>();
            player = gameObject.AddComponent<Player>();

            InitializeAreaRects();
            InitializeDebugSettings();
        }

        public void AddMobile(Mobile m)
        {
            if (mobiles.Contains(m))
                Debug.LogWarning("EventHorizon - Le mobile " + m + " est déjà présent dans la liste");

            else mobiles.Add(m);
        }

        void Update()
        {

        }
    }
}

