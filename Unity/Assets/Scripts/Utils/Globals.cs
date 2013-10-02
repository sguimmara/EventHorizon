using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EventHorizon
{
    public class Globals : MonoBehaviour
    {
        public static Rect SpawnArea;
        public static Rect GameArea;
        public static Rect VoidArea;

        void Awake()
        {
            Globals.InitializeAreaRects();

            Debug.Log(VoidArea.x);
            Debug.Log(VoidArea.y);
            Debug.Log(VoidArea.width);
            Debug.Log(VoidArea.height);
        }

        public static void InitializeAreaRects()
        {
            GameObject SpawnAreaGo = GameObject.Find("SpawnArea");
            GameObject GameAreaGo = GameObject.Find("GameArea");
            GameObject VoidAreaGo = GameObject.Find("VoidArea");

            if (SpawnAreaGo != null)
            {
                Bounds b = SpawnAreaGo.GetComponent<MeshRenderer>().bounds;
                SpawnArea.x = b.min.x;
                SpawnArea.y = b.min.y;
                SpawnArea.width = b.max.x - b.min.x;
                SpawnArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("SpawnArea null");

            if (VoidAreaGo != null)
            {
                Bounds b = VoidAreaGo.GetComponent<MeshRenderer>().bounds;
                VoidArea.x = b.min.x;
                VoidArea.y = b.min.y;
                VoidArea.width = b.max.x - b.min.x;
                VoidArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("VoidArea null");

            if (GameAreaGo)
            {
                Bounds b = GameAreaGo.GetComponent<MeshRenderer>().bounds;
                GameArea.x = b.min.x;
                GameArea.y = b.min.y;
                GameArea.width = b.max.x - b.min.x;
                GameArea.height = b.max.y - b.min.y;
            }
            else Debug.LogError("GameArea null");
        }
    }

}
