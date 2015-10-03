using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @author Marshall Mason
/// </summary>
public class SaveGame : MonoBehaviour {


    Engine engine;

    void Start()
    {
        engine = this.gameObject.GetComponent<Engine>();
    }


    public void Save()
    {
        //loop through hexes
        foreach (BoardHex hex in engine.hexes)
        {
            //export type and value
        }

        //loop through corners
        foreach(BoardCorner corner in engine.corners)
        {
            //export owner
        }

        //Loop through edges
        foreach(BoardEdge edge in engine.edges)
        {
            //export owner
        }
    }

}
