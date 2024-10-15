using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ButtonController : MonoBehaviour
{
    //fields for the buttons
    [SerializeField] Button upButton;

    //so we can find the local player
    private PlayerNetwork networkedPlayer;

    //vector for setting movement
    private Vector3 initialMovement = new Vector3(5f, 0f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        //FindLocalPlayer();
        
    }

    public void OnUpButtonClicked() {
        FindLocalPlayer();
        //networkedPlayer.SetInitialVectorFromUI(initialMovement);
        Debug.Log("Up Button Clicked");
        networkedPlayer.SetInitialVectorFromUI(new Vector3(0f, 0f, 10f));
        
    
    }

    void FindLocalPlayer() {
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var p in players) {
            if (p.IsOwner) {
                networkedPlayer = p;
                break;
            }
        }

        NetworkObject localPlayer = networkedPlayer.gameObject.GetComponent<NetworkObject>();
        ulong localPlayerId = localPlayer.OwnerClientId;
        Debug.Log("Local user is: " +  localPlayerId);
    }
    
}
