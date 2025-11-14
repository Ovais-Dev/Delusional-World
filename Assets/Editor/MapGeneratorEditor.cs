using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator gen = (MapGenerator)target;

        if (GUILayout.Button("Generate Path"))
        {
            gen.GeneratePath();
        }
        if(GUILayout.Button("Clear Path"))
        {
            gen.ClearPath();
        }
    }
}
