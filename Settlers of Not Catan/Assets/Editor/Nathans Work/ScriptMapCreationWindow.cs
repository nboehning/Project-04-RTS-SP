using System.Threading;
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
        window.position = new Rect(100, 50, 1250, 750);
        window.maxSize = new Vector2(1250, 750);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        xOffset = 250f;
        yOffset = 45f;
        //Debug.Log("Comment out this line");
        EditorGUILayout.BeginHorizontal();

        // @author: MARSHALL AND HIS MATH GODLINESS, plus nathan
        #region Map
        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.70f), GUILayout.Height(position.height - 54));

        EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);

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


        EditorGUILayout.EndVertical();

        #endregion

        #region Map Data

        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.29f));

        EditorGUILayout.LabelField("Hex Data", EditorStyles.boldLabel);

        //mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, GUILayout.Width(100), GUILayout.Height(1000));
        mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, false, true, GUILayout.Width(position.width * 0.29f), GUILayout.Height(position.height - 54));

        for (int i = 0; i < numRows; i++)
        {
            switch (i)
            {
                // 8 across
                #region row1
                case 0:
                    EditorGUILayout.LabelField("Row 0", EditorStyles.boldLabel);
                    for (int j = 0; j < 8; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion          // 8 across

                // 9 across
                #region row2
                case 1:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 1", EditorStyles.boldLabel);
                    for (int j = 0; j < 9; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 10 across
                #region row3
                case 2:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 2", EditorStyles.boldLabel);
                    for (int j = 0; j < 10; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 11 across
                #region row4
                case 3:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 3", EditorStyles.boldLabel);
                    for (int j = 0; j < 11; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 12 across
                #region row5
                case 4:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 4", EditorStyles.boldLabel);
                    for (int j = 0; j < 12; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 13 across
                #region row6
                case 5:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 5", EditorStyles.boldLabel);
                    for (int j = 0; j < 13; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 12:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 14 across
                #region row7
                case 6:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 6", EditorStyles.boldLabel);
                    for (int j = 0; j < 14; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 12:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 13:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 15 across
                #region row8
                case 7:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 7", EditorStyles.boldLabel);
                    for (int j = 0; j < 15; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 12:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 13:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 14:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 14 across
                #region row9
                case 8:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 8", EditorStyles.boldLabel);
                    for (int j = 0; j < 14; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 12:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 13:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 13 across
                #region row10
                case 9:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 9", EditorStyles.boldLabel);
                    for (int j = 0; j < 13; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 12:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 12 across
                #region row11
                case 10:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 10", EditorStyles.boldLabel);
                    for (int j = 0; j < 12; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 11:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 11 across
                #region row12
                case 11:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 11", EditorStyles.boldLabel);
                    for (int j = 0; j < 11; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 10:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 10 across
                #region row13
                case 12:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 12", EditorStyles.boldLabel);
                    for (int j = 0; j < 10; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 9:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 9 across
                #region row14
                case 13:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 13", EditorStyles.boldLabel);
                    for (int j = 0; j < 9; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 8:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion

                // 8 across
                #region row15
                case 14:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Row 14", EditorStyles.boldLabel);
                    for (int j = 0; j < 8; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
                        switch (j)
                        {
                            case 0:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 1:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 2:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 3:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 4:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 5:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 6:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                            case 7:
                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
                                    intPopup, GUILayout.Width(25f));
                                curHex++;
                                EditorGUILayout.EndHorizontal();
                                break;
                        }
                    }
                    break;
                #endregion          // 8 across
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();
    }
}


//using System.Threading;
//using UnityEngine;
//using UnityEditor;

//public class ScriptMapCreationWindow : EditorWindow
//{

//    private Vector2 mapDataScrollPos = Vector2.zero;
//<<<<<<< HEAD
//    private int numRows = 15;
//    private int hexEdgeLength = 30;
//    private Rect labelPosition;
//    private float xOffset = 45f;
//    private float yOffset = 200f;
//    private string labelText;
//    private Vector2 hexCenter;
//    private Color oldColor;
//    private ScriptHex selectedHex = new ScriptHex(Vector2.zero, 2);
//    private int curHex = 0;
//    private ScriptHex[] hexMap = new ScriptHex[169];
//    string[] intPopupString = {"1", "2", "3", "4", "5", "6"};
//    int[] intPopup = {1, 2, 3, 4, 5, 6};
//=======
//    private int numRows = 9;
//    private Color oldColor;
//    private ScriptHex selectedHex = new ScriptHex(Vector2.zero, 2);
//    private int curHex = 0;
//    private ScriptHex[] hexMap = new ScriptHex[35];
//    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
//    int[] intPopup = { 1, 2, 3, 4, 5, 6 };
//>>>>>>> mdobson2/master
//    private int numWool;
//    private int numWood;
//    private int numBrick;
//    private int numWheat;
//<<<<<<< HEAD
//    private const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f;   //Mathf.Sqrt(0.75f) & -Sin(Mathf.PI * 4 / 3) (apparently) Marshallllllll
//=======
//>>>>>>> mdobson2/master

//    [MenuItem("Tools/Create New Map")]
//    private static void Init()
//    {
//        // Get existing open window or if none, make a new one:
//        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
//        window.position = new Rect(100, 50, 1250, 750);
//        window.maxSize = new Vector2(1250, 750);
//        window.minSize = window.maxSize;
//        window.Show();
//    }

//    void OnGUI()
//    {
//        xOffset = 250f;
//        yOffset = 45f;
//        //Debug.Log("Comment out this line");
//        EditorGUILayout.BeginHorizontal();

//        // @author: MARSHALL AND HIS MATH GODLINESS, plus nathan
//        #region Map
//<<<<<<< HEAD
//        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.70f), GUILayout.Height(position.height - 54));
//=======
//        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.70f));
//>>>>>>> mdobson2/master

//        EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);

//        // yay marshall
//        for (int i = 0; i < numRows; i++)
//        {
//            switch (i)
//            {
//                case 0:
//                case 1:
//                case 2:
//                case 3:
//                case 4:
//                case 5:
//                case 6:
//                    for (int j = 0; j < 8 + i; j++)
//                    {
//                        labelText = string.Format("{0},{1}", i, j);
//                        labelPosition = new Rect(xOffset, yOffset, 40f, 15f);
//                        hexCenter.x = xOffset + 20f;
//                        hexCenter.y = yOffset + 8f;
//                        EditorGUI.LabelField(labelPosition, labelText);
//                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

//                        for (int k = 0; k < 5; k++)
//                        {
//                            // Draw line from hex.hexCorners[k] to hex.hexCorners[k + 1]
//                        }

//                        // Draw last line from hex.hexCorner[5] to hex.hexCorners[0]

//                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
//                    }
//                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (i + 8);
//                    xOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
//                    yOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * MAGIC_EDGE_DISTANCE_CONVERT;
//                    break;
//                case 7:
//                case 8:
//                case 9:
//                case 10:
//                case 11:
//                case 12:
//                case 13:
//                case 14:
//                    for (int j = 0; j < 22 - i; j++)
//                    {
//                        labelText = string.Format("{0},{1}", i, j);
//                        labelPosition = new Rect(xOffset, yOffset, 40f, 15f);
//                        hexCenter.x = xOffset + 20f;
//                        hexCenter.y = yOffset + 8f;
//                        EditorGUI.LabelField(labelPosition, labelText);
//                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

//                        for (int k = 0; k < 5; k++)
//                        {
//                            // Draw line from hex.hexCorners[k] to hex.hexCorners[k + 1]
//                        }

//                        // Draw last line from hex.hexCorner[5] to hex.hexCorners[0]

//                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
//                    }
//                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (22 - i);
//                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
//                    yOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Sin(Mathf.PI * (4f / 3f)));
//                    break;
//            }
//        }


//        EditorGUILayout.EndVertical();

//        #endregion

//        #region Map Data

//        EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.29f));

//        EditorGUILayout.LabelField("Hex Data", EditorStyles.boldLabel);

//        //mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, GUILayout.Width(100), GUILayout.Height(1000));
//<<<<<<< HEAD
//        mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, false, true, GUILayout.Width(position.width * 0.29f), GUILayout.Height(position.height - 54));
//=======
//        mapDataScrollPos = EditorGUILayout.BeginScrollView(mapDataScrollPos, false, true, GUILayout.Width(position.width * 0.28f), GUILayout.Height(position.height - 54));
//>>>>>>> mdobson2/master

//        for (int i = 0; i < numRows; i++)
//        {
//            switch (i)
//            {
//<<<<<<< HEAD
//                // 8 across
//                #region row1
//                case 0:
//                    EditorGUILayout.LabelField("Row 0", EditorStyles.boldLabel);
//                    for (int j = 0; j < 8; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//=======
//                case 0:
//                    for (int j = 0; j < 4; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex ({0},{1}):", i, j), GUILayout.Width(57f));
//                        switch (j)
//                        {
//                            case 0:
//                                //selectedHex = hexMap[curHex];
//>>>>>>> mdobson2/master
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//<<<<<<< HEAD
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//=======
//                                break;
//                            case 2:
//                                break;
//                            case 3:
//>>>>>>> mdobson2/master
//                                break;
//                        }
//                    }
//                    break;
//<<<<<<< HEAD
//                #endregion          // 8 across

//                // 9 across
//                #region row2
//                case 1:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 1", EditorStyles.boldLabel);
//                    for (int j = 0; j < 9; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 10 across
//                #region row3
//                case 2:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 2", EditorStyles.boldLabel);
//                    for (int j = 0; j < 10; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 11 across
//                #region row4
//                case 3:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 3", EditorStyles.boldLabel);
//                    for (int j = 0; j < 11; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 12 across
//                #region row5
//                case 4:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 4", EditorStyles.boldLabel);
//                    for (int j = 0; j < 12; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 13 across
//                #region row6
//                case 5:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 5", EditorStyles.boldLabel);
//                    for (int j = 0; j < 13; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 12:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 14 across
//                #region row7
//                case 6:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 6", EditorStyles.boldLabel);
//                    for (int j = 0; j < 14; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 12:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 13:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 15 across
//                #region row8
//                case 7:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 7", EditorStyles.boldLabel);
//                    for (int j = 0; j < 15; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 12:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 13:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 14:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 14 across
//                #region row9
//                case 8:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 8", EditorStyles.boldLabel);
//                    for (int j = 0; j < 14; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 12:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 13:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 13 across
//                #region row10
//                case 9:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 9", EditorStyles.boldLabel);
//                    for (int j = 0; j < 13; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 12:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 12 across
//                #region row11
//                case 10:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 10", EditorStyles.boldLabel);
//                    for (int j = 0; j < 12; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 11:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 11 across
//                #region row12
//                case 11:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 11", EditorStyles.boldLabel);
//                    for (int j = 0; j < 11; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 10:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 10 across
//                #region row13
//                case 12:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 12", EditorStyles.boldLabel);
//                    for (int j = 0; j < 10; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 9:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 9 across
//                #region row14
//                case 13:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 13", EditorStyles.boldLabel);
//                    for (int j = 0; j < 9; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 8:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                #endregion

//                // 8 across
//                #region row15
//                case 14:
//                    EditorGUILayout.Space();
//                    EditorGUILayout.LabelField("Row 14", EditorStyles.boldLabel);
//                    for (int j = 0; j < 8; j++)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField(string.Format("Hex({0},{1}):", i, j), GUILayout.Width(68f));
//                        switch (j)
//                        {
//                            case 0:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 1:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 2:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 3:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 4:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 5:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 6:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                            case 7:
//                                EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(60f));
//                                selectedHex.hexType = (HexType)EditorGUILayout.EnumPopup(selectedHex.hexType, GUILayout.Width(100f));
//                                EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(60f));
//                                selectedHex.hexNum = EditorGUILayout.IntPopup("", selectedHex.hexNum, intPopupString,
//                                    intPopup, GUILayout.Width(25f));
//                                curHex++;
//                                EditorGUILayout.EndHorizontal();
//                                break;
//                        }
//                    }
//                    break;
//                    #endregion          // 8 across
//=======
//                case 1:
//                    break;
//                case 2:
//                    break;
//                case 3:
//                    break;
//                case 4:
//                    break;
//                case 5:
//                    break;
//                case 6:
//                    break;
//                case 7:
//                    break;
//                case 8:
//                    break;
//                case 9:
//                    break;
//>>>>>>> mdobson2/master
//            }
//        }
//        EditorGUILayout.EndScrollView();

//        EditorGUILayout.EndVertical();
//        #endregion

//        EditorGUILayout.EndHorizontal();
//    }
//}
