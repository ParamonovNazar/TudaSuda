using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBall : MonoBehaviour
{
    private const string PLAYER_TAG="Player";

    [SerializeField] private int collisionPoint = 1; 
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(PLAYER_TAG))
        {
            GameManager.Instance.Progress += collisionPoint;
        }
         
        GameManager.Instance.FallingBallEnded(this); 
    }
}
