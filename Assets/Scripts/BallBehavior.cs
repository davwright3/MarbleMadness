using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BallBehavior : NetworkBehaviour
{
    //materials for changing color based on userId
    [SerializeField] Material redBallMaterial;
    [SerializeField] Material blueBallMaterial;

    //the gameobject itself and its rigidbody for physics and materials
    [SerializeField] GameObject marble;
    [SerializeField] Rigidbody body;

    //initial settings for the motion of the ball
    [SerializeField] NetworkVariable<Vector3> startVector = new NetworkVariable<Vector3>();
    [SerializeField] float drag = 0.5f;

    private ulong ownerId;
    private int points = 0;
        


    public void Start() {
        ownerId = NetworkManager.Singleton.LocalClientId;

        Renderer renderer = marble.GetComponentInChildren<Renderer>();

        if (OwnerClientId == 0)
        {
            renderer.material = redBallMaterial;
        }
        else if (OwnerClientId == 1) {
            renderer.material = blueBallMaterial;
        }

        body.AddForce(startVector.Value, ForceMode.Impulse);
        body.drag = drag;

    }

    public void Update()
    {


    }

    public void InitializeBall(Vector3 initVector) { 
        startVector.Value = initVector;
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        NetworkObject collider = collision.gameObject.GetComponent<NetworkObject>();
        ulong colliderId = 0;

        if (collider != null)
        {
            colliderId = collider.OwnerClientId;
        }


        if (collision.gameObject.tag == "Wall")
        {
            points -= 1;
            Debug.Log("Collilded with " + collision.gameObject.tag + ":  " + points + " points");
        }
        else if (collision.gameObject.tag == "Ball" && colliderId != ownerId)
        {

            points += 5;
            Debug.Log("Collilded with " + collision.gameObject.tag + ":  " + points + " points");
        }
        else if (collision.gameObject.tag == "Ball" && colliderId == ownerId)
        {

            points -= 1;
            Debug.Log("Collilded with " + collision.gameObject.tag + ":  " + points + " points");
        }
    }
    
        

}
