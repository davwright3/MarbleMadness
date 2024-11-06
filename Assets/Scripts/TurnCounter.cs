using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TurnCounter : NetworkBehaviour
{
    //create a number for tracking the turn
    [SerializeField] NetworkVariable<int> turnCounter = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    //method for the players to check whose turn it is when they try to take their move
    public int GetTurnCounter() { 
        return turnCounter.Value;
    }

    //method for incrementing the turn counter
    public void IncrementTurnCounter() {
        turnCounter.Value += 1;
    }
    


}
