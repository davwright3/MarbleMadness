using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallBehavior : MonoBehaviour
{

    [SerializeField] Vector3 movDir = new Vector3(0, 0, 0);
    [SerializeField] float speed = 3.0f;
    [SerializeField] float deceleration = 0.00001f;
    [SerializeField] Vector3 startVector = new Vector3(1, 0, 1);

    
    public void OnNetworkSpawn() { 

        
    }

    public void Update()
    {
        this.transform.position += startVector * speed * Time.deltaTime;

        if (this.speed > 0)
        {
            this.speed = this.speed - this.deceleration;
        }
        
    }

}
