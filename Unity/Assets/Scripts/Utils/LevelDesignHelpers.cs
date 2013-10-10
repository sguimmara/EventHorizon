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
    public class LevelDesignHelpers : MonoBehaviour
    {
        public bool Active;
        float LevelLength;
        int LevelDuration;
        public bool VerticalLimits;
        public bool Graduations;
        public bool GameArea;
        LevelSlider levelSlider;

        static float speed;
        public Color centerLine;
        public Color gameLimits;
        public Color viewLimits;
        public Color graduation;
        public Color gameArea;

        const float upperLimit = 4.146766F;
        const float labelHeight = 7F;

        Matrix4x4 handleMatrix;

        void DrawLevelVerticalLimits()
        {
            Gizmos.color = viewLimits;
            Gizmos.DrawLine(new Vector3(transform.position.x, upperLimit, 10), new Vector3(transform.position.x + LevelLength, upperLimit, 10));
            Gizmos.DrawLine(new Vector3(transform.position.x, -upperLimit, 10), new Vector3(transform.position.x + LevelLength, -upperLimit, 10));
            Gizmos.color = gameLimits;
            Gizmos.DrawLine(new Vector3(transform.position.x, 2.072079F, 5), new Vector3(transform.position.x + LevelLength, 2.072079F, 5));
            Gizmos.DrawLine(new Vector3(transform.position.x, -2.072079F, 5), new Vector3(transform.position.x + LevelLength, -2.072079F, 5));
            Gizmos.color = centerLine;
            Gizmos.DrawLine(new Vector3(transform.position.x, 0, 5), new Vector3(transform.position.x + LevelLength, 0, 5));
        }

        public static string GetTimeString(float x)
        {           
            float seconds = (int)(Mathf.Abs(x) / speed);
            float minutes = (int)seconds / 60;
            seconds = seconds % 60;

            return string.Concat(minutes, ":", seconds < 10 ? string.Concat("0",seconds.ToString()) : seconds.ToString());
        }

        void DrawLevelExtremities()
        {
            Handles.Label(new Vector3(0, labelHeight + 2, 5), GetTimeString(transform.position.x));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(transform.position.x, upperLimit, 5), new Vector3(transform.position.x, -upperLimit, 5));
            Handles.Label(new Vector3(transform.position.x, labelHeight + 1, 5), "Start");
            Handles.Label(new Vector3(transform.position.x + LevelLength, labelHeight + 1, 5), "End");
            Gizmos.DrawLine(new Vector3(transform.position.x + LevelLength, upperLimit, 5), new Vector3(transform.position.x + LevelLength, -upperLimit, 5));
        }

        void DrawGameArea()
        {
            Gizmos.color = gameArea;
            Gizmos.DrawLine(new Vector3(-3.681635F, 2.059772F, 5), new Vector3(-3.681635F, -2.059772F, 5));
            Gizmos.DrawLine(new Vector3(3.67029F, 02.059772F, 5), new Vector3(3.67029F, -2.059772F, 5));
            Gizmos.DrawLine(new Vector3(-3.681635F, 2.059772F, 5), new Vector3(3.67029F, 2.059772F, 5));
            Gizmos.DrawLine(new Vector3(-3.681635F, -2.059772F, 5), new Vector3(3.67029F, -2.059772F, 5));
        }

        void DrawHorizontalGraduations()
        {
            Gizmos.color = graduation;

            for (int i = 0; i < LevelLength / (float)levelSlider.speed; i++)
            {
                Gizmos.DrawLine(new Vector3(transform.position.x + i * levelSlider.speed, 4.1467666F, 10), new Vector3(transform.position.x + i * levelSlider.speed, -4.1467666F, 10));
            }
        }

        void Awake()
        {
            levelSlider = GetComponent<LevelSlider>();
        }

        void OnDrawGizmos()
        {
            if (Active)
            {
                if (levelSlider == null)
                {
                    levelSlider = GetComponent<LevelSlider>();
                }

                speed = levelSlider.speed;

                LevelLength = levelSlider.DurationInMinutes * 60 * levelSlider.speed;

                if (VerticalLimits)
                    DrawLevelVerticalLimits();

                if (Graduations)
                    DrawHorizontalGraduations();

                if (GameArea)
                    DrawGameArea();

                DrawLevelExtremities();
            }
        }
    }

}
