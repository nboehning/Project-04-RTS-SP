using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptBoardHex : MonoBehaviour {

    public GameObject cornerPrefab;
    public GameObject edgePrefab;
    public float hexSideLength;

    List<ScriptBoardCorner> cornerScripts = new List<ScriptBoardCorner>(0);    
    
    public void CheckAndGenerateCorners()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 6; i++)
        {
            //@ref Nathan
            float angle = Mathf.PI * (60f * i + 30) / 180;
            Vector3 cornerPos = new Vector3((center.x + hexSideLength * Mathf.Cos(angle)),
                                    (center.y + hexSideLength * Mathf.Sin(angle)), (center.z + 1));
            //@endRef Nathan
            if (Physics.CheckSphere (cornerPos, .125f))
            {

            }
        }
    }
}
