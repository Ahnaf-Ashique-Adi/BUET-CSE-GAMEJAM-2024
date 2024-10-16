using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScoreAlocator : MonoBehaviour
{
    [SerializeField]
    private int _killScore;


    private ScoreController _scoreController;


    private void Awake()
    {
        _scoreController = FindObjectOfType<ScoreController>();
        
    }


    public void AllocateScore() 
    {
        _scoreController.AddScore(_killScore);
    }
}
