using UnityEngine;
using System.Collections;

[System.Flags]
public enum HexType
{
    WOOD = 1,
    GRAIN = 2,
    BRICK = 4,
    WOOL = 8,
    NONE = 16
}
public class ScriptHex
{
    private HexType hexType;
    private Vector2 hexCenter;
    private Vector2[] hexCorners = new Vector2[6];

    public ScriptHex(Vector2 center, float size)
    {
        hexType = HexType.NONE;
        hexCenter = center;
        for (int i = 0; i < 6; i++)
        {
            hexCorners[i] = GenerateHexPoint(hexCenter, size, i);
        }
    }

    public Vector2 GenerateHexPoint(Vector2 center, float size, int numCorner)
    {
        Vector2 returnPoint;

        float angleDegree = 60 * numCorner + 30;
        var angleRadian = Mathf.PI / 180 * angleDegree;
        returnPoint.x = center.x + size * Mathf.Cos(angleRadian);
        returnPoint.y = center.y + size * Mathf.Sin(angleRadian);

        return returnPoint;
        
    }

}
