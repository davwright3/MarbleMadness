using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBallDirection : MonoBehaviour
{
    [SerializeField] GameObject ball;
    
    [SerializeField] Button leftButton;
    [SerializeField] Button upLeftButton;
    [SerializeField] Button upButton;
    [SerializeField] Button upRightButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button downRightButton;
    [SerializeField] Button downButton;
    [SerializeField] Button downLeftButton;

    private void OnLeftButtonClicked() {
        Vector3 startVector = new Vector3(5f, 0, 0);

        
    
    }

}
