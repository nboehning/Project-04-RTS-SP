using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptBoardEdge : MonoBehaviour {

    public ScriptEngine engine;
    public ScriptPlayer owner = null;
    public List<ScriptBoardEdge> adjacentRoads = new List<ScriptBoardEdge>(0);
    public List<ScriptBoardCorner> adjacentSettlements = new List<ScriptBoardCorner>(0);

    public bool CheckValidBuild()
    {
        foreach (ScriptBoardEdge road in adjacentRoads)
        {
            if (road.owner == engine.players[0])
            {
                owner = engine.players[0];
                return true;
            }
        }
        return false;
    }

    public bool CheckStartRoad()
    {
        foreach (ScriptBoardCorner settlement in adjacentSettlements)
        {
            if (settlement.owner == engine.players[0])
            {
                owner = engine.players[0];
                return true;
            }
        }
        return false;
    }
}
