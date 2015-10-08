using System.Threading;
using UnityEngine;
using UnityEditor;

public class ScriptMapCreationWindow : EditorWindow
{

    private Vector2 mapDataScrollPos = Vector2.zero;
    private int numRows = 9;
    private Color oldColor;
    private ScriptHex selectedHex = new ScriptHex(Vector2.zero, 2);
    private int curHex = 0;
    private ScriptHex[] hexMap = new ScriptHex[35];
    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
    int[] intPopup = { 1, 2, 3, 4, 5, 6 };
    private int numWool;
    private int numWood;
    private int numBrick;
    private int numWheat;

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
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.70f));

        EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();

        #endregion
        #region Map Data

        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.29f));

        EditorGUILayout.LabelField("Hex Data", EditorStyles.boldLabel);

        //mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, GUILayout.Width(100), GUILayout.Height(1000));
        mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, false, true, GUILayout.Width(position.width * 0.28f), GUILayout.Height(position.height - 54));

        for (int i = 0; i < numRows; i++)
        {
            switch (i)
            {
                case 0:
                    for (int j = 0; j < 4; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex ({0},{1}):", i, j), GUILayout.Width(57f));
                        switch (j)
                        {
                            case 0:
                                //selectedHex = hexMap[curHex];
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                        }
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();
    }
}