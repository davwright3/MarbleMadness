using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Transform spawnedBall;

    [SerializeField] private NetworkVariable<Vector3> initialVector = new NetworkVariable<Vector3>();

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



    [ServerRpc(RequireOwnership =false)]
    void SpawnBallServerRpc(Vector3 spawnPosition, ulong clientId) {

        Vector3 vectorInit = initialVector.Value;   
        Transform spawnedBallTransform = Instantiate(spawnedBall, spawnPosition, Quaternion.identity);
        BallBehavior newBall = spawnedBallTransform.GetComponent<BallBehavior>();
        newBall.InitializeBall(vectorInit);
        newBall.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        

    }

    
    public void SetInitialVectorFromUI(Vector3 sentInitialVector) {
        initialVector.Value = sentInitialVector;
    
    }
    
}
