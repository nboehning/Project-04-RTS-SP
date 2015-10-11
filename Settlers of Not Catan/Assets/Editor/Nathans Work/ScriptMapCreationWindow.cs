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
    private int hexEdgeLength = 40;
    private Rect labelPosition;
    private float xOffset = 45f;
    private float yOffset = 200f;
    private string labelText;
    private Vector2 hexCenter;
    private const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f;   //Mathf.Sqrt(0.75f) & -Sin(Mathf.PI * 4 / 3) (apparently) Marshallllllll

    // Hex data variables
    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
    int[] intPopup = { 1, 2, 3, 4, 5, 6 };
    
    // Stuff for heuristics
    private int numWool;
    private int numWood;
    private int numBrick;
    private int numWheat;
    private int numOnes;
    private int numTwos;
    private int numThrees;
    private int numFours;
    private int numFives;
    private int numSixes;

    [MenuItem("Tools/Create New Map")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
        window.position = new Rect(100, 50, 1250, 1000);
        window.maxSize = new Vector2(1250, 1000);
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

        #region Map Data

        Rect hexDataRect = new Rect(100f, 100f, 175f, 15f);
        EditorGUI.LabelField(hexDataRect, "Selected Hex Data", EditorStyles.boldLabel);

        Rect hexCoordinateLabelRect = new Rect(100f, 117f, 40f, 15f);
        EditorGUI.LabelField(hexCoordinateLabelRect, string.Format("({0},{1})", selectedRow, selectedColumn));

        Rect rollValueLabelRect = new Rect(100f, 134f, 70f, 15f);
        EditorGUI.LabelField(rollValueLabelRect, "Roll Value: ");

        Rect rollDropDownRect = new Rect(174f, 134f, 25f, 17f);
        hexMap[selectedRow][selectedColumn].hexNum = EditorGUI.IntPopup(rollDropDownRect, hexMap[selectedRow][selectedColumn].hexNum, intPopupString, intPopup);
        
        Rect hexTypeLabelRect = new Rect(100f, 151f, 60f, 15f);
        EditorGUI.LabelField(hexTypeLabelRect, "Hex Type: ");

        Rect hexTypeRect = new Rect(174f, 151f, 75f, 17f);
        hexMap[selectedRow][selectedColumn].hexType = (HexType)EditorGUI.EnumPopup(hexTypeRect, hexMap[selectedRow][selectedColumn].hexType);

        #endregion


        #region heuristic display
        float heuristicOffset = 750f;
         
        // Displays the heuristics to the designer
        Rect heuristicLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(heuristicLabelRect, "Amount Placed", EditorStyles.boldLabel);
        heuristicOffset += 17f;

        Rect numBrickLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numBrickLabelRect, "Brick: " + numBrick);
        heuristicOffset += 17f;

        Rect numWheatLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numWheatLabelRect, "Grain: " + numWheat);
        heuristicOffset += 17f;

        Rect numWoodLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numWoodLabelRect, "Wood: " + numWood);
        heuristicOffset += 17f;

        Rect numWoolLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numWoolLabelRect, "Wool: " + numWool);
        heuristicOffset += 17f;

        Rect numOneLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numOneLabelRect, "1's: " + numOnes);
        heuristicOffset += 17f;

        Rect numTwoLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numTwoLabelRect, "2's: " + numTwos);
        heuristicOffset += 17f;

        Rect numThreeLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numThreeLabelRect, "3's: " + numThrees);
        heuristicOffset += 17f;

        Rect numFourLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numFourLabelRect, "4's: " + numFours);
        heuristicOffset += 17f;

        Rect numFiveLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numFiveLabelRect, "5's: " + numFives);
        heuristicOffset += 17f;

        Rect numSixLabelRect = new Rect(100f, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(numSixLabelRect, "6's: " + numSixes);

#endregion


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
                        labelText = string.Format("({0},{1})\n", i, j) + hexMap[i][j].hexType.ToString().ToLower() + "\n" + hexMap[i][j].hexNum;
                        labelPosition = new Rect(xOffset, yOffset, 40f, 40f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 13f;

                        if (hexMap[i][j].isActive)
                        {
                            #region data change check
                            // Check to modify the number of each dice roll if it's been changed
                            if (hexMap[i][j].oldHexNum != hexMap[i][j].hexNum)
                            {
                                switch (hexMap[i][j].hexNum)
                                {
                                    case 1:
                                        numOnes++;
                                       break;
                                    case 2:
                                        numTwos++;
                                        break;
                                    case 3:
                                        numThrees++;
                                        break;
                                    case 4:
                                        numFours++;
                                        break;
                                    case 5:
                                        numFives++;
                                        break;
                                    case 6:
                                        numSixes++;
                                        break;
                                }
                                switch (hexMap[i][j].oldHexNum)
                                {
                                    case 1:

                                        numOnes--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 2:
                                        numTwos--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 3:
                                        numThrees--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 4:
                                        numFours--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 5:
                                        numFives--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 6:
                                        numSixes--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    default:
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                }
                            }

                            // Check to modify the number of the hex types if it's been changed
                            if (hexMap[i][j].hexType != hexMap[i][j].prevHexType)
                            {
                                switch (hexMap[i][j].hexType)
                                {
                                    case HexType.BRICK:
                                        numBrick++;
                                        break;
                                    case HexType.GRAIN:
                                        numWheat++;
                                        break;
                                    case HexType.WOOD:
                                        numWood++;
                                        break;
                                    case HexType.WOOL:
                                        numWool++;
                                        break;
                                }
                                switch (hexMap[i][j].prevHexType)
                                {
                                    case HexType.BRICK:
                                        numBrick--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.GRAIN:
                                        numWheat--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.WOOD:
                                        numWood--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.WOOL:
                                        numWool--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    default:
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;

                                }
                            }
                            #endregion
                            
                            if (hexMap[i][j].hexType != HexType.NONE)
                            {
                                TurnOnActive(i, j);
                            }
                            if (GUI.Button(labelPosition, labelText, style))
                            {

                                selectedRow = i;
                                selectedColumn = j;

                            }
                        }
                        else
                        {
                            EditorGUI.LabelField(labelPosition, string.Format("({0},{1})\n", i, j));
                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines

                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, false);
                        }


                        // Bottom of hex
                        hex.hexCorners[0].x += 17f;
                        hex.hexCorners[1].x += 17f;
                        hex.hexCorners[2].x += 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, false);
                        }

                        // Top of hex
                        hex.hexCorners[3].x -= 17f;
                        hex.hexCorners[4].x -= 17f;
                        hex.hexCorners[5].x -= 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, false);
                        }

                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (i + 8);
                    xOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * MAGIC_EDGE_DISTANCE_CONVERT - 1;
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
                        labelText = string.Format("({0},{1})\n", i, j) + hexMap[i][j].hexType.ToString().ToLower() + "\n" + hexMap[i][j].hexNum;
                        labelPosition = new Rect(xOffset, yOffset, 40f, 40f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 13f;

                        #region data change check
                        // Check to modify the number of each dice roll if it's been changed
                        if (hexMap[i][j].oldHexNum != hexMap[i][j].hexNum)
                        {
                            switch (hexMap[i][j].hexNum)
                            {
                                case 1:
                                    numOnes++;
                                    break;
                                case 2:
                                    numTwos++;
                                    break;
                                case 3:
                                    numThrees++;
                                    break;
                                case 4:
                                    numFours++;
                                    break;
                                case 5:
                                    numFives++;
                                    break;
                                case 6:
                                    numSixes++;
                                    break;
                            }
                            switch (hexMap[i][j].oldHexNum)
                            {
                                case 1:

                                    numOnes--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 2:
                                    numTwos--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 3:
                                    numThrees--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 4:
                                    numFours--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 5:
                                    numFives--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 6:
                                    numSixes--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                default:
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                            }
                        }

                        // Check to modify the number of the hex types if it's been changed
                        if (hexMap[i][j].hexType != hexMap[i][j].prevHexType)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    numBrick++;
                                    break;
                                case HexType.GRAIN:
                                    numWheat++;
                                    break;
                                case HexType.WOOD:
                                    numWood++;
                                    break;
                                case HexType.WOOL:
                                    numWool++;
                                    break;
                            }
                            switch (hexMap[i][j].prevHexType)
                            {
                                case HexType.BRICK:
                                    numBrick--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.GRAIN:
                                    numWheat--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.WOOD:
                                    numWood--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.WOOL:
                                    numWool--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                default:
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;

                            }
                        }

                        #endregion

                        if (hexMap[i][j].isActive)
                        {
                            if (hexMap[i][j].hexType != HexType.NONE)
                            {
                                TurnOnActive(i, j);
                            }
                            if (GUI.Button(labelPosition, labelText, style))
                            {

                                selectedRow = i;
                                selectedColumn = j;
                            }
                        }
                        else
                        {
                            EditorGUI.LabelField(labelPosition, string.Format("({0},{1})\n", i, j));
                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines

                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, false);
                        }


                        // Bottom of hex
                        hex.hexCorners[0].x += 17f;
                        hex.hexCorners[1].x += 17f;
                        hex.hexCorners[2].x += 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, false);
                        }

                        // Top of hex
                        hex.hexCorners[3].x -= 17f;
                        hex.hexCorners[4].x -= 17f;
                        hex.hexCorners[5].x -= 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, false);
                        }

                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (22 - i);
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Sin(Mathf.PI * (4f / 3f))) + 1;
                    break;
            }
        }

        #endregion

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        Rect ExportDataButtonRect = new Rect((position.width / 2f) - 37f, position.height - 50f, 100f, 25f);
        Color oldColor = GUI.color;

        GUI.color = Color.green;
        if (GUI.Button(ExportDataButtonRect, "Export Data"))
        {
            Debug.Log("Exports the data!");
        }

        GUI.color = oldColor;
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
        TurnOnActive(7, 7);

    }

    void TurnOnActive(int startRow, int startColumn)
    {
        // Series of checks to make sure you don't active a hex that isn't there

        // Check directly right
        if (startColumn + 1 < hexMap[startRow].Length)
        {
            hexMap[startRow][startColumn + 1].isActive = true;
        }

        // Check directly left
        if (startColumn - 1 >= 0)
        {
            hexMap[startRow][startColumn - 1].isActive = true;
        }

        // First row of map
        if (startRow == 0)
        {
            // Check down right
            if (startColumn + 1 < hexMap[startRow + 1].Length && startRow + 1 < hexMap.Length)
            {
                hexMap[startRow + 1][startColumn + 1].isActive = true;
            }

            // Check down left
            if (startColumn >= 0 && startRow + 1 < hexMap.Length)
            {
                hexMap[startRow + 1][startColumn].isActive = true;
            }
        }

        // First column of top half
        else if (startColumn == 0 && startRow < 7)
        {
            // Down left
            hexMap[startRow + 1][startColumn].isActive = true;

            // Down right
            hexMap[startRow + 1][startColumn + 1].isActive = true;
        }

        // Top half of map
        else if (startRow < 7)
        {
            // Down right
            hexMap[startRow + 1][startColumn + 1].isActive = true;

            // Down left
            hexMap[startRow + 1][startColumn].isActive = true;

            // Check up left
            if (startColumn >= 0 && startRow - 1 >= 0)
            {
                hexMap[startRow - 1][startColumn].isActive = true;
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length && startRow > 0)
            {
                hexMap[startRow - 1][startColumn + 1].isActive = true;
            }
        }

        // Special case for right most middle row
        else if (startColumn == hexMap[startRow].Length - 1 && startRow == 7)
        {

            // Down left
            hexMap[startRow + 1][startColumn - 1].isActive = true;

            // Up left
            hexMap[startRow - 1][startColumn - 1].isActive = true;

        }

        // Special case for left most middle row
        else if (startColumn == 0 && startRow == 7)
        {

            // Up right
            hexMap[startRow - 1][startColumn].isActive = true;

            // Down right
            hexMap[startRow + 1][startColumn].isActive = true;

        }

        // Middle row of map
        else if (startRow == 7)
        {
            // Down right
            hexMap[startRow + 1][startColumn].isActive = true;

            // Down left
            hexMap[startRow + 1][startColumn - 1].isActive = true;

            // Up left
            hexMap[startRow - 1][startColumn - 1].isActive = true;
            
            // Up right
            hexMap[startRow - 1][startColumn].isActive = true;
        }

        // Bottom row of map
        else if (startRow == hexMap.Length - 1)
        {
            // Check up left
            if (startColumn >= 0)
            {
                hexMap[startRow - 1][startColumn].isActive = true;
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length)
            {
                hexMap[startRow - 1][startColumn + 1].isActive = true;
            }
        }

        // First column of bottom half
        else if (startColumn == 0 && startRow > 7)
        {
            // Up left
            hexMap[startRow - 1][startColumn].isActive = true;

            // Up right
            hexMap[startRow - 1][startColumn + 1].isActive = true;

            // Down right
            hexMap[startRow + 1][startColumn].isActive = true;
        }

        // Bottom half of map
        else if (startColumn < hexMap[startRow].Length - 1 && startRow > 7)
        {
            // Check down right
            if (startColumn + 1 < hexMap[startRow + 1].Length && startRow + 1 < hexMap.Length)
            {
                hexMap[startRow + 1][startColumn + 1].isActive = true;
            }

            // Check down left
            if (startColumn - 1 >= 0 && startRow + 1 < hexMap.Length)
            {
                hexMap[startRow + 1][startColumn - 1].isActive = true;
            }

            // Check up left
            if (startColumn >= 0 && startRow - 1 >= 0)
            {
                hexMap[startRow - 1][startColumn].isActive = true;
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length && startRow > 0)
            {
                hexMap[startRow - 1][startColumn + 1].isActive = true;
            }
        }
    }
}