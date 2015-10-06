﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @author Mike Dobson
/// This is the basic state machine for the Settlers of not catan game for project 4, 
/// this machine will transfer the game from one phase to the next and run the content 
/// for each of the phases.
/// </summary>

public enum GameState
{
    PHASE0,
    PHASE1,
    PHASE2,
    PHASE3,
    PHASE4,
    PHASE5,
    PHASE6
}

public enum StateCommands
{
    GOTO_PHASE0,
    GOTO_PHASE1,
    GOTO_PHASE2,
    GOTO_PHASE3,
    GOTO_PHASE4,
    GOTO_PHASE5,
    GOTO_PHASE6
}

public class ScriptEngine : MonoBehaviour {

    List<ScriptPlayer> players = new List<ScriptPlayer>();

    Dictionary<ScriptPhaseTransition, GameState> allTransitions; //a dictionary of phase transitions

    Dictionary<string, StateCommands> enumParse;

    public GameState CurrentState { get; private set; } //the current state of the game

    public GameState PreviousState { get; private set; } //the previous state of the game

    public GameObject phase0menu; //the phase 0 menu

    public GameObject phase1menu; //the phase 1 menu

    public GameObject phase2menu; // the phase 2 menu

    public GameObject phase3menu; // the phase 3 menu

    public GameObject phase4menu; // the phase 4 menu

    public GameObject phase5menu; // the phase 5 menu

    public GameObject phase6menu; // the phase 6 menu

	// Use this for initialization
	void Start () {

        players.Add(new ScriptPlayer("Mike"));
	    //setup the current state
        CurrentState = GameState.PHASE0;

        //setup the previous state
        PreviousState = GameState.PHASE0;

        //create the dictionary
        allTransitions = new Dictionary<ScriptPhaseTransition, GameState>
        {
            //Defines the state transitions where
            //{new ScriptPhaseTransition(actual state of the machine, transition state/command), final state of the machine)}
            {new ScriptPhaseTransition(GameState.PHASE0, StateCommands.GOTO_PHASE5), GameState.PHASE5 },
            {new ScriptPhaseTransition(GameState.PHASE1, StateCommands.GOTO_PHASE2), GameState.PHASE2 },
            {new ScriptPhaseTransition(GameState.PHASE2, StateCommands.GOTO_PHASE3), GameState.PHASE3 },
            {new ScriptPhaseTransition(GameState.PHASE3, StateCommands.GOTO_PHASE4), GameState.PHASE4 },
            {new ScriptPhaseTransition(GameState.PHASE4, StateCommands.GOTO_PHASE5), GameState.PHASE5 },
            {new ScriptPhaseTransition(GameState.PHASE5, StateCommands.GOTO_PHASE1), GameState.PHASE1 },
            {new ScriptPhaseTransition(GameState.PHASE5, StateCommands.GOTO_PHASE6), GameState.PHASE6 }
        };

        //Create the dictionary where
        //{string that is passed by the button, command the string represents
        enumParse = new Dictionary<string, StateCommands>
        {
            {"goto phase 0", StateCommands.GOTO_PHASE0},
            {"goto phase 1", StateCommands.GOTO_PHASE1},
            {"goto phase 2", StateCommands.GOTO_PHASE2},
            {"goto phase 3", StateCommands.GOTO_PHASE3},
            {"goto phase 4", StateCommands.GOTO_PHASE4},
            {"goto phase 5", StateCommands.GOTO_PHASE5},
            {"goto phase 6", StateCommands.GOTO_PHASE6}
        };

        Debug.Log("Current state: " + CurrentState);
        Phase0();
	}
	
	GameState GetNext(StateCommands command)
    {
        //construct transition based on machine current state and the command
        ScriptPhaseTransition newTransition = new ScriptPhaseTransition(CurrentState, command);

        //store the location to got to here
        GameState newState;

        if(!allTransitions.TryGetValue(newTransition, out newState))
            throw new UnityException("Invalid Game State transition " + CurrentState + " -> " + command);

        //return the new state
        return newState;
    }

    public void MoveNextAndTransition(string command)
    {
        //record the previous state of the machine
        PreviousState = CurrentState;

        //location for the new command
        StateCommands newCommand;

        //try to get the value of the command
        if (!enumParse.TryGetValue(command, out newCommand))
            throw new UnityException("Invalid command  -> " + command);

        //setup the new state
        CurrentState = GetNext(newCommand);

        //transition the game to the next state
        Transition();

        Debug.Log("Transitioning from " + PreviousState + " -> " + CurrentState);
    }

    void Transition()
    {
        switch(PreviousState)
        {
            case GameState.PHASE0:
                Phase5();
                break;
            case GameState.PHASE1:
                Phase2();
                break;
            case GameState.PHASE2:
                Phase3();
                break;
            case GameState.PHASE3:
                Phase4();
                break;
            case GameState.PHASE4:
                Phase5();
                break;
            case GameState.PHASE5:
                if(CurrentState == GameState.PHASE1)
                {
                    Phase1();
                }
                else
                {
                    Phase6();
                }
                break;
        }
    }

    void Phase0()
    {
        MoveNextAndTransition("goto phase 5");
    }

    void Phase1()
    {
        int diceRoll = Random.Range(1, 6);
        Debug.Log("Dice Roll " + diceRoll);
        MoveNextAndTransition("goto phase 2");
    }

    void Phase2()
    {
        MoveNextAndTransition("goto phase 3");
    }

    void Phase3()
    {
        MoveNextAndTransition("goto phase 4");
    }

    void Phase4()
    {
        MoveNextAndTransition("goto phase 5");
    }

    void Phase5()
    {
        foreach(ScriptPlayer player in players)
        {
            if (player.NumSettlements > (players.Count * 1.25))
            {
                MoveNextAndTransition("goto phase 6");
            }
            else
            {
                MoveNextAndTransition("goto phase 1");
            }
        }
    }

    void Phase6()
    {
        Application.Quit();
    }
}