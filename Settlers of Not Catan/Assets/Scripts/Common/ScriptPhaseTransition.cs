using UnityEngine;
using System.Collections;

public class ScriptPhaseTransition {

    GameState currentPhase;

    StateCommands command;

    public ScriptPhaseTransition(GameState thisPhase, StateCommands thisCommand)
    {
        currentPhase = thisPhase;
        command = thisCommand;
    }
    
}
