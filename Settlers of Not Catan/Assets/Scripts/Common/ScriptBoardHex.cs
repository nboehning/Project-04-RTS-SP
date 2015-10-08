using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptBoardHex : MonoBehaviour {

    public GameObject cornerPrefab;
    public GameObject edgePrefab;
    public ScriptEngine engine;
    public float hexSideLength;
    public int hexDieValue;
    public HexType resource;

    List<ScriptBoardCorner> cornerScripts = new List<ScriptBoardCorner>(0);    
    
    void Start()
    {
        engine = GameObject.Find("Game Engine").GetComponent<ScriptEngine>();
    }


    public void GrantResources()
    {
        foreach (ScriptBoardCorner corner in cornerScripts)
        {
            //if (corner.owner.PlayerName == engine.players[0].PlayerName)
            //{
            //    //tell the settlement to add 1 of blah resources to owning player.
            //}
        }
    }

    public void CheckAndGenerateCorners()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 6; i++)
        {
            //@ref Nathan
            float angle = Mathf.PI * (60f * i) / 180;
            Vector3 cornerPos = new Vector3((center.x + hexSideLength * Mathf.Cos(angle)),
                                    (center.y + hexSideLength * Mathf.Sin(angle)), (center.z - 1));
            //@endRef Nathan


            Collider[] hitColliders = Physics.OverlapSphere(cornerPos, .125f);
            bool settlementFound = false;
            foreach (Collider other in hitColliders)
            {
                if (other.tag == "Settlement")
                {
                    settlementFound = true;
                    cornerScripts.Add(other.gameObject.GetComponent<ScriptBoardCorner>());
                }
            }
            if(!settlementFound)
            {
                GameObject temp = (GameObject)Instantiate(cornerPrefab, cornerPos, Quaternion.identity);
                cornerScripts.Add(temp.GetComponent<ScriptBoardCorner>());
            }
        }
    }
}
