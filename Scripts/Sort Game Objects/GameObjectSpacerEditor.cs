using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObjectSpacer))]
public class GameObjectSpacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameObjectSpacer spacer = (GameObjectSpacer)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Space Objects"))
        {
            spacer.SpaceGameObjects();
        }
    }
}
