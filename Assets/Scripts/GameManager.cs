using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private NetworkVariable<int> _player0Score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    [SerializeField]
    private NetworkVariable<int> _player1Score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    [SerializeField]
    private TMP_Text _player0ScoreObject;
    [SerializeField]
    private TMP_Text _player1ScoreObject;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer0Score(int score)
    { 
        _player0Score.Value = score;
        _player0ScoreObject.text = _player0Score.Value.ToString();
    }


    public void SetPlayer1Score(int score)
    { 
        _player1Score.Value = score;
        _player1ScoreObject.text = _player1Score.Value.ToString();
    }
}
