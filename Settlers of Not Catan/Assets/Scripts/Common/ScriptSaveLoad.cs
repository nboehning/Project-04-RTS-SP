using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @author Marshall Mason
/// </summary>
public class ScriptSaveLoad : MonoBehaviour
{
    public List<ScriptBoardCorner> corners;
    public List<ScriptBoardEdge> edges;
    public List<ScriptBoardHex> hexes;

    ScriptEngine engine;

    void Start()
    {
        engine = this.gameObject.GetComponent<ScriptEngine>();
    }


    public void Save()
    {
        //loop through hexes
        foreach (ScriptBoardHex hex in hexes)
        {
            //export type and value
        }

        //loop through players
        //foreach (ScriptPlayer player in engine.players)
        //{

        //}

        ////loop through corners
        //foreach (ScriptBoardCorner corner in corners)
        //{
        //    //export owner
        //}

        ////Loop through edges
        //foreach (ScriptBoardEdge edge in edges)
        //{
        //    //export owner
        //}
    }

}
