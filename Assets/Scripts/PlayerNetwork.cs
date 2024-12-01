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

    [SerializeField] private NetworkVariable<Vector3> _initialVector = new NetworkVariable<Vector3>();
    [SerializeField] private NetworkVariable<int> _initialSpeed = new NetworkVariable<int>(1);

    private GameObject _gameManager;
    private Transform _bucket0;
    private Transform _bucket1;

    //[SerializeField] Button upButton;

    [SerializeField] NetworkManager manager;
    [SerializeField] GameObject gameManager;
    TurnCounter turnCounterVar;
    [SerializeField] int turnId;
    [SerializeField] int playerID;

    //public NetworkVariable<int> player0Score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    //public NetworkVariable<int> player1Score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);


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

        _gameManager = GameObject.Find("GameManager");
        _bucket0 = _gameManager.GetComponent<Transform>().Find("Player_0_Bucket"); 
        _bucket1 = _gameManager.GetComponent<Transform>().Find("Player_1_Bucket");

        if (_gameManager == null)
        {
            Debug.Log("GameManager is NULL");
        }

        if (_bucket0 == null)
        {
            Debug.Log("bucket0 is NULL");
        }

        if (_bucket1 == null)
        {
            Debug.Log("bucket1 is NULL");
        }
        

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
        Vector3 vectorInit = _initialVector.Value;   
        int startSpeed = _initialSpeed.Value;
        
        Transform spawnedBallTransform = Instantiate(spawnedBall, spawnPosition, Quaternion.identity);
        BallBehavior newBall = spawnedBallTransform.GetComponent<BallBehavior>();
        newBall.InitializeBall(vectorInit, startSpeed);
        newBall.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

        if (clientId == 0)
        {
            newBall.transform.parent = _bucket0;
            
        }
        else if (clientId == 1)
        {
            newBall.transform.parent = _bucket1;
        }
        
    }


    //getting the initial vector from the button controller
    [ServerRpc(RequireOwnership =false)]
    public void SetInitialVectorFromUIServerRpc(Vector3 sentInitialVector) {
        _initialVector.Value = sentInitialVector;
    
    }

    [ServerRpc(RequireOwnership =false)]
    public void SetInitialSpeedFromUIServerRpc(int speed)
    {
        _initialSpeed.Value = speed;
    }

    //coroutine to delay calculating the score
    private IEnumerator RunScoreCalculation() {

        while (true)
        {
            //Debug.Log("Calculating Score");
            CalculatePlayerScoreServerRpc();

            yield return new WaitForSeconds(5.0f);
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
        


        //update the network variable
        if (playerID == 0)
        {
            int thisScore = 0;

            foreach (Transform child in _bucket0)
            {
                thisScore += child.GetComponent<BallBehavior>().GetPoints();
            }

            _gameManager.GetComponent<GameManager>().SetPlayer0Score(thisScore);
        }
        else if (playerID == 1)
        {
            int thisScore = 0;

            foreach (Transform child in _bucket1)
            {
                thisScore += child.GetComponent<BallBehavior>().GetPoints();
            }

            _gameManager.GetComponent<GameManager>().SetPlayer1Score(thisScore);
        }

        
    }

    //send the score display to all the client
    [ClientRpc]
    void DisplayScoreClientRpc(int playerScore) {
        Debug.Log("Player " + OwnerClientId + "'s score is: " + playerScore);
    }
    
}
