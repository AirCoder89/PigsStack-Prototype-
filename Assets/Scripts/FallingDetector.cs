using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDetector : MonoBehaviour
{
    public delegate void DetectorEvents();
    public static event DetectorEvents OnDetectBlock;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            OnDetectBlock?.Invoke();
        }
    }
}
