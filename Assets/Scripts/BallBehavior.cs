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
    [SerializeField] private NetworkVariable<Vector3> startVector = new NetworkVariable<Vector3>();
    [SerializeField] private NetworkVariable<int> startSpeed = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone);
    [SerializeField] float drag = 0.5f;

    [SerializeField] ulong ownerId;
    [SerializeField] int points = 0;

    private float collisionVelocity;
        

    //setting up the color of the balls when they spawn on the network
    public override void OnNetworkSpawn() {
        ownerId = NetworkManager.Singleton.LocalClientId;

        Renderer renderer = marble.GetComponentInChildren<Renderer>();

        if (OwnerClientId == 0)
        {
            renderer.material = redBallMaterial;
        }
        else if (OwnerClientId == 1) {
            renderer.material = blueBallMaterial;
        }

        body.AddForce(startVector.Value * startSpeed.Value, ForceMode.Impulse);
        body.drag = drag;

    }

    private void FixedUpdate()
    {
        collisionVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;
    }


    //method to initialize the ball from the player script
    public void InitializeBall(Vector3 initVector, int speed) { 
        startVector.Value = initVector;
        startSpeed.Value = speed;
    
    }

    //collision detection for getting points.  need to make sure to ignore the floor since the balls bounce a bit when they collide so it causes issues
    private void OnCollisionEnter(Collision collision)
    {
        
        NetworkObject collider = collision.gameObject.GetComponent<NetworkObject>();
        ulong colliderId = 0;

        NetworkObject thisObject = this.gameObject.GetComponent<NetworkObject>();
        ulong thisID = thisObject.OwnerClientId;

        if (collider != null)
        {
            colliderId = collider.OwnerClientId;
        }

        if (collision.gameObject.tag == "Ball" && colliderId != thisID)
        {
 
            points += (int)(collisionVelocity);
            Debug.Log(thisID + " Collided with other ball: " + collisionVelocity);
 
        }
        else if (collision.gameObject.tag == "Ball" && colliderId == thisID)
        {

            points -= 1;
            Debug.Log("Collilded with own ball: " + collision.gameObject.tag + ":  " + points + " points");
        }
        else if (collision.gameObject.tag == "Wall")
        {
            points -= 1;
            Debug.Log("Collilded with wall" + collision.gameObject.tag + ":  " + points + " points");
        }
    }


    public int GetPoints() { 
        return points;
    }
        

}
