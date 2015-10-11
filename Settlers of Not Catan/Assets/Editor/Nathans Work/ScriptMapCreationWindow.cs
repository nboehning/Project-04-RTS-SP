using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptMapCreationWindow : EditorWindow
{

    // Hex map variables
    private ScriptHex[][] hexMap = new ScriptHex[15][];
    private int selectedRow = 7;
    private int selectedColumn = 7;


    // Drawing of map variables
    private int numRows = 15;
    private int hexEdgeLength = 30;
    private Rect labelPosition;
    private float xOffset = 45f;
    private float yOffset = 200f;
    private string labelText;
    private Vector2 hexCenter;
    private const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f;   //Mathf.Sqrt(0.75f) & -Sin(Mathf.PI * 4 / 3) (apparently) Marshallllllll

    // Hex data variables
    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
    int[] intPopup = { 1, 2, 3, 4, 5, 6 };
    private string hexName = string.Format("Hex ({0}, {1})", 7, 7);
    
    // Stuff for heuristics
    private int numWool;
    private int numWood;
    private int numBrick;
    private int numWheat;

    [MenuItem("Tools/Create New Map")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
        window.position = new Rect(100, 50, 1000, 750);
        window.maxSize = new Vector2(1000, 750);
        window.minSize = window.maxSize;
        window.InitializeVariables();
        window.Show();
    }

    void OnGUI()
    {
        xOffset = 375f;
        yOffset = 50f;
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width - 5), GUILayout.Height(position.height - 40f));

        EditorGUILayout.BeginVertical();
        #region Map Data

        Rect hexDataRect = new Rect(100f, 100f, 65f, 15f);
        EditorGUI.LabelField(hexDataRect, "Hex Data", EditorStyles.boldLabel);

        Rect hexNameRect = new Rect(100f, 117f, 65f, 15f);
        EditorGUI.LabelField(hexNameRect, hexName);

        Rect rollValueLabelRect = new Rect(100f, 134f, 70f, 15f);
        EditorGUI.LabelField(rollValueLabelRect, "Roll Value: ");

        Rect rollDropDownRect = new Rect(174f, 134f, 25f, 17f);
        hexMap[selectedRow][selectedColumn].hexNum = EditorGUI.IntPopup(rollDropDownRect, hexMap[selectedRow][selectedColumn].hexNum, intPopupString, intPopup);

        Rect hexTypeLabelRect = new Rect(100f, 151f, 60f, 15f);
        EditorGUI.LabelField(hexTypeLabelRect, "Hex Type: ");

        Rect hexTypeRect = new Rect(174f, 134f, 25f, 17f);
        hexMap[selectedRow][selectedColumn].hexType = (HexType)EditorGUI.EnumPopup(hexTypeRect, hexMap[selectedRow][selectedColumn].hexType);

        #endregion

        EditorGUILayout.EndVertical();


        GUIStyle style = EditorStyles.label;

        style.alignment = TextAnchor.MiddleCenter;


        // @author: MARSHALL AND HIS MATH GODLINESS, plus nathan
        #region Map
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
                        
                        if (GUI.Button(labelPosition, labelText, style))
                        {

                            selectedRow = i;
                            selectedColumn = j;
                            hexName = string.Format("Hex ({0}, {1})", i, j);

                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines
                        Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, true);

                        // Bottom of hex
                        hex.hexCorners[0].x += 13f;
                        hex.hexCorners[1].x += 13f;
                        hex.hexCorners[2].x += 13f;
                        Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, true);

                        // Top of hex
                        hex.hexCorners[3].x -= 13f;
                        hex.hexCorners[4].x -= 13f;
                        hex.hexCorners[5].x -= 13f;
                        Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, true);
                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (i + 8);
                    xOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * MAGIC_EDGE_DISTANCE_CONVERT - 2;
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

                        if (GUI.Button(labelPosition, labelText, style))
                        {

                            selectedRow = i;
                            selectedColumn = j;
                            hexName = string.Format("Hex ({0}, {1})", i, j);

                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines
                        Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, true);

                        // Bottom of hex
                        hex.hexCorners[0].x += 13f;
                        hex.hexCorners[1].x += 13f;
                        hex.hexCorners[2].x += 13f;
                        Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, true);

                        // Top of hex
                        hex.hexCorners[3].x -= 13f;
                        hex.hexCorners[4].x -= 13f;
                        hex.hexCorners[5].x -= 13f;
                        Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, true);
                        Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, true);

                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (22 - i);
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Sin(Mathf.PI * (4f / 3f))) + 2;
                    break;
            }
        }

        #endregion

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

    }

    void InitializeVariables()
    {
        hexMap[0] = new ScriptHex[8];
        hexMap[1] = new ScriptHex[9];
        hexMap[2] = new ScriptHex[10];
        hexMap[3] = new ScriptHex[11];
        hexMap[4] = new ScriptHex[12];
        hexMap[5] = new ScriptHex[13];
        hexMap[6] = new ScriptHex[14];
        hexMap[7] = new ScriptHex[15];
        hexMap[8] = new ScriptHex[14];
        hexMap[9] = new ScriptHex[13];
        hexMap[10] = new ScriptHex[12];
        hexMap[11] = new ScriptHex[11];
        hexMap[12] = new ScriptHex[10];
        hexMap[13] = new ScriptHex[9];
        hexMap[14] = new ScriptHex[8];
       
        for (int i = 0; i < hexMap.Length; i++)
        {
            
            for (int j = 0; j < hexMap[i].Length; j++)
            {
                hexMap[i][j] = new ScriptHex();
            }
        }

        hexMap[7][7].isActive = true;
        hexMap[7][7].hexType = HexType.WOOD;

    }
}