using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallBehavior : NetworkBehaviour
{

    public Material redBallMaterial;
    public Material blueBallMaterial;
    
    public GameObject marble;
    public Rigidbody body;

    [SerializeField] Vector3 startVector = new Vector3(5f, 0, 2f);
    public float drag = 0.5f;

    
    

    
    public void Start() {
        Renderer renderer = marble.GetComponentInChildren<Renderer>();

        if (OwnerClientId == 0)
        {
            renderer.material = redBallMaterial;
        }
        else if (OwnerClientId == 1) { 
            renderer.material = blueBallMaterial;
        }

        body.AddForce(startVector, ForceMode.Impulse);
        body.drag = drag;
        
    }

    public void Update()
    {
        

        /*if (this.speed > 0)
        {
            this.speed = this.speed - this.deceleration;
        }*/
        
    }

}
