using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquereCityGenerato))]
public class CityBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SquereCityGenerato scgenerator = (SquereCityGenerato)target;
        if (GUILayout.Button("Refresh Resource"))
        {
            //scgenerator.RefreshAssets();
        }
        if (GUILayout.Button("Generate City"))
        {
            scgenerator.GenerateCity();
        }
    }
}
