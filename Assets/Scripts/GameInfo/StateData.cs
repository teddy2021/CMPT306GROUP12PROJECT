using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class StateData
{    
    public int floorsExplored;
    public List<string> floorCodes;

    public string lastPlayedText;

    public StateData (State state)
    {
        floorsExplored = state.floorsExplored;
        floorCodes = state.floorCodes;

        lastPlayedText = state.lastPlayedText;
    }

}
