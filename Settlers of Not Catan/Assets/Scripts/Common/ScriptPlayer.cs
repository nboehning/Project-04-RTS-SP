using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptPlayer : MonoBehaviour {

    public List<GameObject> settlements;

    public List<GameObject> roads;

    public string PlayerName
    {
        get;
        set;
    }

    public int NumSettlements
    {
        get;
        set;
    }

    public ScriptPlayer(string Name)
    {
        PlayerName = Name;
        NumSettlements = 0;
    }
}
