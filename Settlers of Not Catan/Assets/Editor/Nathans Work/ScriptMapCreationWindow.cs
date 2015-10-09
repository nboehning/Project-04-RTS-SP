using UnityEngine;
using UnityEditor;

public class ScriptMapCreationWindow : EditorWindow
{

    private Vector2 mapDataScrollPos = Vector2.zero;
    private int numRows = 15;
    private int hexEdgeLength = 30;
    private Rect labelPosition;
    private float xOffset = 45f;
    private float yOffset = 200f;
    private string labelText;
    private Vector2 hexCenter;
    private Color oldColor;
    private ScriptHex selectedHex = new ScriptHex(Vector2.zero, 2);
    private int curHex = 0;
    private ScriptHex[] hexMap = new ScriptHex[169];
    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
    int[] intPopup = { 1, 2, 3, 4, 5, 6 };
    private int numWool;
    private int numWood;
    private int numBrick;
    private int numWheat;
    private const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f;   //Mathf.Sqrt(0.75f) & -Sin(Mathf.PI * 4 / 3) (apparently) Marshallllllll

    [MenuItem("Tools/Create New Map")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
        window.position = new Rect(100, 50, 1000, 750);
        window.maxSize = new Vector2(1000, 750);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        xOffset = 400f;
        yOffset = 45f;
        EditorGUILayout.BeginHorizontal();

        // @author: MARSHALL AND HIS MATH GODLINESS, plus nathan
        #region Map
        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.99f), GUILayout.Height(position.height - 40f));

        EditorGUILayout.LabelField("                                        " +
                                   "                                                                                          " +
                                   "      Map", EditorStyles.boldLabel);
        #region vertical spacing
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion
        EditorGUILayout.BeginVertical();
        #region Map Data

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Hex Data", EditorStyles.boldLabel, GUILayout.Width(65f));


        EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
        selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString, intPopup, GUILayout.Width(25f));

        EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
        selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.EndVertical();

        // yay marshall
        for (int i = 0; i < numRows; i++)
        {
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    for (int j = 0; j < 8 + i; j++)
                    {
                        labelText = string.Format("{0},{1}", i, j);
                        labelPosition = new Rect(xOffset, yOffset, 40f, 15f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 8f;
                        EditorGUI.LabelField(labelPosition, labelText);
                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        for (int k = 0; k < 5; k++)
                        {
                            // Draw line from hex.hexCorners[k] to hex.hexCorners[k + 1]
                        }

                        // Draw last line from hex.hexCorner[5] to hex.hexCorners[0]

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (i + 8);
                    xOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * MAGIC_EDGE_DISTANCE_CONVERT;
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    for (int j = 0; j < 22 - i; j++)
                    {
                        labelText = string.Format("{0},{1}", i, j);
                        labelPosition = new Rect(xOffset, yOffset, 40f, 15f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 8f;
                        EditorGUI.LabelField(labelPosition, labelText);
                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        for (int k = 0; k < 5; k++)
                        {
                            // Draw line from hex.hexCorners[k] to hex.hexCorners[k + 1]
                        }

                        // Draw last line from hex.hexCorner[5] to hex.hexCorners[0]

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (22 - i);
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Sin(Mathf.PI * (4f / 3f)));
                    break;
            }
        }

        #endregion

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

    }
}