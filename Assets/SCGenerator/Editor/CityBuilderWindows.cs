using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Assets.SCGenerator.Scripts.BO.City;
using System.IO;
using System.Linq;
using Assets.SCGenerator.Editor;

public class CityBuilderWindows : CustomEditorWindow
{
    private int toolbarInt = 0;
    string[] toolbarStrings = new string[] {"City", "Resource"};


    private int resurceToolbarInt = 0;
    string[] resurceToolbarStrings = new string[] { "Street", "Building", "Light", "Props"};

    // Add menu named "City Builder" to the Window menu
    [MenuItem("Window/SCGenerator/City Builder")]
    static void Init()
    {
        var SCGenerator = new BaseCity();

        // Get existing open window or if none, make a new one:
        CityBuilderWindows window = (CityBuilderWindows)EditorWindow.GetWindow(typeof(CityBuilderWindows));
        window.Show();

    }

    void OnGUI()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        switch (toolbarInt)
        {
            case 0:
                    GenerateCityTab();
                break;
            case 1:
                    GenerateSettingTab();
                break;
        }

    }

    private void GenerateCityTab()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        BaseCity.MapWidth = EditorGUILayout.IntField("City Width", BaseCity.MapWidth);
        BaseCity.MapHeight = EditorGUILayout.IntField("City Height", BaseCity.MapHeight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        BaseCity.RandMin = EditorGUILayout.IntField("Street Min Seed", BaseCity.RandMin);
        BaseCity.RandMax = EditorGUILayout.IntField("Street Max Seed", BaseCity.RandMax);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        BaseCity.Step = EditorGUILayout.IntField("Tile Step", BaseCity.Step);
        BaseCity.BuildingFootprint = EditorGUILayout.IntField("Tile Footprint", BaseCity.BuildingFootprint);
        GUILayout.EndHorizontal();

        GUILayout.Label("Generate Settings", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        BaseCity.GenerateStreet = GUILayout.Toggle(BaseCity.GenerateStreet, "GenerateStreet");
        BaseCity.GenerateBuilding = GUILayout.Toggle(BaseCity.GenerateBuilding, "GenerateBuilding");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        BaseCity.GenerateLight = GUILayout.Toggle(BaseCity.GenerateLight, "GenerateLight");
        BaseCity.GenerateProp = GUILayout.Toggle(BaseCity.GenerateProp, "GenerateProp");
        GUILayout.EndHorizontal();

        // Generate 
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generated"))
        {
            BaseCity.Clear();
            BaseCity.Clear();
            BaseCity.GenerateCity();

            Debug.Log("Generate City");
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Delete", GUILayout.Width(100)))
        {
            BaseCity.Clear();
            BaseCity.Clear();

            Debug.Log("City Deleted");
        }
        GUILayout.EndHorizontal();
    }

    private void GenerateSettingTab()
    {
        resurceToolbarInt = GUILayout.Toolbar(resurceToolbarInt, resurceToolbarStrings);
        switch (resurceToolbarInt)
        {
            case 0:
                GenerateStreetTab();
                break;
            case 1:
                GenerateBuildingTab();
                break;
            case 2:
                GenerateLightTab();
                break;
            case 3:
                GeneratePropsTab();
                break;
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh"))
        {
            BaseCity.RefreshAssets();
        }
    }
}
