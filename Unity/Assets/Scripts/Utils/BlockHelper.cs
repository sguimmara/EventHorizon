using EventHorizon.Core;
using EventHorizon.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EventHorizon.Helpers
{
    [Serializable]
    public class DesignLine
    {
        Vector3 start;
        Vector3 end;
        string label;
    }

    public class BlockHelper : MonoBehaviour
    {
        LevelSlider levelSlider;

        public Texture2D blockZoneTex;

        float BlockLength;
        public int BlockDurationInSeconds;

        public bool DisplayBlockLimits;
        public Color BlockColor;

        public DesignLine[] customLines;

        const float upperLimit = 4.146766F;
        const float labelHeight = 7F;

        Vector3 startTop;
        Vector3 startBottom;
        Vector3 endTop;
        Vector3 endBottom;

        void DrawBlockLimits()
        {
            float seconds = (int)(Mathf.Abs(transform.position.x) / levelSlider.speed);
            float minutes = (int)seconds / 60;
            seconds = seconds % 60;

            string time = string.Concat(minutes.ToString(), ":", seconds.ToString());

            Gizmos.color = BlockColor;
            Handles.Label(new Vector3(transform.position.x, labelHeight, 5), string.Concat(name, " - ", LevelDesignHelpers.GetTimeString(transform.position.x)));
            Gizmos.DrawLine(new Vector3(transform.position.x, upperLimit, 5), new Vector3(transform.position.x, -upperLimit, 5));
            Gizmos.DrawLine(new Vector3(transform.position.x + BlockLength, upperLimit, 5), new Vector3(transform.position.x + BlockLength, -upperLimit, 5));

        }

        void Awake()
        {
            levelSlider = GetComponent<LevelSlider>();
        }

        void OnDrawGizmos()
        {
            startTop = new Vector3(transform.position.x, upperLimit, 5);
            startBottom = new Vector3(transform.position.x, -upperLimit, 5);
            endTop = new Vector3(transform.position.x + BlockLength, upperLimit, 5);
            endBottom = new Vector3(transform.position.x + BlockLength, -upperLimit, 5);

            if (levelSlider == null)
                levelSlider = GameObject.Find("LevelSlider").GetComponent<LevelSlider>();

            BlockLength = BlockDurationInSeconds * levelSlider.speed;

            if (DisplayBlockLimits)
                DrawBlockLimits();
        }

        void OnDrawGizmosSelected()
        {
            Rect blockZone = new Rect(startTop.x, startTop.y, endTop.x - startTop.x, startBottom.y - startTop.y);

            Gizmos.DrawGUITexture(blockZone, blockZoneTex);
        }
    }

}
