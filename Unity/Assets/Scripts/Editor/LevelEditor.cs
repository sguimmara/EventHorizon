using EventHorizon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EventHorizon.Helpers
{
    public class LevelEditor : EditorWindow
    {
        static LevelController slider;
		static float sliderValue;
		static float maxValue;

        [MenuItem("Event Horizon/Level Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
            
            slider = GameObject.Find("LevelSlider").GetComponent<LevelController>();
			
        }

        void MirrorOnCenterLine()
        {
            GameObject t;
            foreach (UnityEngine.Object o in Selection.objects)
            {
                t = o as GameObject;
                t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y * -1, t.transform.position.z);
            }
        }

        void Rewind()
        {
            slider.transform.position = new Vector3(0, slider.transform.position.y, slider.transform.position.z);
        }

        void OnGUI()
        {			
            GUILayout.Label("Level : " + Application.loadedLevelName, EditorStyles.boldLabel);
			
			if (slider != null && slider.audio != null && slider.audio.clip != null)
			{
				GUILayout.Label("Track : " + slider.audio.clip.name, EditorStyles.boldLabel);
			maxValue = slider.audio.clip.length * slider.speed;
								sliderValue = GUILayout.HorizontalSlider(sliderValue, 0, maxValue);
			slider.transform.position = new Vector3(-sliderValue, slider.transform.position.y, slider.transform.position.z);
				
			}


//            if (GUILayout.Button("Rewind"))
//                Rewind();

            GUILayout.Label("\nSelection", EditorStyles.boldLabel);
            if (GUILayout.Button("Mirror on center line"))
                MirrorOnCenterLine();
            GUILayout.Button("Mirror on Y");
            GUILayout.Button("Mirror on X");
        }
    }


}