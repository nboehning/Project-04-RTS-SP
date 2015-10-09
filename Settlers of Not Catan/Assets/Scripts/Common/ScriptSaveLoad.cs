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
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + "/saves/saveGame.txt"))
        {
            //loop through hexes
            writer.WriteLine(hexes.Count);
            foreach (ScriptBoardHex hex in hexes)
            {
                //export type and value
            }

        //loop through players
        //foreach (ScriptPlayer player in engine.players)
        //{

        //}

            //loop through players
            writer.WriteLine(engine.players.Count);
            foreach (ScriptPlayer player in engine.players)
            {
                writer.Write(player.NumLumber + ",");
                writer.Write(player.NumBrick + ",");
                writer.Write(player.NumWheat + ",");
                writer.Write(player.NumWool + ",");
                writer.Write(player.NumSettlements + ",");
                writer.Write(player.roads.Count + ",");
                writer.WriteLine(player.PlayerName);
                
                foreach (GameObject settlement in player.settlements)
                {
                    writer.WriteLine(settlement.transform.position.x
                                    + "," + settlement.transform.position.y
                                    + "," + settlement.transform.position.z);
                }

                foreach (GameObject road in player.roads)
                {
                    writer.WriteLine(road.transform.position.x
                                    + "," + road.transform.position.y
                                    + "," + road.transform.position.z);
                }

            }

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

}
