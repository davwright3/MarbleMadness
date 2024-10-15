using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Transform spawnedBall;

    [SerializeField] private Vector3 initialVector = new Vector3(1f, 0f, 1f);

    [SerializeField] Button upButton;

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + "; randomNumber:  " + randomNumber.Value);
        };
    }

    private void Update()
    {
        

        if (!IsOwner) return;




        if (Input.GetMouseButtonDown(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {

                SpawnBallServerRpc(hit.point, NetworkManager.Singleton.LocalClientId);
            }

            
            //Transform spawnedBallTransform = Instantiate(spawnedBall);
            //spawnedBallTransform.GetComponent<NetworkObject>().Spawn(true);
        
        }
        
        /*Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;

        float moveSpeed = 3f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;*/
    }



    [ServerRpc]
    void SpawnBallServerRpc(Vector3 spawnPosition, ulong clientId) {

                
        Transform spawnedBallTransform = Instantiate(spawnedBall, spawnPosition, Quaternion.identity);
        BallBehavior newBall = spawnedBallTransform.GetComponent<BallBehavior>();
        newBall.InitializeBall(initialVector);
        newBall.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        

    }

    public void SetInitialVectorFromUI(Vector3 sentInitialVector) {
        initialVector = sentInitialVector;
    
    }
    
}
