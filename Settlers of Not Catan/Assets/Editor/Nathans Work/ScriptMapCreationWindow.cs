using UnityEngine;
using UnityEditor;

public class ScriptMapCreationWindow : EditorWindow
{

    private Vector2 mapDataScrollPos = Vector2.zero;
    private int numHexes = 100;
    private Color oldColor;

    [MenuItem("Tools/Create New Map")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
        window.position = new Rect(100, 50, 1250, 750);
        window.maxSize = new Vector2(1250, 750);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        #region Map
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.75f));

        EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();

        #endregion
        #region Map Data

        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.24f));

        EditorGUILayout.LabelField("Hex Data", EditorStyles.boldLabel);

        //mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, GUILayout.Width(100), GUILayout.Height(1000));
        mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, false, true, GUILayout.Width(position.width * 0.23f), GUILayout.Height(position.height - 54));

        for (int i = 0; i < numHexes; i++)
        {
            EditorGUILayout.LabelField(
                string.Format(
                    "Hex {0}: aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    i + 1));
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();
    }
}
