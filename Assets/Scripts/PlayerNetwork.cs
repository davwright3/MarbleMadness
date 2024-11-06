using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Transform spawnedBall;

    [SerializeField] private NetworkVariable<Vector3> initialVector = new NetworkVariable<Vector3>();

    //[SerializeField] Button upButton;

    [SerializeField] NetworkManager manager;
    [SerializeField] GameObject gameManager;
    TurnCounter turnCounterVar;
    [SerializeField] int turnId;
    [SerializeField] int playerID;

    public int playerScore = 0;

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + "; randomNumber:  " + randomNumber.Value);
        };

        StartCoroutine(RunScoreCalculation());
        playerID = (int)OwnerClientId;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager"); 
        turnCounterVar = gameManager.GetComponent<TurnCounter>();
        

    }

    private void Update()
    {
        //make sure that only the owner of the player can run the code
        if (!IsOwner) return;

        //get the current turn id from the turn counter
        turnId = turnCounterVar.GetTurnCounter();

        //verify that the current player is active
        if (playerID == turnId % 2)
        {


            if (Input.GetMouseButtonDown(0))
            {


                //use a ray to make sure the mouse is hitting a legal position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //check to see if it is the player's turn
                if (Physics.Raycast(ray, out hit))
                {

                    SpawnBallServerRpc(hit.point, NetworkManager.Singleton.LocalClientId);
                    EndTurnServerRpc();

                }

            }

        } 
           
    }


    //spawn the ball using the input from the button controller
    [ServerRpc(RequireOwnership =false)]
    void SpawnBallServerRpc(Vector3 spawnPosition, ulong clientId) {

        //initial vector from the player object        
        Vector3 vectorInit = initialVector.Value;   
        Transform spawnedBallTransform = Instantiate(spawnedBall, spawnPosition, Quaternion.identity);
        BallBehavior newBall = spawnedBallTransform.GetComponent<BallBehavior>();
        newBall.InitializeBall(vectorInit);
        newBall.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        

    }


    //getting the initial vector from the button controller
    [ServerRpc(RequireOwnership =false)]
    public void SetInitialVectorFromUIServerRpc(Vector3 sentInitialVector) {
        initialVector.Value = sentInitialVector;
    
    }

    //coroutine to delay calculating the score
    private IEnumerator RunScoreCalculation() {

        while (true)
        {
            Debug.Log("Calculating Score");
            CalculatePlayerScoreServerRpc();

            yield return new WaitForSeconds(2.0f);
        }
    }

    //server rpc to update the turn counter
    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc() {

        TurnCounter turnCounter = gameManager.GetComponent<TurnCounter>();
        turnCounter.IncrementTurnCounter();
        
    }

    //score needs to go through all of the marbles that belong to the player and add up their points
    [ServerRpc(RequireOwnership =false)]
    void CalculatePlayerScoreServerRpc() {
        playerScore = 0;

        //find all of the balls in the scene
        GameObject[] taggedBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject taggedBall in taggedBalls) {
            int playerId = (int)OwnerClientId;

            NetworkObject selectBall = taggedBall.GetComponent<NetworkObject>();
            int ballId = (int)selectBall.OwnerClientId;

            if (ballId == playerId) {
                BallBehavior ballToScore = taggedBall.GetComponent<BallBehavior>();
                int ballScore = ballToScore.GetPoints();
                playerScore += ballScore;
            }
        }

        DisplayScoreClientRpc(playerScore);
        
    }

    //send the score display to all the client
    [ClientRpc]
    void DisplayScoreClientRpc(int playerScore) {
        Debug.Log("Player " + OwnerClientId + "'s score is: " + playerScore);
    }
    
}
