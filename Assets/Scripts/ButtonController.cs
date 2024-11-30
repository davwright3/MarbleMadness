using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEditor;

public class ButtonController : MonoBehaviour
{
    //fields for the buttons
    [SerializeField] Button upButton;

    //so we can find the local player
    private PlayerNetwork networkedPlayer;

    //vector for setting movement
    private Vector3 initialMovement = new Vector3(5f, 0f, 0f);

      

    [ServerRpc(RequireOwnership =false)]
    public void OnUpButtonClickedServerRpc() {
        

        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(0f, 0f, 10f));
        }
        
    
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnUpRightButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(6f, 0f, 6f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnRightButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(10f, 0f, 0f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnDownRightButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(6f, 0f, -6f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnDownButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(0f, 0f, -10f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnDownLeftButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(-6f, 0f, -6f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnLeftButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(-10f, 0f, 0f));
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnUpLeftButtonClickedServerRpc()
    {
        
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            Debug.Log("Up Button Clicked");
            player.SetInitialVectorFromUIServerRpc(new Vector3(-6f, 0f, 6f));
        }


    }

    [ServerRpc(RequireOwnership =false)]
    public void OnFastButtonClickedServerRpc()
    {
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            player.SetInitialSpeedFromUIServerRpc(3);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnMediumButtonClickedServerRpc()
    {
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players)
        {
            player.SetInitialSpeedFromUIServerRpc(2);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnSlowButtonClickedServerRpc()
    {
        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();

        foreach (var player in players )
        {
            player.SetInitialSpeedFromUIServerRpc(1);
        }
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
