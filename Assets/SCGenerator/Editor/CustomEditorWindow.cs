using Assets.SCGenerator.Scripts.BO.Base;
using Assets.SCGenerator.Scripts.BO.City;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.SCGenerator.Editor
{
    public class CustomEditorWindow : EditorWindow
    {
        public void GenerateStreetTab()
        {
            // Folder
            BaseCity.RoadListDirectory = GenerateFolderPathGUI("Street", BaseCity.RoadListDirectory);
            foreach (var item in BaseCity.RoadList)
            {
                EditorGUILayout.ObjectField(item.name, item, typeof(GameObject), false);
            }
            //GUILayout.FlexibleSpace();
            //if (GUILayout.Button("Refresh Street"))
            //{
            //    BaseCity.RefreshAssets();
            //}
        }

        public void GenerateBuildingTab()
        {
            // Folder
            BaseCity.BuildingListDirectory = GenerateFolderPathGUI("Building", BaseCity.BuildingListDirectory);
            foreach (var item in BaseCity.BuildingList)
            {
                EditorGUILayout.ObjectField(item.name, item, typeof(GameObject), false);
            }
            //GUILayout.FlexibleSpace();
            //if (GUILayout.Button("Refresh Building"))
            //{
            //    BaseCity.RefreshAssets();
            //}
        }

        public void GenerateLightTab()
        {
            // Light
            BaseCity.LightListDirectory = GenerateFolderPathGUI("Light", BaseCity.LightListDirectory);
            foreach (var item in BaseCity.LightList)
            {
                EditorGUILayout.ObjectField(item.name, item, typeof(GameObject), false);
            }

            //GUILayout.FlexibleSpace();
            //if (GUILayout.Button("Refresh Light"))
            //{
            //    BaseCity.RefreshAssets();
            //}
        }

        public void GeneratePropsTab()
        {
            // Prop
            BaseCity.PropListDirectory = GenerateFolderPathGUI("Prop", BaseCity.PropListDirectory);
            foreach (var item in BaseCity.PropList)
            {
                EditorGUILayout.ObjectField(item.name, item, typeof(GameObject), false);
            }

            //GUILayout.FlexibleSpace();
            //if (GUILayout.Button("Refresh Prop"))
            //{
            //    BaseCity.RefreshAssets();
            //}
        }

        public static string GenerateFolderPathGUI(string name, string folderPath)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            GUILayout.TextField(folderPath, GUILayout.MaxWidth(350));
            if (GUILayout.Button("...📁", GUILayout.Width(30)))
            {
                string path = EditorUtility.OpenFolderPanel(name + " folder", folderPath, "");
                if (!string.IsNullOrEmpty(path))  //recording
                {
                    folderPath = path.Substring(path.IndexOf("Assets"));
                    Debug.Log($"📁 Folder {name} Updated :\n{path}");
                }
            }
            GUILayout.EndHorizontal();
            return folderPath;
        }
    }
}
